using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private IUnitOfWork<ApplicationDbContext> unitOfWork;
        private IPostRepository postRepository;

        public PostController(ApplicationDbContext context)
        {
            unitOfWork = new UnitOfWork<ApplicationDbContext>(context);
            postRepository = new PostRepository(unitOfWork);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await postRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetPost([FromRoute] int id)
        {
            try
            {
                var result = await postRepository.GetById(id);
                if (result == null)
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


    }
}
