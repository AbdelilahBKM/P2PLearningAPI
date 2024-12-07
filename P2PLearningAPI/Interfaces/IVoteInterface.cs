using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IVoteInterface
    {
        ICollection<Vote> GetVotes();
        Vote GetVote(long id);
        bool CheckVoteExist(long postId, long userId);
        ICollection<Vote> GetVotesByPost(long postId);
        ICollection<Vote> GetVotesByUser(long userId);
        Vote CreateVote(Vote vote);
        bool DeleteVote(long id);
        bool Save();
    }
}
