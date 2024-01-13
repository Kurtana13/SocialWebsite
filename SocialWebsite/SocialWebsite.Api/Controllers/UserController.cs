using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Filters;
using SocialWebsite.Api.Identity;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Api.Services;
using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUnitOfWork<ApplicationDbContext> unitOfWork;
        private IUserRepository userRepository;
        private JwtTokenGenerator jwtTokenGenerator;

        public UserController(IUnitOfWork<ApplicationDbContext> _unitOfWork,
            IUserRepository _userRepository,
            JwtTokenGenerator _jwtTokenGenerator)
        {
            unitOfWork = _unitOfWork;
            jwtTokenGenerator = _jwtTokenGenerator;
            userRepository = _userRepository;
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                return Ok(await userRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        [Route("[action]/{userId}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser([FromRoute]int userId)
        {
            try
            {
                var result = await userRepository.GetById(userId);
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody]UserViewModel userViewModel)
        {
            try
            {
                if(userViewModel == null)
                {
                    return BadRequest();
                }
                
                var createdUser = await userRepository.Create(userViewModel);
                if(createdUser == null)
                {
                    return BadRequest("User Already Exists");
                }

                await unitOfWork.Save();
                return Ok(await jwtTokenGenerator.GenerateToken(createdUser));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating user");
            }
        }

        [Route("[action]/{username}")]
        [Authorize]
        [AuthorizeUser]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser([FromRoute]string username)
        {
            try
            {
                var result = await userRepository.GetByUsername(username);
                if (result == null)
                {
                    return BadRequest();
                }
                await userRepository.Delete(result);
                await unitOfWork.Save();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting user");
            }
        }

        //[Route("[action]")]
        //[HttpDelete]
        //public async Task<ActionResult<User>> DeleteUser([FromBody] UserViewModel userViewModel)
        //{
        //    try
        //    {
        //        var result = await userRepository.GetByUsername(userViewModel.Username);
        //        if (result == null)
        //        {
        //            return BadRequest();
        //        }
        //        await userRepository.Delete(result);
        //        await unitOfWork.Save();
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting user");
        //    }
        //}

        [Route("[action]/{username}")]
        [Authorize]
        [AuthorizeUser]
        [HttpPatch]
        public async Task<ActionResult<User>> UpdateUser([FromRoute]string username, [FromBody] UserViewModel userViewModel)
        {
            try
            {
                var result = await userRepository.GetByUsername(username);
                if(result == null)
                {
                    return BadRequest();
                }
                await userRepository.Put(result,new User(result.Id,userViewModel));
                await unitOfWork.Save();
                return Ok(result);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating user");
            }
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
