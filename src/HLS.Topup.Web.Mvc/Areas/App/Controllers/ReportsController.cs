using System.Linq;
using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using HLS.Topup.Common;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Web.Areas.App.Models.CardStocks;
using HLS.Topup.Web.Areas.App.Models.Reports;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization.Accounts;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    public class ReportsController : TopupControllerBase
    {
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly IPrivateAppService _privateService;


        public ReportsController(ICommonLookupAppService commonLookupAppService,
            IPrivateAppService privateService)
        {
            _commonLookupAppService = commonLookupAppService;
            _privateService = privateService;
        }

        // GET
        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportDetailBalanceAccount)]
        public async Task<IActionResult> BalanceAccount(string key)
        {
            var module = new ReportAccountDetailViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AgentTypes = await _privateService.GetAgenTypeInfo(),
            };
            if (!string.IsNullOrEmpty(key))
            {
                var skey = key.Split('|');
                if (!string.IsNullOrEmpty(skey[0]))
                {
                    module.FromDate = new DateTime(Convert.ToInt32(skey[0].Split('-')[0]), Convert.ToInt32(skey[0].Split('-')[1]), Convert.ToInt32(skey[0].Split('-')[2]));
                }

                if (skey.Length >= 2 && !string.IsNullOrEmpty(skey[1]))
                {
                    module.ToDate = new DateTime(Convert.ToInt32(skey[1].Split('-')[0]), Convert.ToInt32(skey[1].Split('-')[1]), Convert.ToInt32(skey[1].Split('-')[2]));
                }

                if (skey.Length >= 3 && !string.IsNullOrEmpty(skey[2]))
                {
                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[2]).Result;
                    module.AgentCode = skey[2];
                    if (user != null)
                    {
                        module.AgentInfo = user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName;
                    }
                }

            }
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportDetailBalanceAccount)]
        public async Task<IActionResult> BalanceAccounts()
        {
            var module = new ReportAccountDetailViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AgentTypes = await _privateService.GetAgenTypeInfo(),
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportSumTotalBalanceAccount)]
        public async Task<IActionResult> TotalBalance()
        {
            var module = new ReportAccountDetailViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AgentTypes = await _privateService.GetAgenTypeInfo(),
            };
            return View(module);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportTotalDebt)]
        public IActionResult TotalDebtBalance()
        {

            return View();
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportDebtDetail)]
        public IActionResult AccountDebtDetail(string key)
        {

            var model = new AccountDebtDetailViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            };
            if (!string.IsNullOrEmpty(key))
            {
                var skey = key.Split('|');
                if (!string.IsNullOrEmpty(skey[0]))
                {
                    model.FromDate = new System.DateTime(Convert.ToInt32(skey[0].Split('-')[0]), Convert.ToInt32(skey[0].Split('-')[1]), Convert.ToInt32(skey[0].Split('-')[2]));
                }

                if (skey.Length >= 2 && !string.IsNullOrEmpty(skey[1]))
                {
                    model.ToDate = new System.DateTime(Convert.ToInt32(skey[1].Split('-')[0]), Convert.ToInt32(skey[1].Split('-')[1]), Convert.ToInt32(skey[1].Split('-')[2]));
                }

                if (skey.Length >= 3 && !string.IsNullOrEmpty(skey[2]))
                {
                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[2]).Result;
                    model.SaleCode = skey[2];
                    if (user != null)
                    {
                        model.SaleInfo = user.UserName + " - " + user.PhoneNumber + " - " + user.FullName;
                    }
                }

                if (skey.Length >= 5 && !string.IsNullOrEmpty(skey[4]))
                {
                    model.Type = skey[4];
                }

            }
            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportRefundDetail)]
        public async Task<IActionResult> ReportRefundDetail()
        {
            var list = await _commonLookupAppService.GetListVendorTrans("");
            var services = await _commonLookupAppService.GetSetvices(false);
            var module = new ReportServiceViewModel()
            {
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),

                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportTransferDetail)]
        public async Task<IActionResult> ReportTransferDetail(string key)
        {
            var module = new ReportTransferViewModel()
            {
                AgentTypes = await _privateService.GetAgenTypeInfo(),
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportServiceDetail)]
        public async Task<IActionResult> ReportServiceDetail(string key)
        {
            
            var list = await _commonLookupAppService.GetProvider(false);
            var services = await _commonLookupAppService.GetSetvices(false);
            var users = await _privateService.GetUserInfoQuery(new Authorization.Users.Dto.GetUserInfoRequest()
            {
                UserId = AbpSession.UserId
            });
            var module = new ReportServiceViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AccountType = users.AccountType.ToString("G"),
                AgentTypes = await _privateService.GetAgenTypeInfo(),
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),

                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
            };

            if (!string.IsNullOrEmpty(key))
            {
                var skey = key.Split('|');
                module.Status = -1;

                //0: FromDate => Từ ngày
                //1: ToDate => Tới ngày
                //2: Status => Trạng thái
                //3: ProductCode => Mã sản phẩm
                //4: AgentCode => Mã đại lý
                //5: SaleCode => Mã nhân viên kinh doanh
                //6: ServiceCode => Mã dịch vụ
                //7: CityId => Id của Tỉnh/TP
                //8: DistrictId => Id của Quận - Huyện
                //9: WardId => Id của Phường -Xã
                //10: ProviderCode => Nhà cung cấp
                if (!string.IsNullOrEmpty(skey[0]))
                {
                    module.FromDate = new DateTime(Convert.ToInt32(skey[0].Split('-')[0]), Convert.ToInt32(skey[0].Split('-')[1]), Convert.ToInt32(skey[0].Split('-')[2]));
                }

                if (skey.Length >= 2 && !string.IsNullOrEmpty(skey[1]))
                {
                    module.ToDate = new DateTime(Convert.ToInt32(skey[1].Split('-')[0]), Convert.ToInt32(skey[1].Split('-')[1]), Convert.ToInt32(skey[1].Split('-')[2]));
                }

                if (skey.Length >= 3 && !string.IsNullOrEmpty(skey[2]))
                {
                    module.Status = Convert.ToInt32(skey[2]);
                }

                if (skey.Length >= 4 && !string.IsNullOrEmpty(skey[3]))
                {
                    var product = await _commonLookupAppService.GetProductByCode(skey[3]);
                    module.ProductCode = skey[3];
                    if (product != null)
                    {
                        module.ProductName = product.ProductName;
                        module.CategoryCode = product.CategoryCode;
                        module.CategoryName = product.CategoryName;
                    }
                }

                if (skey.Length >= 5 && !string.IsNullOrEmpty(skey[4]))
                {

                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[4]).Result;
                    module.AgentCode = skey[4];
                    if (user != null)
                    {
                        module.AgentInfo = user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName;
                    }
                }

                if (skey.Length >= 6 && !string.IsNullOrEmpty(skey[5]))
                {

                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[5]).Result;
                    module.SaleCode = skey[5];
                    if (user != null)
                    {
                        module.SaleInfo = user.UserName + " - " + user.PhoneNumber + " - " + user.FullName;
                    }
                }

                if (skey.Length >= 7 && !string.IsNullOrEmpty(skey[6]))
                {
                    module.ServiceCode = skey[6];
                }

                if (skey.Length >= 8 && !string.IsNullOrEmpty(skey[7]))
                {
                    module.CityId = Convert.ToInt32(skey[7]);
                    var city = await _commonLookupAppService.GetCityById(module.CityId);
                    module.CityName = city != null ? city.CityName : string.Empty;
                }

                if (skey.Length >= 9 && !string.IsNullOrEmpty(skey[8]))
                {
                    module.DistrictId = Convert.ToInt32(skey[8]);
                    var district = await _commonLookupAppService.GetDistrictsById(module.DistrictId);
                    module.DistrictName = district != null ? district.DistrictName : string.Empty;
                }

                if (skey.Length >= 10 && !string.IsNullOrEmpty(skey[9]))
                {
                    module.WardId = Convert.ToInt32(skey[9]);
                    var ward = await _commonLookupAppService.GetWardsById(module.WardId);
                    module.WardName = ward != null ? ward.WardName : string.Empty;
                }

                if (skey.Length >= 11 && !string.IsNullOrEmpty(skey[10]))
                {
                    module.ProviderCode = skey[10];
                }

            }
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportServiceTotal)]
        public async Task<IActionResult> ReportServiceTotal()
        {
            var services = await _commonLookupAppService.GetSetvices(false);
            var module = new ReportServiceTotalViewModel()
            {
                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),               
            };

            module.AgentTypes = await _privateService.GetAgenTypeInfo();
            return View(module);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportServiceProvider)]
        public async Task<IActionResult> ReportServiceProvider()
        {
            var list = await _commonLookupAppService.GetProvider(false);
            var services = await _commonLookupAppService.GetSetvices(false);
            var users = await _privateService.GetUserInfoQuery(new Authorization.Users.Dto.GetUserInfoRequest()
            {
                UserId = AbpSession.UserId
            });

            var module = new ReportServiceProviderViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AccountType = users.AccountType.ToString("G"),
                AgentTypes = await _privateService.GetAgenTypeInfo(),
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),

                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
            };
            return View(module);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportAgentBalance)]
        public async Task<IActionResult> ReportAgentBalance()
        {
            var module = new ReportAgentBalanceViewModel()
            {
                AgentTypes = await _privateService.GetAgenTypeInfo()
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportRevenueAgent)]
        public async Task<IActionResult> ReportRevenueAgent(string key)
        {
            var services = await _commonLookupAppService.GetSetvices(false);
            var citys = await _commonLookupAppService.GetProvinces();
            var module = new ReportRevenueAgentViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AgentTypes = await _privateService.GetAgenTypeInfo(),
                Citys = (from x in citys
                         select new ComboboxItemDto()
                         {
                             Value = x.Id.ToString(),
                             DisplayText = x.CityName,
                         }).ToList(),
                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
            };

            if (!string.IsNullOrEmpty(key))
            {
                var skey = key.Split('|');
                module.Status = -1;
                if (!string.IsNullOrEmpty(skey[0]))
                {
                    module.FromDate = new DateTime(Convert.ToInt32(skey[0].Split('-')[0]), Convert.ToInt32(skey[0].Split('-')[1]), Convert.ToInt32(skey[0].Split('-')[2]));
                }

                if (skey.Length >= 2 && !string.IsNullOrEmpty(skey[1]))
                {
                    module.ToDate = new DateTime(Convert.ToInt32(skey[1].Split('-')[0]), Convert.ToInt32(skey[1].Split('-')[1]), Convert.ToInt32(skey[1].Split('-')[2]));
                }

                if (skey.Length >= 3 && !string.IsNullOrEmpty(skey[2]))
                {
                    module.Status = Convert.ToInt32(skey[2]);
                }

                if (skey.Length >= 4 && !string.IsNullOrEmpty(skey[3]))
                {
                    var product = await _commonLookupAppService.GetProductByCode(skey[3]);
                    module.ProductCode = skey[3];
                    if (product != null)
                    {
                        module.ProductName = product.ProductName;
                    }
                }
            }

            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportRevenueCity)]
        public async Task<IActionResult> ReportRevenueCity()
        {
            var services = await _commonLookupAppService.GetSetvices(false);
            var citys = await _commonLookupAppService.GetProvinces();
            var module = new ReportRevenueCityViewModel()
            {
                Citys = (from x in citys
                         select new ComboboxItemDto()
                         {
                             Value = x.Id.ToString(),
                             DisplayText = x.CityName,
                         }).ToList(),
                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
                AgentTypes = await _privateService.GetAgenTypeInfo()
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportTotalSaleAgent)]
        public async Task<IActionResult> ReportTotalSaleAgent()
        {
            var services = await _commonLookupAppService.GetSetvices(false);
            var module = new ReportTotalSaleAgentViewModel()
            {
                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
                AgentTypes = await _privateService.GetAgenTypeInfo()
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportRevenueActive)]
        public async Task<IActionResult> ReportRevenueActive()
        {
            var module = new ReportRevenueActiveViewModel()
            {
                AgentTypes = await _privateService.GetAgenTypeInfo()
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CardStockHistories)]
        public async Task<IActionResult> CardStockHistories()
        {
            var allCategory = await _commonLookupAppService.GetCategoryUseCard(false);
            var cate = allCategory.Select(p => new CardVendorLookupTableDto
            {
                Id = p == null || p.CategoryCode == null
                    ? ""
                    : p.CategoryCode.ToString(),
                DisplayName = p == null || p.CategoryName == null ? "" : p.CategoryName.ToString()
            }).ToList();
            var model = new CardStocksViewModel
            {
                FilterText = "",
                CardValues = _commonLookupAppService.CardValues(),
                VendorList = cate,
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CardStockImExPort)]
        public async Task<IActionResult> CardStockImExPort()
        {
            var allCategory = await _commonLookupAppService.GetCategoryUseCard(false);
            var cate = allCategory.Select(p => new CardVendorLookupTableDto
            {
                Id = p == null || p.CategoryCode == null
                    ? ""
                    : p.CategoryCode.ToString(),
                DisplayName = p == null || p.CategoryName == null ? "" : p.CategoryName.ToString()
            }).ToList();
            var model = new CardStocksViewModel
            {
                FilterText = "",
                CardValues = _commonLookupAppService.CardValues(),
                VendorList = cate,
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CardStockImExProvider)]
        public async Task<IActionResult> CardStockImExProvider()
        {
            var list = await _commonLookupAppService.GetProvider(false);
            var allCategory = await _commonLookupAppService.GetCategoryUseCard(false);
            var cate = allCategory.Select(p => new CardVendorLookupTableDto
            {
                Id = p == null || p.CategoryCode == null
                    ? ""
                    : p.CategoryCode.ToString(),
                DisplayName = p == null || p.CategoryName == null ? "" : p.CategoryName.ToString()
            }).ToList();

            var model = new CardStocksViewModel
            {
                FilterText = "",
                CardValues = _commonLookupAppService.CardValues(),
                VendorList = cate,
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_CardStockInventory)]
        public async Task<IActionResult> CardStockInventory()
        {
            var allCategory = await _commonLookupAppService.GetCategoryUseCard(false);
            var cate = allCategory.Select(p => new CardVendorLookupTableDto
            {
                Id = p == null || p.CategoryCode == null
                    ? ""
                    : p.CategoryCode.ToString(),
                DisplayName = p == null || p.CategoryName == null ? "" : p.CategoryName.ToString()
            }).ToList();
            var model = new CardStocksViewModel
            {
                FilterText = "",
                CardValues = _commonLookupAppService.CardValues(),
                VendorList = cate,
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportComparePartner)]
        public async Task<IActionResult> ReportComparePartner()
        {
            var module = new ReportComparePartnerViewModel()
            {
                AgentTypes = await _privateService.GetAgenTypeInfo(),
            };
            return View(module);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportComparePartner)]
        public async Task<PartialViewResult> ConfirmEmailModal(string agentCode, DateTime fromDate, DateTime toDate)
        {
            var model = new ComaprePartnerEmailViewModel()
            {
                AgentCode = agentCode,
                FromDateText = fromDate.ToString("yyyy-MM-dd"),
                ToDateText = toDate.ToString("yyyy-MM-dd")
            };

            return PartialView("_ConfirmEmailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Report_ReportTopupRequestLogs)]
        public async Task<IActionResult> ReportTopupRequestLogs(string key)
        {

            var list = await _commonLookupAppService.GetProvider(false);
            var services = await _commonLookupAppService.GetSetvices(false);
            var users = await _privateService.GetUserInfoQuery(new Authorization.Users.Dto.GetUserInfoRequest()
            {
                UserId = AbpSession.UserId
            });
            var module = new ReportServiceViewModel()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AccountType = users.AccountType.ToString("G"),
                AgentTypes = await _privateService.GetAgenTypeInfo(),
                Providers = (from x in list
                             select new ComboboxItemDto()
                             {
                                 Value = x.Code,
                                 DisplayText = x.Name,
                             }).ToList(),

                Services = (from x in services
                            select new ComboboxItemDto()
                            {
                                Value = x.ServiceCode,
                                DisplayText = x.ServicesName,
                            }).ToList(),
            };

            if (!string.IsNullOrEmpty(key))
            {
                var skey = key.Split('|');
                module.Status = -1;

                //0: FromDate => Từ ngày
                //1: ToDate => Tới ngày
                //2: Status => Trạng thái
                //3: ProductCode => Mã sản phẩm
                //4: AgentCode => Mã đại lý
                //5: SaleCode => Mã nhân viên kinh doanh
                //6: ServiceCode => Mã dịch vụ
                //7: CityId => Id của Tỉnh/TP
                //8: DistrictId => Id của Quận - Huyện
                //9: WardId => Id của Phường -Xã
                //10: ProviderCode => Nhà cung cấp
                if (!string.IsNullOrEmpty(skey[0]))
                {
                    module.FromDate = new DateTime(Convert.ToInt32(skey[0].Split('-')[0]), Convert.ToInt32(skey[0].Split('-')[1]), Convert.ToInt32(skey[0].Split('-')[2]));
                }

                if (skey.Length >= 2 && !string.IsNullOrEmpty(skey[1]))
                {
                    module.ToDate = new DateTime(Convert.ToInt32(skey[1].Split('-')[0]), Convert.ToInt32(skey[1].Split('-')[1]), Convert.ToInt32(skey[1].Split('-')[2]));
                }

                if (skey.Length >= 3 && !string.IsNullOrEmpty(skey[2]))
                {
                    module.Status = Convert.ToInt32(skey[2]);
                }

                if (skey.Length >= 4 && !string.IsNullOrEmpty(skey[3]))
                {
                    var product = await _commonLookupAppService.GetProductByCode(skey[3]);
                    module.ProductCode = skey[3];
                    if (product != null)
                    {
                        module.ProductName = product.ProductName;
                        module.CategoryCode = product.CategoryCode;
                        module.CategoryName = product.CategoryName;
                    }
                }

                if (skey.Length >= 5 && !string.IsNullOrEmpty(skey[4]))
                {

                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[4]).Result;
                    module.AgentCode = skey[4];
                    if (user != null)
                    {
                        module.AgentInfo = user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName;
                    }
                }

                if (skey.Length >= 6 && !string.IsNullOrEmpty(skey[5]))
                {

                    var user = _privateService.GetUserInfoQueryByAccountCode(skey[5]).Result;
                    module.SaleCode = skey[5];
                    if (user != null)
                    {
                        module.SaleInfo = user.UserName + " - " + user.PhoneNumber + " - " + user.FullName;
                    }
                }

                if (skey.Length >= 7 && !string.IsNullOrEmpty(skey[6]))
                {
                    module.ServiceCode = skey[6];
                }

                if (skey.Length >= 8 && !string.IsNullOrEmpty(skey[7]))
                {
                    module.CityId = Convert.ToInt32(skey[7]);
                    var city = await _commonLookupAppService.GetCityById(module.CityId);
                    module.CityName = city != null ? city.CityName : string.Empty;
                }

                if (skey.Length >= 9 && !string.IsNullOrEmpty(skey[8]))
                {
                    module.DistrictId = Convert.ToInt32(skey[8]);
                    var district = await _commonLookupAppService.GetDistrictsById(module.DistrictId);
                    module.DistrictName = district != null ? district.DistrictName : string.Empty;
                }

                if (skey.Length >= 10 && !string.IsNullOrEmpty(skey[9]))
                {
                    module.WardId = Convert.ToInt32(skey[9]);
                    var ward = await _commonLookupAppService.GetWardsById(module.WardId);
                    module.WardName = ward != null ? ward.WardName : string.Empty;
                }

                if (skey.Length >= 11 && !string.IsNullOrEmpty(skey[10]))
                {
                    module.ProviderCode = skey[10];
                }

            }
            return View(module);
        }
    }
}
