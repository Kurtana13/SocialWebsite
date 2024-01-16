using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        public async Task<ActionResult<Group>> GetGroup([FromRoute]int id)
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

        [Route("[action]/{username}")]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromRoute]string username, [FromBody] GroupViewModel groupViewModel)
        {
            try
            {
                var userResult = await userRepository.GetByUsername(username);
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

        [Route("[action]/{groupId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllGroupPost([FromRoute]int groupId)
        {
            try
            {
                var result = await groupRepository.GetById(groupId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(await postRepository.Get(x=>x.GroupId == groupId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting group");
            }
        }

        [Route("[action]/{groupId}/{username}")]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroupPost([FromRoute] int groupId, [FromRoute]string username, [FromBody]PostViewModel postViewModel)
        {
            try
            {
                var userResult = await userRepository.GetByUsername(username);
                var createdPost = await postRepository.Create(userResult.Id,postViewModel);
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
        public async Task<ActionResult<User>> CreateGroupUser([FromRoute]int groupId, [FromBody]string username)
        {
            try
            {
                var userResult = await userRepository.GetById(username);
                if (userResult == null)
                {
                    return BadRequest();
                }
                await userGroupRepository.Create(groupId, userResult);
                await unitOfWork.Save();
                return Ok(userResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding user to group");
            }
        }

        [Route("[action]/{groupId}")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteGroupUser([FromRoute] int groupId, [FromBody]string username)
        {
            try
            {
                var userResult = await userRepository.GetByUsername(username);
                if (userResult == null)
                {
                    return BadRequest();
                }
                await userGroupRepository.Delete(ug=>ug.UserId == userResult.Id && ug.GroupId == groupId);
                await unitOfWork.Save();
                return Ok(userResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding user to group");
            }
        }
    }
}
