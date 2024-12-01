using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IPostInterface
    {
        ICollection<Post> GetPosts();
        Post GetPost(long id);
        bool CheckPostExist(long id);
        ICollection<Post> GetPostsByUser(long userId);
        Post CreatePost(Post post);
        Post UpdatePost(Post post);
        bool DeletePost(long id);
        bool ClosePost(long id);
        bool ReopenPost(long id);
        bool VoteOnPost(long postId, long userId, int voteValue); // Assuming you are handling votes.
        bool Save();
    }
}
