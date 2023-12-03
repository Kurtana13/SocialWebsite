using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private IUnitOfWork<ApplicationDbContext> unitOfWork;
        private IGroupRepository groupRepository;
        private IPostRepository postRepository;
        private ICommentRepository commentRepository;

        public GroupController(ApplicationDbContext context)
        {
            unitOfWork = new UnitOfWork<ApplicationDbContext>(context);
            groupRepository = new GroupRepository(unitOfWork);
            postRepository = new PostRepository(unitOfWork);
            commentRepository = new CommentRepository(unitOfWork);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await groupRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetGroup([FromRoute]int id)
        {
            try
            {
                var result = await groupRepository.GetById(id);
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group group)
        {
            try
            {
                if (group == null)
                {
                    return BadRequest();
                }
                var createdGroup = await groupRepository.Create(group);
                if (createdGroup == null)
                {
                    return BadRequest("User Already Exists");
                }
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetGroup), new { Id = createdGroup.Id }, createdGroup);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating group");
            }
        }

        [Route("[action]/{id}")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteGroup([FromRoute] int id)
        {
            try
            {
                var result = await groupRepository.GetById(id);
                if (result == null)
                {
                    return BadRequest();
                }
                var postResult = await postRepository.Get(x=>x.GroupId == id);
                foreach(var post in postResult)
                {
                    var commentResult = await commentRepository.Get(x=>x.PostId == post.Id);
                    foreach(var comment in commentResult)
                    {
                        await commentRepository.Delete(comment);
                    }
                }
                await groupRepository.Delete(result);
                await unitOfWork.Save();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting group");
            }
        }

    }
}
