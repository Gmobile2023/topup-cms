using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.AgentManagerment;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Dtos.Partner;
using HLS.Topup.Web.Areas.App.Models.AgentsManage;
using JetBrains.Annotations;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    public class AgentsSupperController : TopupControllerBase
    {
        private readonly IAgentManagermentAppService _agentManage;
        public AgentsSupperController(IAgentManagermentAppService agentManage)
        {
            _agentManage = agentManage;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<PartialViewResult> ViewAgentSupperModal(int? id)
        {
            var modal = await _agentManage.GetAgentSupperDetail(id ?? 0);

            return PartialView("_AgentDetailSupperModal", modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_Edit)]
        public async Task<PartialViewResult> EditAgentSupperModal(int? id)
        {
            var modal = new AgentSupperDetailView()
            {
                ContractRegister = System.DateTime.Now,
                PartnerConfig = new PartnerConfigTransDto(),
                IdentityServerStorage=new IdentityServerStorageInputDto()
            };
            if (id > 0)
                modal = await _agentManage.GetAgentSupperDetail(id ?? 0);

            return PartialView("_EditAgentSupperModal", modal);
        }
    }
}
