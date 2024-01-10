using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialWebsite.Api.Services
{
    public class JwtTokenGenerator
    {
        private const string TokenKey = "SabaTokenWhichShouldNotBeSharedWithAnyone";
        private TimeSpan TokenLifeTime = TimeSpan.FromMinutes(2);
        private UserManager<User> userManager;

        public JwtTokenGenerator(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        public async Task<string> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenKey);

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
