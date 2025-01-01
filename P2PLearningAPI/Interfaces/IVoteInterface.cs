using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IVoteInterface
    {
        ICollection<Vote> GetVotes();
        Vote? GetVote(long id);
        Vote? GetVote(long postId, string userId);
        bool CheckVoteExist(long postId, string userId);
        ICollection<Vote> GetVotesByPost(long postId);
        ICollection<Vote> GetVotesByUser(string userId);
        Vote CreateVote(Vote vote);
        bool DeleteVote(long id);
        bool Save();
    }
}
