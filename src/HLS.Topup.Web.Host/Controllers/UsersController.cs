using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using HLS.Topup.Storage;
using Abp.BackgroundJobs;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}