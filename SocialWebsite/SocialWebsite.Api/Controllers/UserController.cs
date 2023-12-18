using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;
using System.Reflection.Metadata.Ecma335;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUnitOfWork<ApplicationDbContext> unitOfWork;
        private IUserRepository userRepository;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public UserController(ApplicationDbContext context,UserManager<User> userManager,SignInManager<User> signInManager)
        {
            unitOfWork = new UnitOfWork<ApplicationDbContext>(context);
            userRepository = new UserRepository(unitOfWork);
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Route("[action]")]
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
        [HttpGet]
        public async Task<ActionResult<User>> GetUser([FromRoute]int id)
        {
            try
            {
                var result = await userRepository.GetById(id);
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
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody]User user)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest();
                }
                var createdUser = await userRepository.Create(user);
                if(createdUser == null)
                {
                    return BadRequest("User Already Exists");
                }
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetUser), new { Id = createdUser.Id }, createdUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating user");
            }
        }

        [Route("[action]/{userId}")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser([FromRoute]int userId)
        {
            try
            {
                var result = await userRepository.GetById(userId);
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

        [Route("[action]")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser([FromBody]User user)
        {
            try
            {
                var result = await userRepository.GetById(user.Id);
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

        [Route("[action]/{userId}")]
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser([FromRoute]int userId, [FromBody] User user)
        {
            try
            {
                var result = await userRepository.GetById(userId);
                if(result == null)
                {
                    return BadRequest();
                }
                await userRepository.Put(result,user);
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
