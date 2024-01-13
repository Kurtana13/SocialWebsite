using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SocialWebsite.Api.Filters
{
    //Detects if the user is the one who is making changes on its own account
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeUserAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var username = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                var usernameToConfirm = context.HttpContext.Request.RouteValues["username"]?.ToString();

                if (!string.IsNullOrEmpty(usernameToConfirm))
                {
                    if (usernameToConfirm != username)
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
