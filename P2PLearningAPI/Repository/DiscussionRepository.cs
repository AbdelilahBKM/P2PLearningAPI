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
            return _context.Discussions
        .Where(d => d.Id == id)
        .Select(d => new DiscussionDTO
        {
            Id = d.Id,
            D_Name = d.D_Name,
            D_Profile = d.D_Profile,
            D_Description = d.D_Description,
            Number_of_members = d.Number_of_members,
            Number_of_active_members = d.Number_of_active_members,
            Number_of_posts = d.Number_of_posts,
            Owner = new UserMiniDTO
            {
                Id = d.Owner.Id,
                UserName = d.Owner.UserName,
                ProfilePicture = d.Owner.ProfilePicture
            },
            Questions = d.Questions.Select(q => new QuestionDTO
            {
                Id = q.Id,
                Title = q.Title,
                Content = q.Content,
                IsAnswered = q.isAnswered,
                PostedAt = q.PostedAt,
                PostedBy = new UserMiniDTO
                {
                    Id = q.PostedBy.Id,
                    UserName = q.PostedBy.UserName,
                    ProfilePicture = q.PostedBy.ProfilePicture
                },
                Answers = q.Answers.Select(a => new AnswerDTO
                {
                    Id = a.Id,
                    Content = a.Content,
                    IsBestAnswer = a.IsBestAnswer,
                    PostedBy = new UserMiniDTO
                    {
                        Id = a.PostedBy.Id,
                        UserName = a.PostedBy.UserName,
                        ProfilePicture = a.PostedBy.ProfilePicture
                    },
                    Replies = a.Replies.Select(r => new AnswerDTO
                    {
                        Id = r.Id,
                        Content = r.Content,
                        IsBestAnswer = r.IsBestAnswer,
                        PostedBy = new UserMiniDTO
                        {
                            Id = r.PostedBy.Id,
                            UserName = r.PostedBy.UserName,
                            ProfilePicture = r.PostedBy.ProfilePicture
                        }
                    }).ToList()
                }).ToList()
            }).ToList()
        })
        .FirstOrDefault();
        }

        public DiscussionDTO? GetDiscussion(long id)
        {
            return _context.Discussions
                          .Include(d => d.Questions)
                          .ThenInclude(q => q.PostedBy)
                          .Include(d => d.Questions)
                          .ThenInclude(q => q.Answers)
                          .FirstOrDefault(d => d.Id == id);
        }
        public DiscussionDTO? GetDiscussion(string name)
        {
            return _context.Discussions
                            .Include (d => d.Owner)
                          .Include(d => d.Questions)
                          .ThenInclude(q => q.Answers)
                          .Include(d => d.Questions)
                            .ThenInclude(q => q.PostedBy)
                          .FirstOrDefault(d => d.D_Name == name);
        }

        public bool CheckDiscussionExist(long id)
        {
            return _context.Discussions.Any(d => d.Id == id);
        }

        public ICollection<Question> GetQuestionsByDiscussion(long discussionId)
        {
            var discussion = _context.Discussions
                                    .Include(d => d.Questions)
                                    .FirstOrDefault(d => d.Id == discussionId);

            return discussion?.Questions.ToList() ?? new List<Question>();
        }

        public ICollection<DiscussionDTO> GetDiscussionsByOwner(string ownerId)
        {
            return _context.Discussions
                          .Where(d => d.OwnerId == ownerId)
                          .ToList();
        }

        public DiscussionDTO CreateDiscussion(Discussion discussion, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            if (UserId != discussion.OwnerId)
                throw new UnauthorizedAccessException("Unauthorized User");
            if(GetDiscussion(discussion.D_Name) != null)
                throw new InvalidOperationException("Discussion already exists.");
            _context.Discussions.Add(discussion);
            if (Save()) return discussion;

            throw new InvalidOperationException("Failed to save the discussion to the database.");
        }

        public DiscussionDTO UpdateDiscussion(Discussion discussion, string token)
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

            _context.Discussions.Update(existingDiscussion);
            if (Save()) return existingDiscussion;

            throw new InvalidOperationException("Failed to update the discussion.");
        }

        public bool DeleteDiscussion(long id, string token)
        {
            var discussion = GetDiscussion(id);
            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            _context.Discussions.Remove(discussion);
            return Save();
        }

        public bool MarkDiscussionAsDeleted(long id)
        {
            var discussion = GetDiscussion(id);
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
    }
}
