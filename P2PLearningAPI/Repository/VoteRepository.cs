using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace P2PLearningAPI.Repository
{
    public class VoteRepository : IVoteInterface
    {
        private readonly P2PLearningDbContext _context;

        public VoteRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        public ICollection<Vote> GetVotes()
        {
            return _context.Votes.Include(v => v.User).OrderBy(v => v.Id).ToList();
        }

        public Vote GetVote(long id)
        {
            return _context.Votes.Include(v => v.User).FirstOrDefault(v => v.Id == id)!;
        }

        public bool CheckVoteExist(long postId, string userId)
        {
            return _context.Votes.Any(v => v.PostId == postId && v.UserId == userId);
        }

        public ICollection<Vote> GetVotesByPost(long postId)
        {
            return _context.Votes.Include(v => v.User).Where(v => v.PostId == postId).ToList();
        }

        public ICollection<Vote> GetVotesByUser(string userId)
        {
            return _context.Votes.Include(v => v.Post).Where(v => v.UserId == userId).ToList();
        }

        public Vote CreateVote(Vote vote)
        {
            if (vote == null)
                throw new ArgumentNullException(nameof(vote));

            _context.Votes.Add(vote);
            if(Save())
                return vote;
            throw new InvalidOperationException("unable to create vote");
        }

        public bool DeleteVote(long id)
        {
            var vote = GetVote(id);
            if (vote == null)
                return false;

            _context.Votes.Remove(vote);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
