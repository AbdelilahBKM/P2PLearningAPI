using NuGet.Common;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IJoiningInterface
    {
        ICollection<Joining> GetJoinings();
        Joining? GetJoining(long id);
        Joining? GetJoining(string userId, long discussionId);
        ICollection<Joining> GetJoiningsByUser(string userId);
        ICollection<Joining> GetJoiningsByDiscussion(long discussionId);
        Joining CreateJoining(Joining joining);
        bool DeleteJoining(long id, String token);
        bool Save();
    }
}
