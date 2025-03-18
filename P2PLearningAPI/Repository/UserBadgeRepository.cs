using P2PLearningAPI.Data;
using P2PLearningAPI.Models;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Repository
{
    public class UserBadgeRepository: IUserBadgeInterface
    {
        private readonly P2PLearningDbContext _context;

        public UserBadgeRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        public ICollection<UserBadge> GetUserBadges()
        {
            return _context.UserBadges
                          .OrderBy(d => d.UserId)
                          .ToList();
        }

        public ICollection<UserBadge> GetUserBadges(string userId)
        {
            return _context.UserBadges
                          .Where(d => d.UserId == userId)
                          .OrderBy(d => d.BadgeId)
                          .ToList();
        }

        public ICollection<UserBadge> GetUserBadges(int badgeId)
        {
            return _context.UserBadges
                          .Where(d => d.BadgeId == badgeId)
                          .OrderBy(d => d.UserId)
                          .ToList();
        }

        public UserBadge? GetUserBadge(string userId, int badgeId)
        {
            return _context.UserBadges.Find(userId, badgeId);
        }

        public UserBadge AddUserBadge(UserBadge userBadge)
        {
            _context.UserBadges.Add(userBadge);
            if (Save()) return userBadge;
            return null;
        }

        public bool DeleteUserBadge(string userId, int badgeId)
        {
            var userBadge = _context.UserBadges.Find(userId, badgeId);
            if (userBadge == null) return false;
            _context.UserBadges.Remove(userBadge);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

    }
}
