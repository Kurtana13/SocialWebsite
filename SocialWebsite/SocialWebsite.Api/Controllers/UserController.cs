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
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<User> _genericRepository;
        public UserController(IUserRepository userRepository, IGenericRepository<User> genericRepository)
        {
            _userRepository = userRepository;
            _genericRepository = genericRepository;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await _genericRepository.Get());
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
                var result = await _genericRepository.GetById(id);
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
                var createdUser = await _userRepository.CreateUser(user);
                if(createdUser == null)
                {
                    return BadRequest("User Already Exists");
                }
                return CreatedAtAction("CreateMethod", new { Id = createdUser.Id }, createdUser);
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
