using Abp.AspNetCore.Mvc.Extensions;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace HLS.Topup.Web.Filters
{
    public class CheckUserAccessDomainFilter : ActionFilterAttribute
    {
        private readonly TopupAppSession _topupAppSession;

        public CheckUserAccessDomainFilter(TopupAppSession topupAppSession)
        {
            _topupAppSession = topupAppSession;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (_topupAppSession.UserId != null)
            {
                const string url = "/Error?statusCode={0}";
                var request = context.HttpContext.Request.Path.ToString().ToLower();
                if (AccountTypeHepper.IsAccountBackend(_topupAppSession.AccountType) &&
                    (request == "/" || request.StartsWith("/topup") || request.StartsWith("/transactions") ||
                     request.StartsWith("/billpayment")))
                {
                    //context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    if (request == "/")
                    {
                        context.HttpContext.Response.Redirect("/app");
                        return;
                    }

                    context.HttpContext.Response.Redirect(string.Format(url, StatusCodes.Status403Forbidden));
                    return;
                }

                if (!AccountTypeHepper.IsAccountBackend(_topupAppSession.AccountType) &&
                    request.StartsWith("/app"))
                {
                    context.HttpContext.Response.Redirect(string.Format(url, StatusCodes.Status403Forbidden));
                    //context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
