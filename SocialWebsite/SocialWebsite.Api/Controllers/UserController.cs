using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Models;
using System.Reflection.Metadata.Ecma335;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UnitOfWork unitOfWork;
        private readonly IUserRepository? _userRepository;

        public UserController(ApplicationDbContext context)
        {
          unitOfWork = new UnitOfWork(context);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await unitOfWork.UserRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var result = await unitOfWork.UserRepository.GetById(id);
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
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest();
                }
                var createdUser = await unitOfWork.UserRepository.Create(user);
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

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}
