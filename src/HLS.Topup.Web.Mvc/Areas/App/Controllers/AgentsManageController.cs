using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.AgentManagerment;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLS.Topup.AccountManagement;
using HLS.Topup.Common;
using HLS.Topup.Web.Areas.App.Models.AgentsManage;
using JetBrains.Annotations;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage)]
    public class AgentsManageController : TopupControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IAgentManagermentAppService _agentManage;
        private readonly IAccountManagementAppService _accountManagementService;
        private readonly ICommonLookupAppService _commonLookupAppService;

        public AgentsManageController(IUserAppService userAppService,
            UserManager userManager,
            IAgentManagermentAppService agentManage,
            IAccountManagementAppService accountManagementService,
            ICommonLookupAppService commonLookupAppService)
        {
            _userAppService = userAppService;
            _userManager = userManager;
            _agentManage = agentManage;
            _accountManagementService = accountManagementService;
            _commonLookupAppService = commonLookupAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> ViewAgentModal(int? id)
        {
            var modal = await _agentManage.GetAgentDetail(id ?? 0);

            return PartialView("_AgentDetailViewModal", modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_Edit)]
        public async Task<PartialViewResult> EditAgentModal(int? id)
        {
            var modal = await _agentManage.GetAgentDetail(id ?? 0);

            return PartialView("_EditAgentViewModal", modal);
        }

        //  [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_ConvertPhone)]
        public async Task<PartialViewResult> ConvertPhoneModal(int? id)
        {
            var modal = await _agentManage.GetAgentDetail(id ?? 0);

            return PartialView("_ConvertPhoneModal", modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_Assign)]
        public async Task<PartialViewResult> MappingSaleModal(int? id)
        {
            MappingSaleView modal = new MappingSaleView()
            {
                AgentId = id ?? 0,
            };
            return PartialView("_MappingSaleModal", modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_Assign)]
        public async Task<PartialViewResult> EditMapSaleModal(int? id)
        {
            var modal = await _agentManage.GetSaleAssignAgent(id ?? 0);
            return PartialView("_EditMapSaleModal", modal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_Lock, AppPermissions.Pages_AgentsManage_Unlock)]
        public async Task<PartialViewResult> LockOrUnlockAgentModal(int? id, [CanBeNull] string type)
        {
            var model = new LockOrUnlockModel()
            {
                Id = id,
                Type = type
            };
            return PartialView("_LockOrUnlockAgent", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AgentsManage_Create)]
        public async Task<PartialViewResult> CreateOrEditAgentModal(long? id, string type)
        {
            GetAgentForEditOutput getAgentForEditOutput;

            if (id.HasValue)
            {
                var userProfile = await _accountManagementService.GetAccount(new EntityDto<long>(id ?? 0));
                getAgentForEditOutput = new GetAgentForEditOutput
                {
                    Agent = userProfile
                };
            }
            else
            {
                getAgentForEditOutput = new GetAgentForEditOutput
                {
                    Agent = new UserProfileDto()
                    {
                        IsActive = true,
                        AgentType = type == "AgentGeneral"
                            ? CommonConst.AgentType.AgentGeneral
                            : type == "WholesaleAgent"
                                ? CommonConst.AgentType.WholesaleAgent
                                : type == "SubAgent"
                                    ? CommonConst.AgentType.SubAgent
                                    : CommonConst.AgentType.AgentGeneral
                    }
                };
            }

            var viewModel = new CreateOrEditAgentViewModel()
            {
                Agent = getAgentForEditOutput.Agent,
                Provinces = await _commonLookupAppService.GetProvinces(),
                Districts = await _commonLookupAppService.GetDistricts(getAgentForEditOutput.Agent.CityId ?? 0),
                Wards = await _commonLookupAppService.GetWards(getAgentForEditOutput.Agent.DistrictId ?? 0)
            };

            return PartialView("_CreateOrEditAgentModal", viewModel);
        }
    }
}