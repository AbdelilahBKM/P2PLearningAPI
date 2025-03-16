using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IUserBadgeInterface
    {
        public ICollection<UserBadge> GetUserBadges();
        public ICollection<UserBadge> GetUserBadges(string userId);
        public ICollection<UserBadge> GetUserBadges(int badgeId);
        public UserBadge? GetUserBadge(string userId, int badgeId);
        public UserBadge AddUserBadge(UserBadge userBadge);
        public bool DeleteUserBadge(string userId, int badgeId);
        public bool Save();
    }
}
