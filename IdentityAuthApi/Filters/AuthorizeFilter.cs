using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityAuthApi.Filters
{
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "student")
            {
                context.Result = new BadRequestResult();
                return;
            }
        }
    }
}
