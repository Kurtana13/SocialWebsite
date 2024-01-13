using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialWebsite.Api.Repositories.IRepositories;
using System.Security.Claims;

namespace SocialWebsite.Api.Filters
{
    public class AuthorizeCommentAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var postRepository = context.HttpContext.RequestServices.GetService<IPostRepository>();
            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var commentRepository = context.HttpContext.RequestServices.GetService<ICommentRepository>();
            var requestOwner = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(requestOwner))
            {
                var postId = Convert.ToInt32(context.HttpContext.Request.RouteValues["postId"]!.ToString());
                var commentId = Convert.ToInt32(context.HttpContext.Request.RouteValues["commentId"]!.ToString());
                var postResult = await postRepository.GetById(postId);
                var userResult = await userRepository.GetByUsername(requestOwner);
                var commentResult = await commentRepository.GetById(commentId);

                if (postResult != null && userResult != null)
                {
                    if (userResult.Id != postResult.UserId && userResult.Id != commentResult.UserId)
                    {
                        context.Result = new ForbidResult();
                    }
                }
                else
                {
                    context.Result = new BadRequestResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
