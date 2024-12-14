using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IDiscussionInterface
    {
        ICollection<Discussion> GetDiscussions();
        Discussion? GetDiscussion(long id);
        bool CheckDiscussionExist(long id);
        ICollection<Discussion> GetDiscussionsByOwner(string ownerId);
        Discussion CreateDiscussion(Discussion discussion, string token);
        Discussion UpdateDiscussion(Discussion discussion, string token);
        bool DeleteDiscussion(long id, string token);
        bool MarkDiscussionAsDeleted(long id);
        ICollection<Question> GetQuestionsByDiscussion(long discussionId);
        bool Save();
    }
}
