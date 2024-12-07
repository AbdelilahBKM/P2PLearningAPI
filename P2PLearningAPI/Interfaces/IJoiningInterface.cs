using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IJoiningInterface
    {
        ICollection<Joining> GetJoinings();
        Joining GetJoining(long id);
        ICollection<Joining> GetJoiningsByUser(long userId);
        ICollection<Joining> GetJoiningsByDiscussion(long discussionId);
        Joining CreateJoining(Joining joining);
        bool DeleteJoining(long id);
        bool Save();
    }
}
