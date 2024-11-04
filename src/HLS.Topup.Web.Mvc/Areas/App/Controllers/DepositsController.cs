using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Deposits;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Deposits;
using HLS.Topup.Deposits.Dtos;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Deposits)]
    public class DepositsController : TopupControllerBase
    {
        private readonly IDepositsAppService _depositsAppService;
        private readonly ICommonLookupAppService _commonAppRepository;
        private readonly IPrivateAppService _privateService;

        public DepositsController(IDepositsAppService depositsAppService,
            ICommonLookupAppService commonAppRepository,
            IPrivateAppService privateService)
        {
            _depositsAppService = depositsAppService;
            _commonAppRepository = commonAppRepository;
            _privateService = privateService;
        }

        public async Task<IActionResult> Index(string key)
        {
            var model = new DepositsViewModel
            {
                FilterText = "",
                DepositBankList = await _depositsAppService.GetAllBankForTableDropdown(),
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            };

            if (!string.IsNullOrEmpty(key))
            {
                var skey = key.Split('|');
                if (!string.IsNullOrEmpty(skey[0]))
                {
                    model.FromDate = new DateTime(Convert.ToInt32(skey[0].Split('-')[0]), Convert.ToInt32(skey[0].Split('-')[1]), Convert.ToInt32(skey[0].Split('-')[2]));
                }

                if (skey.Length >= 2 && !string.IsNullOrEmpty(skey[1]))
                {
                    model.ToDate = new DateTime(Convert.ToInt32(skey[1].Split('-')[0]), Convert.ToInt32(skey[1].Split('-')[1]), Convert.ToInt32(skey[1].Split('-')[2]));
                }

                if (skey.Length >= 3 && !string.IsNullOrEmpty(skey[2]))
                {
                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[2]).Result;
                    if (user != null)
                    {
                        model.AgentInfo = user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName;
                        model.AgentId = user.Id;
                    }
                }

                if (skey.Length >= 4 && !string.IsNullOrEmpty(skey[3]))
                {
                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[3]).Result;
                    if (user != null)
                    {
                        model.SaleInfo = user.UserName + " - " + user.PhoneNumber + " - " + user.FullName;
                        model.SaleId = user.Id;
                    }
                }

                if (skey.Length >= 5 && !string.IsNullOrEmpty(skey[4]))
                {
                    model.Type = skey[4];
                }

                if (skey.Length >= 6 && !string.IsNullOrEmpty(skey[5]))
                {
                    model.Status = Convert.ToInt32(skey[5]);
                }

            }

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Deposits_Create, AppPermissions.Pages_Deposits_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDepositForEditOutput getDepositForEditOutput;

            if (id.HasValue)
            {
                getDepositForEditOutput = await _depositsAppService.GetDepositForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDepositForEditOutput = new GetDepositForEditOutput
                {
                    Deposit = new CreateOrEditDepositDto()
                };
            }

            var viewModel = new CreateOrEditDepositModalViewModel()
            {
                Deposit = getDepositForEditOutput.Deposit,
                UserName = getDepositForEditOutput.UserName,
                BankBankName = getDepositForEditOutput.BankBankName,
                UserName2 = getDepositForEditOutput.UserName2,
                DepositBankList = await _depositsAppService.GetAllBankForTableDropdown(),
                DepositUserList = new List<DepositUserLookupTableDto>(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> CreateDebtModal(int? id)
        {
            GetDepositForEditOutput getDepositForEditOutput;

            if (id.HasValue)
            {
                getDepositForEditOutput = await _depositsAppService.GetDepositForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDepositForEditOutput = new GetDepositForEditOutput
                {
                    Deposit = new CreateOrEditDepositDto()
                };
            }

            var viewModel = new CreateOrEditDepositModalViewModel()
            {
                Deposit = getDepositForEditOutput.Deposit,
                UserName = getDepositForEditOutput.UserName,
                BankBankName = getDepositForEditOutput.BankBankName,
                UserName2 = getDepositForEditOutput.UserName2,
                DepositBankList = await _depositsAppService.GetAllBankForTableDropdown(),
                DepositUserList = new List<DepositUserLookupTableDto>()
            };

            if (id > 0)
            {
                // var userSale = await _commonAppRepository.GetUserInfoQuery(new Authorization.Users.Dto.GetUserInfoRequest()
                // { UserId = viewModel.Deposit.UserSaleId });
                var limit = await _depositsAppService.GetLimitAvailability(viewModel.Deposit.UserSaleId ?? 0);
                viewModel.UserNameSale =
                    getDepositForEditOutput
                        .UserNameSale; //userSale != null ? userSale.AccountCode + " - " + userSale.PhoneNumber + " - " + userSale.FullName : "";
                viewModel.SaleLimit = Convert.ToDouble(limit);
            }

            return PartialView("_CreateDebtModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDepositModal(int id)
        {
            var getDepositForViewDto = await _depositsAppService.GetDepositForView(id);

            var model = new DepositViewModel()
            {
                Deposit = getDepositForViewDto.Deposit,
                UserName = getDepositForViewDto.UserName,
                BankBankName = getDepositForViewDto.BankBankName,
                UserName2 = getDepositForViewDto.UserName2,
                CreatorName = getDepositForViewDto.CreatorName,
                AgentType = getDepositForViewDto.AgentType
            };

            return PartialView("_ViewDepositModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Deposits_Create, AppPermissions.Pages_Deposits_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new DepositUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DepositUserLookupTableModal", viewModel);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Deposits_AccountingEntry)]
        public async Task<PartialViewResult> CreateOrEditAccountingEntryModal(int? id)
        {
            GetDepositForEditOutput getDepositForEditOutput;

            if (id.HasValue)
            {
                getDepositForEditOutput = await _depositsAppService.GetDepositForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDepositForEditOutput = new GetDepositForEditOutput
                {
                    Deposit = new CreateOrEditDepositDto()
                };
            }

            var viewModel = new CreateOrEditDepositModalViewModel()
            {
                Deposit = getDepositForEditOutput.Deposit,
                UserName = getDepositForEditOutput.UserName,
                BankBankName = getDepositForEditOutput.BankBankName,
                UserName2 = getDepositForEditOutput.UserName2,
                DepositBankList = await _depositsAppService.GetAllBankForTableDropdown(),
                DepositUserList = new List<DepositUserLookupTableDto>(),
            };

            return PartialView("_CreateOrEditAccountingEntryModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAccountingEntryModal(int id)
        {
            var getDepositForViewDto = await _depositsAppService.GetDepositForView(id);

            var model = new DepositViewModel()
            {
                Deposit = getDepositForViewDto.Deposit,
                UserName = getDepositForViewDto.UserName,
                BankBankName = getDepositForViewDto.BankBankName,
                UserName2 = getDepositForViewDto.UserName2,
                CreatorName = getDepositForViewDto.CreatorName,
                AgentType = getDepositForViewDto.AgentType
            };

            return PartialView("_ViewAccountingEntryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Deposits_Cash)]
        public async Task<PartialViewResult> CreateOrEditDepositCashModal(int? id)
        {
            GetDepositForEditOutput getDepositForEditOutput;

            if (id.HasValue)
            {
                getDepositForEditOutput = await _depositsAppService.GetDepositForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDepositForEditOutput = new GetDepositForEditOutput
                {
                    Deposit = new CreateOrEditDepositDto()
                };
            }

            var viewModel = new CreateOrEditDepositModalViewModel()
            {
                Deposit = getDepositForEditOutput.Deposit,
                UserName = getDepositForEditOutput.UserName,
                BankBankName = getDepositForEditOutput.BankBankName,
                UserName2 = getDepositForEditOutput.UserName2,
                DepositBankList = await _depositsAppService.GetAllBankForTableDropdown(),
                DepositUserList = new List<DepositUserLookupTableDto>(),
            };

            return PartialView("_CreateOrEditDepositCashModal", viewModel);
        }
        
        [AbpMvcAuthorize(AppPermissions.Pages_Deposits_Approval)]
        public async Task<PartialViewResult> ApprovedBankModal(string transCode)
        {
            var viewModel = new ApprovedBankModalViewModel()
            {
                TransCode = transCode
            };

            return PartialView("_ApprovedBankModal", viewModel);
        }
    }
}