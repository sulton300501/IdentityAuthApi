using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthApi.Filters
{
    public class UpdateResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Request.RouteValues.TryGetValue("id", out object aydi))
            {
                context.Result = new BadRequestResult();
                return;
            }
            if (aydi == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            if (Convert.ToString(aydi).Length == 0)
            {
                context.Result = new NotFoundResult();
                return;
            }
        }
    }
}
