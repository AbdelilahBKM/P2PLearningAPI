using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class DiscussionRepository : IDiscussionInterface
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        public DiscussionRepository(P2PLearningDbContext context, ITokenService tokenServices)
        {
            _context = context;
            _tokenService = tokenServices;
        }

        public ICollection<DiscussionDTO> GetDiscussions()
        {
            var discussions = _context.Discussions
                .Include(d => d.Owner)
                .Include(d => d.Joinings)
                    .ThenInclude(j => j.User)
                .Include(d => d.Questions)
                    .ThenInclude(q => q.PostedBy)
                .Include(d => d.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(a => a.PostedBy)
                .Include(d => d.Questions)
                    .ThenInclude(q => q.Votes)
                .AsSplitQuery()
                .ToList();

            // Filter out discussions without questions
            var filteredDiscussions = discussions
                .Where(d => d.Questions != null && d.Questions.Any())
                .OrderByDescending(d => d.Questions.Max(q => q.Created_at))
                .Select(d => DiscussionDTO.FromDiscussion(d))
                .ToList();

            return filteredDiscussions;
        }

        public DiscussionDTO? GetDiscussion(long id)
        {
            var discussion = GetFullDiscussionQuery()
                .FirstOrDefault(d => d.Id == id);
            return discussion != null ? DiscussionDTO.FromDiscussion(discussion) : null;
        }

        public DiscussionDTO? GetDiscussion(string name)
        {
            var discussion = GetFullDiscussionQuery()
                .FirstOrDefault(d => d.D_Name == name);
            return discussion != null ? DiscussionDTO.FromDiscussion(discussion) : null;
        }

        public bool CheckDiscussionExist(long id)
        {
            return _context.Discussions.Any(d => d.Id == id);
        }

        public ICollection<QuestionDTO> GetQuestionsByDiscussion(long discussionId)
        {
            var discussion = _context.Discussions
                                  .Include(d => d.Questions)
                                  .ThenInclude(q => q.PostedBy)
                                  .Include(d => d.Questions)
                                  .ThenInclude(q => q.Answers)
                                  .FirstOrDefault(d => d.Id == discussionId);
            if (discussion == null)
                return new List<QuestionDTO>();

            return discussion.Questions
                            .Select(q => QuestionDTO.FromQuestion(q))
                            .ToList();
        }

        public ICollection<DiscussionDTO> GetDiscussionsByOwner(string ownerId)
        {
            ICollection<Discussion> discussions = _context.Discussions
                          .Where(d => d.OwnerId == ownerId)
                          .ToList();
            if (discussions == null || discussions.Count == 0)
                return new List<DiscussionDTO>();
            return discussions.Select(d => DiscussionDTO.FromDiscussion(d)).ToList();
        }

        public DiscussionDTO CreateDiscussion(Discussion discussion, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            if (UserId != discussion.OwnerId)
                throw new UnauthorizedAccessException("Unauthorized User");
            if(GetDiscussion(discussion.D_Name) != null)
                throw new InvalidOperationException("Discussion already exists.");
            _context.Discussions.Add(discussion);
            if (Save()) return DiscussionDTO.FromDiscussion(discussion);

            throw new InvalidOperationException("Failed to save the discussion to the database.");
        }

        public DiscussionDTO UpdateDiscussion(Discussion discussion, string token)
        {
            var existingDiscussion = _context.Discussions.FirstOrDefault(d => d.Id == discussion.Id);
            if (existingDiscussion == null)
                throw new InvalidOperationException("Discussion not found.");

            existingDiscussion.D_Name = discussion.D_Name;
            existingDiscussion.D_Profile = discussion.D_Profile;
            existingDiscussion.Number_of_members = discussion.Number_of_members;
            existingDiscussion.Number_of_active_members = discussion.Number_of_active_members;
            existingDiscussion.Number_of_posts = discussion.Number_of_posts;
            existingDiscussion.IsDeleted = discussion.IsDeleted;

            _context.Discussions.Update(existingDiscussion);
            if (Save()) return DiscussionDTO.FromDiscussion(existingDiscussion);

            throw new InvalidOperationException("Failed to update the discussion.");
        }

        public bool DeleteDiscussion(long id, string token)
        {
            var discussion = _context.Discussions.FirstOrDefault(d => d.Id == id);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            _context.Discussions.Remove(discussion);
            return Save();
        }

        public bool MarkDiscussionAsDeleted(long id)
        {
            var discussion = _context.Discussions.FirstOrDefault(d => d.Id == id);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            discussion.IsDeleted = true;
            _context.Discussions.Update(discussion);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public Discussion? getFullDiscussionById(long id)
        {
            return _context.Discussions
                .Include(d => d.Joinings)
                .ThenInclude(j => j.User)
                .Include(d => d.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(d => d.Id == id);
        }

        private IQueryable<Discussion> GetFullDiscussionQuery()
        {
            return _context.Discussions
                .Include(d => d.Owner)
                .Include(d => d.Joinings)
                    .ThenInclude(j => j.User)
                .Include(d => d.Questions)
                    .ThenInclude(q => q.PostedBy)
                .Include(d => d.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(a => a.PostedBy)
                .Include(d => d.Questions)
                    .ThenInclude(q => q.Votes)
                .AsSplitQuery();
        }
    }
}
