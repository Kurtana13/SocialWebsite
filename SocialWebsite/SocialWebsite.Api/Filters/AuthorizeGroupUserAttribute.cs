using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialWebsite.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeGroupUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }
    }
}
