using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class BadgeRepository: IBadgeInterface
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        public BadgeRepository(P2PLearningDbContext context, ITokenService tokenServices)
        {
            _context = context;
            _tokenService = tokenServices;
        }

        public ICollection<Badge> GetBadges()
        {
            return _context.Badges
                          .OrderBy(d => d.Id)
                          .ToList();
        }
        public Badge? GetBadge(long id) { 
            return _context.Badges.Find(id); 
        }
        public Badge? CreateBadge(Badge badge, string token)
        {
            (var userId, var _)= _tokenService.DecodeToken(token);
            User user = _context.Users.Find(userId);
            if (user == null) return null;
            if (user.UserType != UserType.Administrator) return null;
            _context.Badges.Add(badge);
            if(Save()) return badge;
            return null;
        }

        public bool UpdateBadge(Badge badge, string token)
        {
            (var userId, var _) = _tokenService.DecodeToken(token);
            User user = _context.Users.Find(userId);
            if (user == null) return false;
            if (user.UserType != UserType.Administrator) return false;
            _context.Badges.Update(badge);
            return Save();
        }

        public bool DeleteBadge(long id, string token)
        {
            (var userId, var _) = _tokenService.DecodeToken(token);
            User user = _context.Users.Find(userId);
            if (user == null) return false;
            if (user.UserType != UserType.Administrator) return false;
            var badge = _context.Badges.Find(id);
            if (badge == null) return false;
            _context.Badges.Remove(badge);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
