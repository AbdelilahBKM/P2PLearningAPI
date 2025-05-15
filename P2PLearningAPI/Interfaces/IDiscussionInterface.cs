using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IDiscussionInterface
    {
        ICollection<DiscussionDTO> GetDiscussions();
        DiscussionDTO? GetDiscussion(long id);
        DiscussionDTO? GetDiscussion(string name);
        bool CheckDiscussionExist(long id);
        ICollection<DiscussionDTO> GetDiscussionsByOwner(string ownerId);
        DiscussionDTO CreateDiscussion(Discussion discussion, string token);
        DiscussionDTO UpdateDiscussion(Discussion discussion, string token);
        bool DeleteDiscussion(long id, string token);
        bool MarkDiscussionAsDeleted(long id);
        ICollection<Question> GetQuestionsByDiscussion(long discussionId);
        bool Save();
    }
}
