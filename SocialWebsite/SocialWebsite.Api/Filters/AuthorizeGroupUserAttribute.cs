using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialWebsite.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeGroupUserAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            
        }
    }
}
