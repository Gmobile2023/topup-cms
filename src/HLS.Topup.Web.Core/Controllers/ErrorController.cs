using System;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Web.Models;
using Abp.Web.Mvc.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Controllers
{
    [DisableAuditing]
    public class ErrorController : AbpController
    {
        private readonly IErrorInfoBuilder _errorInfoBuilder;

        public ErrorController(IErrorInfoBuilder errorInfoBuilder)
        {
            _errorInfoBuilder = errorInfoBuilder;
        }

        public ActionResult Index(int statusCode = 0)
        {
            if (statusCode == 404)
            {
                return E404();
            }

            if (statusCode == 403)
            {
                return E403();
            }
            if (statusCode == 1001)
            {
                return RedirectToAction("Logout", "Account");
            }

            var exHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            // if (exHandlerFeature == null)
            // {
            //     return HttpContext.User.Identity.IsAuthenticated
            //         ? RedirectToAction("Index", "Home")
            //         : RedirectToAction("Login", "Account");
            // }


            var exception = exHandlerFeature.Error;


            return View(
                "Error",
                new ErrorViewModel(
                    _errorInfoBuilder.BuildForException(exception),
                    exception
                )
            );
        }

        public ActionResult E403()
        {
            return View("Error403");
        }

        public ActionResult E404()
        {
            return View("Error404");
        }
    }
}
