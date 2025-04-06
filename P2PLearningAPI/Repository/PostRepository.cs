using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOs;
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
        public ICollection<Post> GetPosts()
        {
            return _context.Posts
                .Include(p => p.PostedBy)
                .ToList();
        }

        // Get a single post by ID
        // Get a single post by ID
        public Post? GetPost(long id)
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
                return _context.Questions
                    .Include(q => q.Answers)        // Include Answers for the Question
                    .ThenInclude(a => a.PostedBy)   // Include PostedBy for the Answers
                    .Include(q => q.Discussion)     // Include Discussion for the Question
                    .FirstOrDefault(q => q.Id == id); // Use the ID to ensure you're getting the right Question
            }
            // Otherwise, check if it's an Answer and return the associated data
            else if (post is Answer answer)
            {
                return _context.Answers
                    .Include(a => a.Replies)        // Include Replies for the Answer
                    .Include(a => a.Question)       // Include the Question the Answer belongs to
                    .Include(a => a.PostedBy)       // Include PostedBy for the Answer
                    .FirstOrDefault(a => a.Id == id); // Use the ID to ensure you're getting the right Answer
            }

            // If it's neither a Question nor an Answer, return null
            return null;
        }


        // Get posts by user ID
        public ICollection<Post> GetPostsByUser(string userId)
        {
            return _context.Posts
                .Include(p => p.PostedBy)
                .Where(p => p.UserID == userId).ToList();
        }

        // Check if a post exists by ID
        public bool CheckPostExist(long id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        // Create a new post
        public Post CreatePost(PostDTO postDTO)
        {
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
        public Post UpdatePost(Post post, string token)
        {
            if(post == null)
                throw new ArgumentNullException(nameof(post));
            if (!CheckPostExist(post.Id))
                throw new InvalidOperationException("Post doesn't exist");
            (string userId, _) = _tokenService.DecodeToken(token);
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to update this post.");
            _context.Posts.Update(post);
            if(Save())
                return post;
            throw new InvalidOperationException("Failed to update Post  to the database");
        }

        // Close a post by setting IsClosed to true
        public bool ClosePost(long id, string token)
        {
            var post = GetPost(id);
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
            var post = GetPost(id);
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
            var post = GetPost(id);
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
            var post = GetPost(postId);
            if (post == null)
                return false;

            post.AddVote(vote);
            return Save();
        }
        public bool DeleteVote(long postId, Vote vote)
        {
            var post = GetPost(postId);
            if(post == null)
                return false;
            post.RemoveVote(vote);
            return Save();
        }

        // Delete a post by ID
        public bool DeletePost(long id, string token)
        {
            var post = GetPost(id);
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

        public Post CreatePostFromDTO(PostDTO postDTO, PostType postType)
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
