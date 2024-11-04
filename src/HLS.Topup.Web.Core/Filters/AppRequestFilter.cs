using Abp.AspNetCore.Mvc.Extensions;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Web.Filters
{
    public class AppRequestFilter : ActionFilterAttribute
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AppRequestFilter(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAjax = context.HttpContext.Request.IsAjaxRequest();
            if (!isAjax)
            {
                var appVersion = _appConfiguration["App:AppVersion"];
                var appRequestInfo = context.HttpContext.Request.Headers["app_request_info"];
                if (!string.IsNullOrEmpty(appRequestInfo))
                {
                    var info = CommonHelper.GetAppRequestInfo(appRequestInfo);
                    if (info != null && info.AppVersion != appVersion)
                        throw new UserFriendlyException(100,
                            "Bạn đang dùng phiên bản ứng dụng cũ. Vui lòng cập nhật ứng dụng của bạn để tiếp tục sử dụng");
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
