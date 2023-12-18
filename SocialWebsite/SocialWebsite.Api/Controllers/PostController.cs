using Microsoft.AspNetCore.Mvc;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

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

        public PostController(ApplicationDbContext context)
        {
            unitOfWork = new UnitOfWork<ApplicationDbContext>(context);
            userRepository = new UserRepository(unitOfWork);
            groupRepository = new GroupRepository(unitOfWork);
            postRepository = new PostRepository(unitOfWork);
            commentRepository = new CommentRepository(unitOfWork);
        }

        [Route("[action]")]
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

        [Route("[action]/{userId}")]
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromRoute] int userId, [FromBody] Post post)
        {
            try
            {
                var userResult = await userRepository.GetById(userId);
                if (userResult == null)
                {
                    return BadRequest();
                }
                var createdPost = await postRepository.Create(userResult.Id, post);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetPost), new { Id = createdPost.Id }, createdPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating post");
            }
        }

        [Route("[action]/{postId}")]
        [HttpDelete]
        public async Task<ActionResult<Post>> DeletePost([FromRoute] int postId)
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

        [Route("[action]/{postId}")]
        [HttpPost]
        public async Task<ActionResult<Comment>> CreatePostComment([FromRoute]int postId, [FromBody]int ownerId, [FromBody]Comment comment)
        {
            try
            {
                var postResult = await postRepository.GetById(postId);
                var ownerResult = await userRepository.GetById(ownerId);
                if (postResult == null || ownerResult == null)
                {
                    return BadRequest();
                }
                await commentRepository.Create(postResult.Id,ownerResult.Id,comment);
                await unitOfWork.Save();
                return CreatedAtAction(nameof(GetPost), new { Id = comment.Id }, comment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting user");
            }
        }

    }
}
