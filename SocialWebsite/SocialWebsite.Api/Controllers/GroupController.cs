using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;
using System.ComponentModel;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private IUnitOfWork<ApplicationDbContext> unitOfWork;
        private IUserRepository userRepository;
        private IGroupRepository groupRepository;
        private IPostRepository postRepository;
        private ICommentRepository commentRepository;
        private IUserGroupRepository userGroupRepository;

        public GroupController(ApplicationDbContext context)
        {
            unitOfWork = new UnitOfWork<ApplicationDbContext>(context);
            userRepository = new UserRepository(unitOfWork);
            groupRepository = new GroupRepository(unitOfWork);
            postRepository = new PostRepository(unitOfWork);
            commentRepository = new CommentRepository(unitOfWork);
            userGroupRepository = new UserGroupRepository(unitOfWork);
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
        public async Task<ActionResult> GetGroup([FromRoute] int id)
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

        [Route("[action]/{userId}")]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromRoute] int userId, [FromBody] Group group)
        {
            try
            {
                var userResult = await userRepository.GetById(userId);
                if (group == null || userResult == null)
                {
                    return BadRequest();
                }
                var createdGroup = await groupRepository.Create(group);
                if (createdGroup == null)
                {
                    return BadRequest("Group Already Exists");
                }
                await unitOfWork.Save();
                await userGroupRepository.Create(createdGroup.Id, userResult);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetGroup), new { Id = createdGroup.Id }, createdGroup);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating group");
            }
        }

        [Route("[action]/{groupId}")]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroupPost([FromRoute] int groupId, [FromBody] Post post)
        {
            try
            {
                if (post == null)
                {
                    return BadRequest();
                }
                var group = await groupRepository.GetById(groupId);
                if (group == null)
                {
                    return BadRequest("No such group found");
                }
                await postRepository.CreateGroupPost(groupId, post);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetGroup), new { Id = post.Id }, post);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating group");
            }
        }

        [Route("[action]/{groupId}")]
        [HttpPost]
        public async Task<ActionResult<User>> AddUserToGroup([FromRoute] int groupId, [FromBody]int userId)
        {
            try
            {
                var userResult = await userRepository.GetById(userId);
                if (userResult == null)
                {
                    return BadRequest();
                }
                await userGroupRepository.Create(groupId, userResult);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(UserController.GetUser), new { Id = userId }, userResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding user to group");
            }
        }

        [Route("[action]/{id}")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteGroup([FromRoute]int id)
        {
            try
            {
                var result = await groupRepository.GetById(id);
                if (result == null)
                {
                    return BadRequest();
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

        [Route("[action]/{groupId}/{postId}")]
        [HttpDelete]
        public async Task<ActionResult<Post>> DeleteGroupPost([FromRoute]int groupId, [FromRoute]int postId)
        {
            try
            {
                var resultGroup = await groupRepository.GetById(groupId);
                var resultPost = await postRepository.GetById(postId);
                if (resultGroup == null || resultPost == null)
                {
                    return BadRequest();
                }
                await postRepository.Delete(resultPost);
                await unitOfWork.Save();
                return Ok(resultPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting group post");
            }
        }
    }
}
