using Microsoft.IdentityModel.Tokens;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P2PLearningAPI.Repository
{
    public class TokenServices : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public (string UserId, string Email) DecodeToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:SecretKey"]
            ));
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                foreach (var claim in principal.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
                var userId = principal.Claims.FirstOrDefault(
                    c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                var email = principal.Claims.FirstOrDefault(
                    c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                Console.WriteLine($"User ID: {userId}, Email: {email}");
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
                {
                    throw new SecurityTokenException("Invalid token claims.");
                }

                return (userId, email);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException($"Invalid token: {ex.Message}", ex);
            }
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration["Jwt:SecretKey"]!
                ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal ValidateRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
