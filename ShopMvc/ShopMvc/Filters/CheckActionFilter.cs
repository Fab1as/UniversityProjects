using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Filters
{
    public class CheckActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Cookies.Append(context.ActionDescriptor.DisplayName, DateTime.Now.ToString("dd/MM/yyyy hh-mm-ss"));

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
