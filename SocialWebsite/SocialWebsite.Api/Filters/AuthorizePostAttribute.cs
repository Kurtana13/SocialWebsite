using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialWebsite.Api.Repositories.IRepositories;
using System.Security.Claims;

namespace SocialWebsite.Api.Filters
{
    public class AuthorizePostAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var postRepository = context.HttpContext.RequestServices.GetService<IPostRepository>();
            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var username = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                var postId = Convert.ToInt32(context.HttpContext.Request.RouteValues["postId"].ToString());
                var postResult = await postRepository.GetById(postId);
                var userResult = await userRepository.GetByUsername(username);
                if (postResult != null && userResult != null)
                {
                    if (userResult.Id != postResult.UserId)
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
