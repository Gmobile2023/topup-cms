using System.Collections.Generic;
using HLS.Topup.Common;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using HLS.Topup.StockManagement.Exporting;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Aspose.Cells;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Notifications;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using HLS.Topup.Services;
using HLS.Topup.StockManagement.Importing;
using HLS.Topup.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.StockManagement
{
    public class CardsAppService : TopupAppServiceBase, ICardsAppService
    {
        private readonly ICardsExcelExporter _cardsExcelExporter;
        private readonly ICardManager _cardManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ICardListExcelDataReader _cardListExcelDataReader;
        private readonly ICommonLookupAppService _commonSv;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<Provider, int> _lookupProviderRepository;
        private readonly IRepository<Service> _serivceRepository;
        private readonly IRepository<Category, int> _lookupCategoryRepository;
        private readonly IRepository<Product, int> _lookupProductRepository;
        private readonly ILogger<CardsAppService> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CardsAppService(ICardsExcelExporter cardsExcelExporter, IBinaryObjectManager binaryObjectManager,
            ICommonLookupAppService commonSv,
            ICategoryManager categoryManager,
            ICardManager cardManager, IWebHostEnvironment env, IAppNotifier appNotifier,
            ICardListExcelDataReader cardListExcelDataReader,
            IRepository<Category, int> lookupCategoryRepository,
            IRepository<Service> serivceRepository,
            IRepository<Product, int> lookupProductRepository,
            IRepository<Provider, int> lookupProviderRepository,
            ILogger<CardsAppService> logger, IUnitOfWorkManager unitOfWorkManager)
        {
            _cardsExcelExporter = cardsExcelExporter;
            _cardManager = cardManager;
            _cardsExcelExporter = cardsExcelExporter;
            _appConfiguration = env.GetAppConfiguration();
            _binaryObjectManager = binaryObjectManager;
            _cardListExcelDataReader = cardListExcelDataReader;
            _appNotifier = appNotifier;
            _lookupProviderRepository = lookupProviderRepository;
            _commonSv = commonSv;
            _lookupCategoryRepository = lookupCategoryRepository;
            _categoryManager = categoryManager;
            _serivceRepository = serivceRepository;
            _lookupProductRepository = lookupProductRepository;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<PagedResultDto<CardDto>> GetAll(GetAllCardsInput input)
        {
            try
            {
                var request = new CardGetListRequest
                {
                    Filter = input.Filter,

                    BatchCode = input.BatchCodeFilter,
                    Serial = input.SerialFilter,
                    //CardCode = input.CardCodeFilter,
                    FromExpiredDate = input.MinExpiredDateFilter, // ?? DateTime.Now,
                    ToExpiredDate = input.MaxExpiredDateFilter, // ?? DateTime.Now,

                    ProviderCode = input.ProviderCodeFilter,
                    StockCode = input.StockCodeFilter,

                    ServiceCode = input.ServiceCodeFilter,
                    CategoryCode = input.CategoryCodeFilter,

                    FormCardValue = input.MaxCardValueFilter,
                    ToCardValue = input.MinCardValueFilter,

                    FromImportDate = input.MinImportedDateFilter, // ?? DateTime.Now,
                    ToImportDate = input.MaxImportedDateFilter, // ?? DateTime.Now,

                    FromExportedDate = input.MinExportedDateFilter,
                    ToExportedDate = input.MaxExportedDateFilter,

                    Status = input.StatusFilter,

                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                };
                var rs = await _cardManager.CardGetListRequest(request);

                var totalCount = rs.Total;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<CardDto>(
                        0,
                        new List<CardDto>()
                    );

                var data = rs.Payload.ConvertTo<List<CardDto>>();
                if (data.Any())
                {
                    var pCodeList = data.Select(x => x.ProviderCode).ToList();
                    var providerList = await _lookupProviderRepository.GetAll()
                        .Where(x => pCodeList.Contains(x.Code)).ToListAsync();
                    var serviceCardList = await ServiceCardList();
                    List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);

                    foreach (var item in data)
                    {
                        var p = providerList.FirstOrDefault(x => x.Code == item.ProviderCode);
                        item.ProviderName = p != null ? p.Name : item.ProviderCode;
                        var c = categoryList.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                        item.CategoryName = c != null ? c.CategoryName : item.CategoryCode;
                        var s = serviceCardList.FirstOrDefault(x => x.Id == item.ServiceCode);
                        item.ServiceName = s != null ? s.DisplayName : item.ServiceCode;
                    }
                }

                return new PagedResultDto<CardDto>(
                    totalCount, data
                );
            }
            catch (Exception ex)
            {
                return new PagedResultDto<CardDto>(
                    0,
                    new List<CardDto>()
                );
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<GetCardForViewDto> GetCardForView(Guid id)
        {
            var rs = await _cardManager.CardGetRequest(new CardGetRequest { Id = id });
            if (rs == null || rs.ResponseCode != "1")
                return new GetCardForViewDto
                {
                    Card = new CardDto
                    {
                    }
                };
            var card = rs.Payload.ConvertTo<CardDto>();

            var provider = await _lookupProviderRepository.FirstOrDefaultAsync(x => x.Code == card.ProviderCode);
            var category =
                await _lookupCategoryRepository.FirstOrDefaultAsync(x => x.CategoryCode == card.CategoryCode);
            var service = (await ServiceCardList()).FirstOrDefault(x => x.Id == card.ServiceCode);

            card.ProviderName = provider == null ? card.ProviderCode : provider.Name;
            card.CategoryName = category == null ? card.CategoryCode : category.CategoryName;
            card.ServiceName = service == null ? card.ServiceCode : service.DisplayName;
            return new GetCardForViewDto
            {
                Card = card,
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Cards_Edit)]
        public async Task<GetCardForEditOutput> GetCardForEdit(Guid id)
        {
            var rs = await _cardManager.CardGetFullRequest(new CardGetFullRequest { Id = id });
            if (rs == null || rs.ResponseCode != "1")
                return new GetCardForEditOutput
                {
                    Card = new CreateOrEditCardDto
                    {
                    }
                };

            var card = rs.Payload.ConvertTo<CreateOrEditCardDto>();

            var provider = await _lookupProviderRepository.FirstOrDefaultAsync(x => x.Code == card.ProviderCode);
            var category =
                await _lookupCategoryRepository.FirstOrDefaultAsync(x => x.CategoryCode == card.CategoryCode);
            var service = (await ServiceCardList()).FirstOrDefault(x => x.Id == card.ServiceCode);

            card.ProviderName = provider == null ? card.ProviderCode : provider.Name;
            card.CategoryName = category == null ? card.CategoryCode : category.CategoryName;
            card.ServiceName = service == null ? card.ServiceCode : service.DisplayName;

            return new GetCardForEditOutput
            {
                Card = card
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task CreateOrEdit(CreateOrEditCardDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Cards_Create)]
        private async Task Create(CreateOrEditCardDto input)
        {
            var rs = await _cardManager.CardImportRequest(new CardImportRequest
            {
                CardItem = new CardItem
                {
                    CardCode = input.CardCode,
                    CardValue = input.CardValue,
                    ExpiredDate = input.ExpiredDate,
                    Serial = input.Serial,
                    Status = (byte)input.Status,
                },
                BatchCode = input.BatchCode,
            });
            if (rs.ResponseCode != "1")
                throw new UserFriendlyException(rs.ResponseMessage);
        }

        [AbpAuthorize(AppPermissions.Pages_Cards_Edit)]
        private async Task Update(CreateOrEditCardDto input)
        {
            var rs = await _cardManager.CardUpdateRequest(new CardUpdateRequest
            {
                Id = input.Id ?? Guid.NewGuid(),
                CardCode = input.CardCode,
                CardValue = input.CardValue,
                ExpiredDate = input.ExpiredDate,
                Serial = input.Serial,
                Status = (byte)input.Status,
            });
            if (rs.ResponseCode != "1")
                throw new UserFriendlyException(rs.ResponseMessage);
        }

        [AbpAuthorize(AppPermissions.Pages_Cards_Delete)]
        public Task Delete(Guid id)
        {
            return null;
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<FileDto> GetCardsToExcel(GetAllCardsForExcelInput input)
        {
            var request = new CardGetListRequest
            {
                Filter = input.Filter,

                BatchCode = input.BatchCodeFilter,
                Serial = input.SerialFilter,
                FromExpiredDate = input.MinExpiredDateFilter, // ?? DateTime.Now,
                ToExpiredDate = input.MaxExpiredDateFilter, // ?? DateTime.Now,

                ProviderCode = input.ProviderCodeFilter,
                StockCode = input.StockCodeFilter,

                ServiceCode = input.ServiceCodeFilter,
                CategoryCode = input.CategoryCodeFilter,

                FormCardValue = input.MaxCardValueFilter,
                ToCardValue = input.MinCardValueFilter,

                FromImportDate = input.MinImportedDateFilter, // ?? DateTime.Now,
                ToImportDate = input.MaxImportedDateFilter, // ?? DateTime.Now,

                FromExportedDate = input.MinExportedDateFilter,
                ToExportedDate = input.MaxExportedDateFilter,

                Status = input.StatusFilter,

                Limit = Int32.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
            };

            var rs = await _cardManager.CardGetListRequest(request);
            var data = rs.Payload;
            //var data = rs.Payload.ConvertTo<List<CardDto>>();
            if (data.Any())
            {
                var pCodeList = data.Select(x => x.ProviderCode).ToList();
                var providerList = await _lookupProviderRepository.GetAll()
                    .Where(x => pCodeList.Contains(x.Code)).ToListAsync();
                var serviceCardList = await ServiceCardList();
                List<CategoryDto> categoryList = await _commonSv.GetCategoryUseCard(false);

                foreach (var item in data)
                {
                    var p = providerList.FirstOrDefault(x => x.Code == item.ProviderCode);
                    item.ProviderName = p != null ? p.Name : item.ProviderCode;
                    var c = categoryList.FirstOrDefault(x => x.CategoryCode == item.CategoryCode);
                    item.CategoryName = c != null ? c.CategoryName : item.CategoryCode;
                    var s = serviceCardList.FirstOrDefault(x => x.Id == item.ServiceCode);
                    item.ServiceName = s != null ? s.DisplayName : item.ServiceCode;
                }

                return _cardsExcelExporter.ExportToFile(data);
            }

            return _cardsExcelExporter.ExportToFile(new List<CardResponseDto>());
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public List<decimal> CardValues()
        {
            var unit = _appConfiguration["CardConfig:CardValues"];
            return unit.Split(',').Select(decimal.Parse).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<GetCardForViewDto> GetCardForViewFull(Guid id)
        {
            var rs = await _cardManager.CardGetFullRequest(new CardGetFullRequest { Id = id });

            if (rs == null || rs.ResponseCode != "1")

                return new GetCardForViewDto
                {
                    Card = new CardDto
                    {
                    }
                };

            return new GetCardForViewDto
            {
                Card = rs.Payload.ConvertTo<CardDto>()
            };
        }

        #region Import excel

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task ImportCardsJob(Guid fileObjectId, string provider, string dataStr, string description,
            UserIdentifier user)
        {
            using var uow = _unitOfWorkManager.Begin();
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                await Process(fileObjectId, provider, dataStr, description, user);
            }

            await uow.CompleteAsync();
        }

        private async Task Process(Guid fileObjectId, string provider, string dataStr, string description,
            UserIdentifier user)
        {
            if (string.IsNullOrEmpty(dataStr))
            {
                await _appNotifier.SendMessageAsync(user, "Lỗi không xác định được thông tin chiết khấu nhập hàng",
                    Abp.Notifications.NotificationSeverity.Error);
                return;
            }

            var dataDiscount = dataStr.FromJson<List<CardImport>>();

            var file = await _binaryObjectManager.GetOrNullAsync(fileObjectId);
            if (file == null)
            {
                await _appNotifier.SendMessageAsync(user, L("FileNotFound"),
                    Abp.Notifications.NotificationSeverity.Error);
                return;
            }

            var list = _cardListExcelDataReader.GetCardsFromExcel(file.Bytes);
            if (list == null || !list.Any())
            {
                await _appNotifier.SendMessageAsync(user, "Lỗi không load được dữ liệu file import",
                    Abp.Notifications.NotificationSeverity.Error);
                return;
            }

            var response = await GetCardImportList(list);
            if (response.ResponseCode != "1")
            {
                await _appNotifier.SendMessageAsync(user, response.ResponseMessage,
                    Abp.Notifications.NotificationSeverity.Error);
                return;
            }

            var dataImport = response.Payload.ConvertTo<List<CardImport>>();
            Parallel.ForEach(dataImport, d =>
            {
                var discount = dataDiscount.FirstOrDefault();
                if (discount != null)
                    d.Discount = discount.Discount;
                else
                    d.Discount = 0;
            });
            var request = new CardImportFileModel()
            {
                Data = new List<CardImportFileItemModel>(),
                Description = description,
                Provider = provider,
            };
            foreach (var item in dataImport)
            {
                var discount = dataDiscount.FirstOrDefault(x => x.ProductCode == item.ProductCode);
                item.Discount = discount?.Discount ?? 0;
                request.Data.Add(item.ConvertTo<CardImportFileItemModel>());
            }

            try
            {
                var rs = await _cardManager.CardImportFileRequest(request);
                _logger.LogInformation($"ImportCardsJob: {rs.ResponseCode}|{rs.ResponseMessage}");
                if (rs.ResponseCode == "1")
                {
                    await _appNotifier.SendMessageAsync(user,
                        "Nhập thẻ qua file thành công: " + request.Data.Sum(x => x.Quantity) + " thẻ.",
                        Abp.Notifications.NotificationSeverity.Success);
                    return;
                }
                else
                {
                    await _appNotifier.SendMessageAsync(user, rs.ResponseMessage,
                        Abp.Notifications.NotificationSeverity.Error);
                    return;
                }
            }
            catch (Exception e)
            {
                await _appNotifier.SendMessageAsync(user, e.Message, Abp.Notifications.NotificationSeverity.Error);
                return;
            }
        }

        public async Task<ResponseMessages> GetCardImportList(List<CardImportItem> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin dữ liệu không hợp lệ"
                };
            }

            var list = dataList.Where(x => x != null).ToList();
            if (!list.All(x => x.CanBeImported()))
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin dữ liệu không hợp lệ: " +
                                      list.FirstOrDefault(x => !x.CanBeImported())?.Exception,
                    Payload = list
                };
            }

            var totalCheck = dataList.Where(c =>
                string.IsNullOrEmpty(c.Serial) || string.IsNullOrEmpty(c.CardCode) || c.ExpiredDate == null).Count();
            if (totalCheck > 0)
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Dữ liệu trong file không hợp lệ. Quý khác kiểm tra lại trường dữ liệu."
                };
            }

            var serials = list.GroupBy(x => x.Serial).Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Counter = y.Count() }).ToList();
            if (serials.Any())
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại dữ liệu, trùng serial"
                };
            }

            var cardCodes = list.GroupBy(x => x.CardCode).Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Counter = y.Count() }).ToList();

            if (cardCodes.Any())
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại dữ liệu, trùng mã"
                };
            }

            var services = await ServiceCardList();
            var categories = await CategoryCardList();
            var product = await ProductCardList();
            var data = list
                .Where(x => services.Select(s => s.Id).Contains(x.ServiceCode))
                .Where(x => categories.Select(s => s.Id).Contains(x.CategoryCode))
                .GroupBy(x => new { x.ServiceCode, x.CategoryCode, x.CardValue })
                .Select(x =>
                {
                    var sv = services.FirstOrDefault(s => s.Id == x.Key.ServiceCode);
                    if (sv == null)
                        return new CardImport()
                        {
                            ServiceCode = ""
                        };
                    var cate = categories.FirstOrDefault(s => s.Id == x.Key.CategoryCode);
                    if (cate == null)
                        return new CardImport()
                        {
                            CategoryCode = ""
                        };
                    if (!cate.Payload.ToString().ToLower().Equals(sv.Id.ToLower()))
                        return new CardImport()
                        {
                            CategoryCode = "Error"
                        };
                    var keyProd = x.Key.CardValue > 0 ? x.Key.CardValue / 1000 : x.Key.CardValue;
                    var prod = product.FirstOrDefault(s => s.Id == (x.Key.CategoryCode + "_" + keyProd));
                    if (prod == null)
                        return new CardImport()
                        {
                            ProductCode = ""
                        };
                    var prodCate = prod.Payload.GetType().GetProperty("CategoryCode")?.GetValue(prod.Payload, null)
                        .ToString();
                    if (string.IsNullOrEmpty(prodCate) || !prodCate.ToLower().Equals(cate.Id.ToLower()))
                        return new CardImport()
                        {
                            ProductCode = "Error"
                        };
                    var amount = x.Key.CardValue * x.Count();
                    return new CardImport()
                    {
                        ServiceCode = sv.Id,
                        ServiceName = sv.DisplayName,
                        CategoryCode = cate.Id,
                        CategoryName = cate.DisplayName,
                        ProductCode = prod.Id,
                        ProductName = prod.DisplayName,
                        CardValue = x.Key.CardValue,
                        Quantity = x.Count(),
                        Amount = amount,
                        Cards = x.ToList()
                    };
                }).Distinct().ToList();


            if (data.Any(x => !string.IsNullOrEmpty(x.ProductCode) && x.ProductCode.Equals("Error")))
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin thẻ và loại thẻ không phù hợp",
                    Payload = data
                };
            }

            if (data.Any(x => !string.IsNullOrEmpty(x.CategoryCode) && x.CategoryCode.Equals("Error")))
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin loại thẻ và dịch vụ thẻ không phù hợp",
                    Payload = data
                };
            }

            if (data.Any(x => string.IsNullOrEmpty(x.ProductCode)))
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin thẻ không hợp lệ",
                    Payload = data
                };
            }

            if (data.Any(x => string.IsNullOrEmpty(x.CategoryCode)))
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin loại thẻ không hợp lệ",
                    Payload = data
                };
            }

            if (data.Any(x => string.IsNullOrEmpty(x.ServiceCode)))
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Kiểm tra lại thông tin dịch vụ thẻ không hợp lệ",
                    Payload = data
                };
            }


            return new ResponseMessages()
            {
                ResponseCode = "1",
                ResponseMessage = "ok",
                Payload = data
            };
        }

        #endregion

        #region Import API

        [AbpAuthorize(AppPermissions.Pages_Cards_Create, AppPermissions.Pages_Cards_Edit)]
        public async Task<NewMessageReponseBase<string>> ImportCardsApi(CardApiImportDto input)
        {
            var user = UserManager.GetUserById(AbpSession.UserId ?? 0);
            var request = input.ConvertTo<CardImportApiRequest>();
            request.PartnerCode = user.AccountCode;
            return await _cardManager.CardImportApiRequest(request);
        }

        #endregion

        [AbpAuthorize(AppPermissions.Pages_Cards_Edit)]
        public async Task UpdateCardStatus(CardUpdateStatusRequest input)
        {
            var rs = await _cardManager.CardUpdateStatusRequest(input);
            if (rs.ResponseCode != "1")
                throw new UserFriendlyException(rs.ResponseMessage);
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<List<CardProviderLookupTableDto>> GetAllProviderForTableDropdown()
        {
            return await _lookupProviderRepository.GetAll()
                .Select(provider => new CardProviderLookupTableDto
                {
                    Id = provider == null || provider.Code == null
                        ? ""
                        : provider.Code.ToString(),
                    DisplayName = provider == null || provider.Name == null ? "" : provider.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<List<CardVendorLookupTableDto>> GetAllVendorForTableDropdown()
        {
            var allCategory = await _commonSv.GetCategoryUseCard(false);
            return allCategory.Select(p => new CardVendorLookupTableDto
            {
                Id = p == null || p.CategoryCode == null
                    ? ""
                    : p.CategoryCode.ToString(),
                DisplayName = p == null || p.CategoryName == null ? "" : p.CategoryName.ToString()
            }).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<List<CardBatchLookupTableDto>> GetAllCardBatchForTableDropdown()
        {
            var request = new CardBatchGetListRequest
            {
                Limit = Int32.MaxValue,
                Offset = 0,
                Status = 1,
                SearchType = SearchType.Export,
            };
            var rs = await _cardManager.CardBatchGetListRequest(request);

            if (rs.ResponseCode != "1")
                return new List<CardBatchLookupTableDto>();
            var data = rs.Payload.ConvertTo<List<CardBatchDto>>();
            if (data.Any())
            {
                var pCodeList = data.Select(x => x.ProviderCode).ToList();
                var providerList = await _lookupProviderRepository.GetAll().Where(x => pCodeList.Contains(x.Code))
                    .ToListAsync();

                return data.Select(c =>
                {
                    var item = new CardBatchLookupTableDto();
                    var p = providerList.FirstOrDefault(x => x.Code == c.ProviderCode);
                    item.Id = c == null || c.BatchCode == null ? "" : c.BatchCode.ToString();
                    item.DisplayName = c == null || c.BatchCode == null ? "" : c.BatchCode.ToString();
                    item.ProviderCode = c == null || c.ProviderCode == null ? "" : c.ProviderCode.ToString();
                    item.ProviderName = p == null || p.Name == null ? "" : p.Name.ToString();
                    return item;
                }).ToList();
            }

            return new List<CardBatchLookupTableDto>();
        }

        [AbpAuthorize]
        public async Task<List<ProductDto>> GetProductByCategory(string categoryCode)
        {
            return await _categoryManager.GetProductByCategory(categoryCode);
        }

        [AbpAuthorize]
        public async Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode)
        {
            return await _categoryManager.GetCategoryByServiceCode(serviceCode);
        }

        // job nhập kho - ko phân quyền
        private async Task<List<CommonLookupTableDto>> ServiceCardList()
        {
            var data = await _serivceRepository.GetAll()
                .Where(x => x.ServiceCode.StartsWith("PIN_"))
                .Select(p => new CommonLookupTableDto
                {
                    Id = p.ServiceCode,
                    DisplayName = p == null || p.ServicesName == null ? "" : p.ServicesName.ToString(),
                    Payload = ""
                }).ToListAsync();
            return data;
        }

        // job nhập kho - ko phân quyền
        public async Task<List<CommonLookupTableDto>> CategoryCardList(string serviceCode = null, bool isAll = true)
        {
            if (!isAll && string.IsNullOrEmpty(serviceCode))
                return null;
            var service = await ServiceCardList();
            var data = await _lookupCategoryRepository.GetAllIncluding(s => s.ServiceFk)
                .Where(x => service.Select(s => s.Id).Contains(x.ServiceFk.ServiceCode))
                .WhereIf(!string.IsNullOrEmpty(serviceCode),
                    x => x.ServiceFk.ServiceCode == serviceCode)
                .Select(p => new CommonLookupTableDto
                {
                    Id = p.CategoryCode,
                    DisplayName = p == null || p.CategoryName == null ? "" : p.CategoryName.ToString(),
                    Payload = p.ServiceFk.ServiceCode
                }).ToListAsync();
            return data;
        }

        // job nhập kho - ko phân quyền
        public async Task<List<CommonLookupTableDto>> ProductCardList(string categoryCode = null, bool isAll = true)
        {
            if (!isAll && string.IsNullOrEmpty(categoryCode))
                return null;
            var categories = await CategoryCardList();
            var data = await _lookupProductRepository.GetAllIncluding(s => s.CategoryFk)
                .Where(x => categories.Select(s => s.Id).Contains(x.CategoryFk.CategoryCode))
                .WhereIf(!string.IsNullOrEmpty(categoryCode),
                    x => x.CategoryFk.CategoryCode == categoryCode)
                .Select(p => new CommonLookupTableDto
                {
                    Id = p.ProductCode,
                    DisplayName = p == null || p.ProductName == null ? "" : p.ProductName.ToString(),
                    Payload = (new
                        { p.CategoryFk.CategoryCode, p.ProductCode, p.ProductName, p.ProductValue, p.ProductType })
                }).ToListAsync();
            return data;
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<PagedResultDto<StockTransRequestDto>> GetCardStockTransList(CardStockTransListInput input)
        {
            try
            {
                var request = input.ConvertTo<CardStockTransListRequest>();
                request.Status = input.Status;
                request.Limit = input.MaxResultCount;
                request.Offset = input.SkipCount;
                var rs = await _cardManager.CardStockTransListAsync(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<StockTransRequestDto>(
                        0,
                        new List<StockTransRequestDto>()
                    );

                var data = rs.Payload.ConvertTo<List<StockTransRequestDto>>();
                return new PagedResultDto<StockTransRequestDto>(totalCount, data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCardStockTransList Exception: {ex}");
                return new PagedResultDto<StockTransRequestDto>(
                    0,
                    new List<StockTransRequestDto>()
                );
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<FileDto> GetCardStockTransToExcel(CardStockTransForExcelInput input)
        {
            var request = input.ConvertTo<CardStockTransListRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;

            var rs = await _cardManager.CardStockTransListAsync(request);
            var data = rs.Payload;
            if (data.Any())
            {
                return _cardsExcelExporter.ExportListToFile(data);
            }

            return _cardsExcelExporter.ExportListToFile(new List<StockTransRequestDto>());
        }

        [AbpAuthorize(AppPermissions.Pages_Cards)]
        public async Task<ResponseMessages> CheckTransStockProvider(StockCardApiCheckTransRequest input)
        {
            var rs = await _cardManager.StockCardApiCheckTransRequest(input);
            return new ResponseMessages
            {
                ResponseCode = rs.ResponseStatus.ErrorCode,
                ResponseMessage = rs.ResponseStatus.Message,
                ExtraInfo = rs.Results.ToJson()
            };
        }
    }
}