using System.Linq;
using System.Threading.Tasks;
using HLS.Topup.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace HLS.Topup.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckUserRouterMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckUserRouterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var username = httpContext.User?.Claims?.First(c => c.Type == "user_name").Value;
                var checkType = httpContext.User?.Claims?.First(c => c.Type == "account_type").Value;
                if (checkType != null)
                {
                    var request = httpContext.Request.Path.ToString().ToLower();
                    if (checkType == CommonConst.SystemAccountType.System.ToString("G") && username?.ToLower()!="admin" &&
                        (request.StartsWith("/topup")
                         || request.StartsWith("/billpayment")
                         || request.StartsWith("/transactions")
                         //|| request.StartsWith("/profile")
                         || request=="/"
                        ))
                    {
                        //httpContext.Response.Redirect("/app");
                        httpContext.Response.Redirect("/Error?statusCode=403");
                    }
                    else if (username?.ToLower()!="admin" && checkType != CommonConst.SystemAccountType.System.ToString("G") && request.StartsWith("/app"))
                    {
                        //httpContext.Response.Redirect("/");
                        httpContext.Response.Redirect("/Error?statusCode=403");
                    }
                }
            }

            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckUserRouterMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckUserRouterMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckUserRouterMiddleware>();
        }
    }
}
