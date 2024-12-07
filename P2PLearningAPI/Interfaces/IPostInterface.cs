using P2PLearningAPI.DTOs;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public enum PostType
    {
        Question,
        Answer
    }
    public interface IPostInterface
    {
        ICollection<Post> GetPosts();
        Post GetPost(long id);
        bool CheckPostExist(long id);
        ICollection<Post> GetPostsByUser(long userId);
        Post CreatePost(PostDTO post, PostType postType);
        Post UpdatePost(Post post);
        bool DeletePost(long id);
        bool ClosePost(long id);
        bool ReopenPost(long id);
        bool VoteOnPost(long id, Vote vote);
        bool DeleteVote(long id, Vote vote);
        bool Save();
    }
}
