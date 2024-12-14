using P2PLearningAPI.DTOs;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public enum PostType
    {
        Question,
        Answer,
        Reply
    }
    public interface IPostInterface
    {
        ICollection<Post> GetPosts();
        Post? GetPost(long id);
        bool CheckPostExist(long id);
        ICollection<Post> GetPostsByUser(string userId);
        Post CreatePost(PostDTO post, PostType postType, string token);
        Post UpdatePost(Post post, string token);
        bool DeletePost(long id, string token);
        bool ClosePost(long id, string token);
        bool ReopenPost(long id, string token);
        bool VoteOnPost(long id, Vote vote);
        bool DeleteVote(long id, Vote vote);
        bool Save();
    }
}
