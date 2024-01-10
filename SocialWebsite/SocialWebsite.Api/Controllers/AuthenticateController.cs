using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Api.Services;
using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly JwtTokenGenerator jwtTokenGenerator;
        private IUserRepository userRepository;

        public AuthenticateController(
            UserManager<User> _userManager,
            JwtTokenGenerator _jwtTokenGenerator,
            IUserRepository _userRepository)
        {
            userManager = _userManager;
            jwtTokenGenerator =  _jwtTokenGenerator;
            userRepository = _userRepository;
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user,model.Password))
            {
                //var userRoles = await userManager.GetRolesAsync(user);

                //var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //};

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}
                var token = jwtTokenGenerator.GenerateToken(user);
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
