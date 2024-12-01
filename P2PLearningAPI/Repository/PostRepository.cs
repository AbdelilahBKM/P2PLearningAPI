using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class PostRepository : IPostInterface
    {
        private readonly P2PLearningDbContext _context;

        public PostRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        // Get all posts
        public ICollection<Post> GetPosts()
        {
            return _context.Posts.ToList();
        }

        // Get a single post by ID
        public Post GetPost(long id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id);
        }

        // Get posts by user ID
        public ICollection<Post> GetPostsByUser(long userId)
        {
            return _context.Posts.Where(p => p.UserID == userId).ToList();
        }

        // Check if a post exists by ID
        public bool CheckPostExist(long id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        // Create a new post
        public Post CreatePost(Post post)
        {
            _context.Posts.Add(post);
            Save();
            return post;
        }

        // Update an existing post
        public Post UpdatePost(Post post)
        {
            _context.Posts.Update(post);
            Save();
            return post;
        }

        // Close a post by setting IsClosed to true
        public bool ClosePost(long id)
        {
            var post = GetPost(id);
            if (post == null)
                return false;

            post.IsClosed = true;
            Save();
            return true;
        }

        // Reopen a post by setting IsClosed to false
        public bool ReopenPost(long id)
        {
            var post = GetPost(id);
            if (post == null)
                return false;

            post.IsClosed = false;
            Save();
            return true;
        }

        // Vote on a post (adjust Reputation)
        public bool VoteOnPost(long postId, long userId, int voteValue)
        {
            var post = GetPost(postId);
            if (post == null)
                return false;

            post.Reputation += voteValue;
            Save();
            return true;
        }

        // Delete a post by ID
        public bool DeletePost(long id)
        {
            var post = GetPost(id);
            if (post == null)
                return false;

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
