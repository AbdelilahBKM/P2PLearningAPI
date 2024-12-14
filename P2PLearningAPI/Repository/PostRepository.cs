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

        public PostRepository(P2PLearningDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Get all posts
        public ICollection<Post> GetPosts()
        {
            return _context.Posts.ToList();
        }

        // Get a single post by ID
        public Post? GetPost(long id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id)!;
        }

        // Get posts by user ID
        public ICollection<Post> GetPostsByUser(string userId)
        {
            return _context.Posts.Where(p => p.UserID == userId).ToList();
        }

        // Check if a post exists by ID
        public bool CheckPostExist(long id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        // Create a new post
        public Post CreatePost(PostDTO postDTO, PostType postType, string token)
        {
            if (postDTO == null) 
                throw new ArgumentNullException(nameof(postDTO));
            (string userId, _, _) = _tokenService.DecodeToken(token);
            if(userId != postDTO.PostedBy.Id)
                throw new UnauthorizedAccessException("User is not authorized to create this post.");
            Post newPost;
            switch (postType)
            {
                case PostType.Question:
                    if(postDTO.Discussion == null)
                        throw new ArgumentNullException(nameof(postDTO.Discussion));
                        newPost = new Question(postDTO.Title, postDTO.Content,postDTO.PostedBy, postDTO.Discussion);
                    break;
                case PostType.Answer:
                    if (postDTO.Question == null)
                        throw new ArgumentException(nameof(postDTO.Question));
                    newPost = new Answer(postDTO.Title, postDTO.Content, postDTO.PostedBy, postDTO.Question);
                    break;
                case PostType.Reply:
                    if (postDTO.Answer == null)
                        throw new ArgumentException(nameof(postDTO.Answer));
                    newPost = new Answer(postDTO.Title, postDTO.Content, postDTO.PostedBy, postDTO.Answer);
                    break;
                default:
                    throw new ArgumentException("Invalid post type", nameof(postType));
            }
            _context.Posts.Add(newPost);
            if(Save())
                return newPost;
            throw new InvalidOperationException("Failed to save the Post to the database.");
        }

        // Update an existing post
        public Post UpdatePost(Post post, string token)
        {
            if(post == null)
                throw new ArgumentNullException(nameof(post));
            if (!CheckPostExist(post.Id))
                throw new InvalidOperationException("Post doesn't exist");
            (string userId, _, _) = _tokenService.DecodeToken(token);
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
            var (userId, _, _) = _tokenService.DecodeToken(token);
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
            var (userId, _, _) = _tokenService.DecodeToken(token);
            if (userId != post.UserID)
                throw new UnauthorizedAccessException("User is not authorized to reopen this post.");
            if (!post.IsClosed)
                throw new InvalidOperationException("Post is already open");
            post.IsClosed = false;
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
            var (userId, _, userType) = _tokenService.DecodeToken(token);
            if (userId != post.UserID && userType != "Administrator")
                throw new UnauthorizedAccessException("User is not authorized to delete this post.");
            _context.Posts.Remove(post);
            return Save();
        }

        // Save changes to the database
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
