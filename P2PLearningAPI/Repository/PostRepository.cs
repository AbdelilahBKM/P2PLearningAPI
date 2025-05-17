using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.DTOsInput;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class PostRepository : IPostInterface
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(P2PLearningDbContext context, ITokenService tokenService, ILogger<PostRepository> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        // Get all posts
        public ICollection<PostDTO> GetPosts()
        {   
            ICollection<Post> posts = _context.Posts
                .Include(p => p.PostedBy)
                .ToList();
            if (posts == null || posts.Count == 0)
            {
                _logger.LogWarning("No posts found in the database.");
                return new List<PostDTO>();
            }
            return posts.Select<Post, PostDTO>(post => post switch
            {
                Question question => QuestionDTO.FromQuestion(question),
                Answer answer => AnswerDTO.FromAnswer(answer),
                _ => throw new InvalidOperationException($"Unexpected post type: {post.GetType()}")
            }).ToList();
        }

        // Get a single post by ID
        // Get a single post by ID
        public PostDTO? GetPost(long id)
        {
            // Get the Post by ID, including related entities (PostedBy, Answers, etc.)
            var post = _context.Posts
                .Include(p => p.PostedBy) // Always include PostedBy
                .FirstOrDefault(p => p.Id == id);

            // If the post is not found, return null
            if (post == null)
            {
                return null;
            }

            // Check if the post is a Question and return the associated data
            if (post is Question question)
            {
                Question? questionRes = _context.Questions
                    .Include(q => q.Answers)
                    .ThenInclude(a => a.PostedBy)
                    .Include(q => q.Discussion)
                    .FirstOrDefault(q => q.Id == id);
                if (questionRes == null)
                    return null;
                return QuestionDTO.FromQuestion(questionRes);
            }
            else if (post is Answer answer)
            {
                Answer? answerRes = _context.Answers
                    .Include(a => a.Replies)        
                    .Include(a => a.Question)       
                    .Include(a => a.PostedBy)
                    .FirstOrDefault(a => a.Id == id);
                if (answerRes == null)
                    return null;
                return AnswerDTO.FromAnswer(answerRes);
            }

            // If it's neither a Question nor an Answer, return null
            return null;
        }


        // Get posts by user ID
        public ICollection<PostDTO> GetPostsByUser(string userId)
        {
            ICollection<Post> posts = _context.Posts
                .Include(p => p.PostedBy)
                .Where(p => p.UserID == userId).ToList();
            if (posts == null || posts.Count == 0)
            {
                _logger.LogWarning($"No posts found for user with ID: {userId}");
                return new List<PostDTO>();
            }
            return posts.Select<Post, PostDTO>(post => post switch
            {
                Question question => QuestionDTO.FromQuestion(question),
                Answer answer => AnswerDTO.FromAnswer(answer),
                _ => throw new InvalidOperationException($"Unexpected post type: {post.GetType()}")
            }).ToList();
        }

        // Check if a post exists by ID
        public bool CheckPostExist(long id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        // Create a new post
        public PostDTO CreatePost(PostCreateDTO postDTO, string token)
        {
            var (userId, _) = _tokenService.DecodeToken(token);
            if (userId != postDTO.PostedBy)
                throw new UnauthorizedAccessException();

            var newPost = CreatePostFromDTO(postDTO, postDTO.PostType);
            _context.Posts.Add(newPost);
            if (postDTO.PostType == PostType.Question)
            {
                Discussion discussion = _context.Discussions.FirstOrDefault(d => d.Id == postDTO.DiscussionId)!;
                if (discussion == null)
                    throw new InvalidOperationException("Discussion doesn't exist");
                discussion.Number_of_posts++;
            }
            if (!Save())
                throw new InvalidOperationException("Failed to save the post");
            return GetPost(newPost.Id)!;
        }

        // Update an existing post
        public bool UpdatePost(PostUpdateDTO postDTO, string token)
        {
            if(postDTO == null)
                throw new ArgumentNullException(nameof(postDTO));
            if (!CheckPostExist(postDTO.Id))
                throw new InvalidOperationException("Post doesn't exist");
            var post = _context.Posts.Include(p => p.PostedBy).FirstOrDefault(p => p.Id == postDTO.Id);
            if (post == null)
                throw new InvalidOperationException("Post doesn't exist");
            (string userId, _) = _tokenService.DecodeToken(token);
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to update this post.");
            post.Title = postDTO.Title;
            post.Content = postDTO.Content;
            return Save();
        }

        // Close a post by setting IsClosed to true
        public bool ClosePost(long id, string token)
        {
            var post = _context.Posts.Include(p => p.PostedBy).FirstOrDefault(p => p.Id == id);
            var (userId, _) = _tokenService.DecodeToken(token);
            if (post == null)
                throw new InvalidOperationException("Post doesn't exist");
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to close this post.");
            post.IsClosed = true;
            return Save();
            
        }

        // Reopen a post by setting IsClosed to false
        public bool ReopenPost(long id, string token)
        {
            var post = _context.Posts.Include(p => p.PostedBy).FirstOrDefault(p => p.Id == id);
            if (post == null)
                return false;
            var (userId, _) = _tokenService.DecodeToken(token);
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to reopen this post.");
            if (!post.IsClosed)
                throw new InvalidOperationException("Post is already open");
            post.IsClosed = false;
            return Save();
        }

        public bool MarkPostAsSolved(long id, string token)
        {
            var post = _context.Posts.Include(p => p.PostedBy).FirstOrDefault(p => p.Id == id);
            if (post == null)
                return false;
            var (userId, _) = _tokenService.DecodeToken(token);
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to mark this post as solved.");

            if (post is not Question question)
                throw new InvalidOperationException("Post is not a question");

            question.isAnswered = true;

            return Save();
        }

        // Vote on a post (adjust Reputation)
        public bool VoteOnPost(long postId, Vote vote)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null)
                return false;

            post.AddVote(vote);
            return Save();
        }

        public bool DeleteVote(long postId, Vote vote)
        {
            var post = _context.Posts.Include(p => p.PostedBy).FirstOrDefault(p => p.Id == postId);
            if(post == null)
                return false;
            post.RemoveVote(vote);
            return Save();
        }

        // Delete a post by ID
        public bool DeletePost(long id, string token)
        {
            var post = _context.Posts.Include(p => p.PostedBy).FirstOrDefault(p => p.Id == id);
            if (post == null)
                throw new InvalidOperationException("Post doesn't exist");
            var (userId, _) = _tokenService.DecodeToken(token);
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to delete this post.");
            _context.Posts.Remove(post);
            return Save();
        }
        public bool MarkAsBestAnswer(long id, string token)
        {
            var (userId, _) = _tokenService.DecodeToken(token);
            var answer = _context.Answers.FirstOrDefault(a => a.Id == id);
            if (answer == null)
                throw new InvalidOperationException("Answer doesn't exist");
            var question = _context.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null)
                throw new InvalidOperationException("Question doesn't exist");
            answer.IsBestAnswer = true;
            question.isAnswered = true;
            return Save();
        }

        // Save changes to the database
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public Post CreatePostFromDTO(PostCreateDTO postDTO, PostType postType)
        {
            return postType switch
            {
                PostType.Question => new Question(
                    postDTO.Title,
                    postDTO.Content,
                    postDTO.PostedBy,
                    postDTO.DiscussionId ?? throw new ArgumentNullException(nameof(postDTO.DiscussionId), "DiscussionId is required for Question posts.")
                ),
                PostType.Answer => new Answer(
                    postDTO.Title,
                    postDTO.Content,
                    postDTO.PostedBy,
                    postDTO.QuestionId ?? throw new ArgumentNullException(nameof(postDTO.QuestionId), "QuestionId is required for Answer posts."),
                    postType
                    ),
                PostType.Reply => new Answer(
                    postDTO.Title,
                    postDTO.Content,
                    postDTO.PostedBy,
                    postDTO.AnswerId ?? throw new ArgumentNullException(nameof(postDTO.AnswerId), "AnswerId is required for Reply posts."),
                    postType
                    ),
                _ => throw new ArgumentException("Invalid post type", nameof(postType))
            };
        }

        
    }
}
