using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IDiscussionInterface
    {
        ICollection<Discussion> GetDiscussions();
        Discussion GetDiscussion(long id);
        bool CheckDiscussionExist(long id);
        ICollection<Discussion> GetDiscussionsByOwner(string ownerId);
        Discussion CreateDiscussion(Discussion discussion);
        Discussion UpdateDiscussion(Discussion discussion);
        bool DeleteDiscussion(long id);
        bool MarkDiscussionAsDeleted(long id);
        ICollection<Question> GetQuestionsByDiscussion(long discussionId);
        bool Save();
    }
}
