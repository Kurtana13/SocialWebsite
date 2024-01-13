using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Filters;
using SocialWebsite.Api.Identity;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private IUnitOfWork<ApplicationDbContext> unitOfWork;
        private IUserRepository userRepository;
        private IGroupRepository groupRepository;
        private IPostRepository postRepository;
        private ICommentRepository commentRepository;

        public PostController(IUnitOfWork<ApplicationDbContext> unitOfWork, 
            IUserRepository userRepository,
            IGroupRepository groupRepository, 
            IPostRepository postRepository, 
            ICommentRepository commentRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.groupRepository = groupRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll()
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
        [Authorize]
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

        [Route("[action]/{username}")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromRoute]string username,[FromBody]PostViewModel postViewModel)
        {
            try
            {
                var userResult = await userRepository.GetByUsername(username);
                if (userResult == null || postViewModel == null)
                {
                    return BadRequest();
                }
                var createdPost = await postRepository.Create(userResult.Id, postViewModel);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetPost), new { Id = createdPost.Id }, createdPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating post");
            }
        }

        [Route("[action]/{postId}")]
        [Authorize]
        [AuthorizePost]
        [HttpDelete]
        public async Task<ActionResult<Post>> DeletePost([FromRoute]int postId)
        {
            try
            {
                var postResult = await postRepository.GetById(postId);
                if (postResult == null)
                {
                    return BadRequest();
                }
                await userRepository.DeleteById(postResult.Id);
                await unitOfWork.Save();
                return Ok(postResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting post");
            }
        }

        [Route("[action]/{postId}/{ownerUsername}")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Comment>> CreatePostComment([FromRoute]int postId, [FromRoute]string ownerUsername, [FromBody]CommentViewModel commentViewModel)
        {
            try
            {
                var postResult = await postRepository.GetById(postId);
                var ownerResult = await userRepository.GetByUsername(ownerUsername);
                if (postResult == null || ownerResult == null || commentViewModel == null)
                {
                    return BadRequest();
                }
                var commentResult = await commentRepository.Create(postResult.Id,ownerResult.Id,commentViewModel);
                await unitOfWork.Save();
                return Ok(commentResult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting user");
            }
        }

        [Route("[action]/{postId}/{commentId}")]
        [Authorize]
        [AuthorizeUser]
        [HttpDelete]
        public async Task<ActionResult<Comment>> DeletePostComment([FromRoute]int postId, [FromRoute]int commentId)
        {
            try
            {
                var postResult = await postRepository.GetById(postId);
                if (postResult == null)
                {
                    return BadRequest("Couldn't find the post");
                }
                var commentResult = postResult.Comments.FirstOrDefault(x => x.Id == commentId);
                if(commentResult == null)
                {
                    return BadRequest("Couldn't find the comment of the post");
                }
                await commentRepository.Delete(commentResult);
                await unitOfWork.Save();
                return Ok(commentResult);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting user");
            }
        }

    }
}
