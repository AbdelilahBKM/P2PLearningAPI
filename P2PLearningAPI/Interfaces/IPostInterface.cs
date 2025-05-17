using P2PLearningAPI.DTOs;
using P2PLearningAPI.DTOsInput;
using P2PLearningAPI.DTOsOutput;
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
        ICollection<PostDTO> GetPosts();
        PostDTO? GetPost(long id);
        bool CheckPostExist(long id);
        ICollection<PostDTO> GetPostsByUser(string userId);
        PostDTO CreatePost(PostCreateDTO post, string token);
        bool UpdatePost(PostUpdateDTO post, string token);
        bool DeletePost(long id, string token);
        bool ClosePost(long id, string token);
        bool ReopenPost(long id, string token);
        bool VoteOnPost(long id, Vote vote);
        bool DeleteVote(long id, Vote vote);
        Post CreatePostFromDTO(PostCreateDTO postDTO, PostType postType);
        bool MarkAsBestAnswer(long id, string token);
        bool Save();
    }
}
