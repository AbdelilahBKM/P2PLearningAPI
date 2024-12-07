using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P2PLearningAPI.Repository
{
    public class JoiningRepository : IJoiningInterface
    {
        private readonly P2PLearningDbContext _context;

        // Constructor to inject the DbContext
        public JoiningRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        // Get all joinings
        public ICollection<Joining> GetJoinings()
        {
            return _context.Joinings.OrderBy(j => j.Id).ToList();
        }

        // Get a single joining by id
        public Joining GetJoining(long id)
        {
            return _context.Joinings.FirstOrDefault(j => j.Id == id);
        }

        // Get joinings by userId
        public ICollection<Joining> GetJoiningsByUser(long userId)
        {
            return _context.Joinings.Where(j => j.UserId == userId).ToList();
        }

        // Get joinings by discussionId
        public ICollection<Joining> GetJoiningsByDiscussion(long discussionId)
        {
            return _context.Joinings.Where(j => j.DiscussionId == discussionId).ToList();
        }

        // Create a new joining
        public Joining CreateJoining(Joining joining)
        {
            if (joining == null)
                throw new ArgumentNullException(nameof(joining));
            Joining test = _context.Joinings.Where(j => 
            (j.UserId == joining.UserId && j.DiscussionId == joining.DiscussionId)
                ).First();
            if (test != null)
                throw new InvalidOperationException("Joining already exist");
            _context.Joinings.Add(joining);
            if (Save())
                return joining;

            throw new InvalidOperationException("Failed to save the joining to the database.");
        }

        // Delete a joining by id
        public bool DeleteJoining(long id)
        {
            var joining = GetJoining(id);
            if (joining == null)
                throw new InvalidOperationException("Joining not found.");

            _context.Joinings.Remove(joining);
            return Save();
        }

        // Save changes to the database
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
