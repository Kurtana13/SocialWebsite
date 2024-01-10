using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;
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

        public GroupController(IUnitOfWork<ApplicationDbContext> unitOfWork,
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IUserGroupRepository userGroupRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.groupRepository = groupRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.userGroupRepository = userGroupRepository;
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetAll()
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
        public async Task<ActionResult<Group>> GetGroup([FromRoute] int id)
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
        public async Task<ActionResult<Group>> CreateGroup([FromRoute] int userId, [FromBody] GroupViewModel groupViewModel)
        {
            try
            {
                var userResult = await userRepository.GetById(userId);
                if (groupViewModel == null || userResult == null)
                {
                    return BadRequest();
                }
                var createdGroup = await groupRepository.Create(groupViewModel);
                if (createdGroup == null)
                {
                    return BadRequest("Group Already Exists");
                }
                await unitOfWork.Save();
                await userGroupRepository.Create(createdGroup.Id, userResult);
                await unitOfWork.Save();
                return Ok(createdGroup);
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
                await groupRepository.Delete(result);
                await unitOfWork.Save();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting group");
            }
        }

        [Route("[action]/{groupId}/{userId}")]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroupPost([FromRoute] int groupId, [FromRoute] int userId, [FromBody]PostViewModel postViewModel)
        {
            try
            {
                var createdPost = await postRepository.Create(userId,postViewModel);
                if (createdPost == null)
                {
                    return BadRequest();
                }
                var group = await groupRepository.GetById(groupId);
                if (group == null)
                {
                    return BadRequest("No such group found");
                }
                await postRepository.CreateGroupPost(groupId,createdPost);
                await unitOfWork.Save();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating group");
            }
        }

        [Route("[action]/{groupId}/{postId}")]
        [HttpDelete]
        public async Task<ActionResult<Post>> DeleteGroupPost([FromRoute] int groupId, [FromRoute] int postId)
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

        [Route("[action]/{groupId}")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateGroupUser([FromRoute] int groupId, [FromBody] int userId)
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

        [Route("[action]/{groupId}")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteGroupUser([FromRoute] int groupId, [FromBody] int userId)
        {
            try
            {
                var userResult = await userRepository.GetById(userId);
                if (userResult == null)
                {
                    return BadRequest();
                }
                await userGroupRepository.Delete(ug=>ug.UserId == userId && ug.GroupId == groupId);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(UserController.GetUser), new { Id = userId }, userResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding user to group");
            }
        }
    }
}
