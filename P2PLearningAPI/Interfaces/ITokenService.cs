using P2PLearningAPI.Models;
using System.Security.Claims;

namespace P2PLearningAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
        public (string UserId, string Email) DecodeToken(string token);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateRefreshToken(string refreshToken);
    }
}
