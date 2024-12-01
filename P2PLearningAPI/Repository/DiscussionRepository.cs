using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class DiscussionRepository : IDiscussionInterface
    {
        private readonly P2PLearningDbContext context;

        public DiscussionRepository(P2PLearningDbContext context)
        {
            this.context = context;
        }

        public ICollection<Discussion> GetDiscussions()
        {
            return context.Discussions
                          .Include(d => d.Questions)
                          .Include(d => d.Joinings)
                          .OrderBy(d => d.Id)
                          .ToList();
        }

        public Discussion GetDiscussion(long id)
        {
            return context.Discussions
                          .Include(d => d.Questions)
                          .Include(d => d.Joinings)
                          .FirstOrDefault(d => d.Id == id);
        }

        public bool CheckDiscussionExist(long id)
        {
            return context.Discussions.Any(d => d.Id == id);
        }

        public ICollection<Question> GetQuestionsByDiscussion(long discussionId)
        {
            var discussion = context.Discussions
                                    .Include(d => d.Questions)
                                    .FirstOrDefault(d => d.Id == discussionId);

            return discussion?.Questions.ToList() ?? new List<Question>();
        }

        public ICollection<Discussion> GetDiscussionsByOwner(long ownerId)
        {
            return context.Discussions
                          .Where(d => d.OwnerId == ownerId)
                          .ToList();
        }

        public Discussion CreateDiscussion(Discussion discussion)
        {
            context.Discussions.Add(discussion);
            if (Save()) return discussion;

            throw new InvalidOperationException("Failed to save the discussion to the database.");
        }

        public Discussion UpdateDiscussion(Discussion discussion)
        {
            var existingDiscussion = GetDiscussion(discussion.Id);
            if (existingDiscussion == null)
                throw new InvalidOperationException("Discussion not found.");

            existingDiscussion.D_Name = discussion.D_Name;
            existingDiscussion.D_Profile = discussion.D_Profile;
            existingDiscussion.Number_of_members = discussion.Number_of_members;
            existingDiscussion.Number_of_active_members = discussion.Number_of_active_members;
            existingDiscussion.Number_of_posts = discussion.Number_of_posts;
            existingDiscussion.IsDeleted = discussion.IsDeleted;

            context.Discussions.Update(existingDiscussion);
            if (Save()) return existingDiscussion;

            throw new InvalidOperationException("Failed to update the discussion.");
        }

        public bool DeleteDiscussion(long id)
        {
            var discussion = GetDiscussion(id);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            context.Discussions.Remove(discussion);
            return Save();
        }

        public bool MarkDiscussionAsDeleted(long id)
        {
            var discussion = GetDiscussion(id);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            discussion.IsDeleted = true;
            context.Discussions.Update(discussion);
            return Save();
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }
    }
}
