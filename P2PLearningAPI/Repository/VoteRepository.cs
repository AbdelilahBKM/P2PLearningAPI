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

        public Vote? GetVote(long id)
        {
            return _context.Votes.Include(v => v.User).FirstOrDefault(v => v.Id == id)!;
        }

        public Vote? GetVote(long postId, string userId)
        {
            return _context.Votes.Include(v => v.User).FirstOrDefault(v => v.PostId == postId && v.UserId == userId);
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
            if (GetVote(vote.PostId, vote.UserId) != null)
                throw new InvalidOperationException("vote already exists");

            _context.Votes.Add(vote);
            Post post = _context.Posts.Find(vote.PostId);
            if (post != null)
            {
                if (vote.VoteType == VoteType.Positive)
                    post.Reputation++;
                else if (vote.VoteType == VoteType.Negative)
                    post.Reputation--;
                else
                    throw new InvalidOperationException("Invalid vote type");
            }
            if (Save())
                return vote;
            throw new InvalidOperationException("unable to create vote");
        }

        public bool DeleteVote(long id)
        {
            var vote = GetVote(id);
            if (vote == null)
                return false;
            long PostId = vote.PostId;
            Post post = _context.Posts.Find(PostId);
            if (post != null)
            {
                if (vote.VoteType == VoteType.Positive)
                    post.Reputation--;
                else if (vote.VoteType == VoteType.Negative)
                    post.Reputation++;
                else
                    throw new InvalidOperationException("Invalid vote type");
            }
            _context.Votes.Remove(vote);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
