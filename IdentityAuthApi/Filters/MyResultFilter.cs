﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityAuthApi.Filters
{
    public class MyResultFilter : Attribute, IResultFilter
    {

        public void OnResultExecuted(ResultExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.Controller as ControllerBase;
            var actionName = context.ActionDescriptor.DisplayName;
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"User {userId} executed action: {actionName} with result: {context.Result}");
        }
    }
}
