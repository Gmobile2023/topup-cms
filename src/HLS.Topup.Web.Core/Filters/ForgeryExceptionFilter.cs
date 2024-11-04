using Abp.AspNetCore.Mvc.Extensions;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Microsoft.AspNetCore.Antiforgery;

namespace HLS.Topup.Web.Filters
{
    public class ForgeryExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //string actionName = context.RouteData.Values["action"]?.ToString()?.ToLower();
            //string controllerName = context.RouteData.Values["controller"]?.ToString()?.ToLower();

            if (context.Exception is AntiforgeryValidationException &&
                context.HttpContext.User != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.ExceptionHandled = true;
                context.Result = new RedirectResult("/");
            }
        }
    }
}
