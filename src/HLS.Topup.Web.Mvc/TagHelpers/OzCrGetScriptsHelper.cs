using Microsoft.AspNetCore.Http;
using NUglify;

namespace HLS.Topup.Web.TagHelpers
{
    public static class OzCrGetScriptsHelper
    {
        /// <summary>
        ///     Gets the content of ABP\GetScripts and optionally minifies it.
        /// </summary>
        /// <param name="aHttpContext">Context the request is running in.</param>
        /// <param name="aBundlingAndMinificationEnabled">Based on whether we are optomising or not.</param>
        /// <returns>Returns the javascript optionally minified.</returns>
        public static string GetScripts(HttpContext aHttpContext, bool aBundlingAndMinificationEnabled)
        {
            //#TODO (DE) 2018-08-21: Improve this once the authors have fixed:
            //#TODO (DE) 2018-08-21: https://github.com/aspnetboilerplate/aspnetboilerplate/issues/3673
            //#TODO (DE) 2018-08-21: https://github.com/tmenier/Flurl/issues/365   //Don't like the proposed solution. This will be sorted when we move to the developer localised domains

            //Get the contents of what ABP needs to return
            var result =$"https://{aHttpContext.Request.Host}/abpscripts/getscripts";

            if (aBundlingAndMinificationEnabled)
            {
                var resultMinified = Uglify.Js(result);
                return resultMinified.HasErrors ? result : resultMinified.Code;
            }
            //Return what we have constructed
            return result;
        }
    }
}
