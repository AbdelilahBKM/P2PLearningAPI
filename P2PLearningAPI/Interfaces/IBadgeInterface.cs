using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IBadgeInterface
    {
        ICollection<Badge> GetBadges();
        Badge? GetBadge(long id);
        Badge? CreateBadge(Badge badge, string token);
        bool UpdateBadge(Badge badge, string token);
        bool DeleteBadge(long id, string token);
        bool Save();

    }
}
