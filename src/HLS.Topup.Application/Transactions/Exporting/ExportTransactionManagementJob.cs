using System;
using System.Collections.Generic;
using Abp;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Threading;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Files;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using HLS.Topup.Storage;
using HLS.Topup.Transactions.Dtos;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Transactions.Exporting
{
    public class ExportTransactionManagementJob : BackgroundJob<ExportTransactionManagementJobArgs>,
        ITransientDependency
    {
        private readonly IAppNotifier _appNotifier;
        private readonly ITransactionManager _transactionManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILogger<ExportTransactionManagementJob> _logger;
        private readonly IFileCommonAppService _fileCommon;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly ILocalizationSource _localizationSource;

        public ExportTransactionManagementJob(
            IAppNotifier appNotifier, ITransactionManager transactionManager,
            IBinaryObjectManager binaryObjectManager,
            //ILocalizationManager localizationManager,
            ILogger<ExportTransactionManagementJob> logger,
            IFileCommonAppService fileCommon, IUnitOfWorkManager unitOfWorkManager)
        {
            _appNotifier = appNotifier;
            _transactionManager = transactionManager;
            _binaryObjectManager = binaryObjectManager;
            _logger = logger;
            _fileCommon = fileCommon;
            _unitOfWorkManager = unitOfWorkManager;
            //_localizationSource = localizationManager.GetSource(TopupConsts.LocalizationSourceName);
        }

        public override void Execute(ExportTransactionManagementJobArgs args)
        {
            using var uow = _unitOfWorkManager.Begin();
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                Export(args);
            }
            uow.Complete();
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        private void Export(ExportTransactionManagementJobArgs input)
        {
            try
            {
                var perpage = decimal.Parse(input.Request.ItemPerPageConfig.ToString());
                var request = new TopupsListRequest
                {
                    PartnerCode = input.Request.PartnerCodeFilter,
                    StaffAccount = input.Request.StaffAccount,
                    StaffUser = input.Request.StaffUser,
                    Limit = input.Request.ItemPerPageConfig,
                    Offset = 0,
                    SearchType = SearchType.Search,
                    MobileNumber = input.Request.MobileNumberFilter,
                    Status = input.Request.StatusFilter,
                    TransCode = input.Request.TransCodeFilter,
                    TransRef = input.Request.TransRefFilter,
                    ProviderTransCode = input.Request.ProviderTransCode,
                    FromDate = input.Request.FromDate,
                    ServiceCode = input.Request.ServiceCode,
                    ProviderCode = input.Request.ProviderCode,
                    Filter = input.Request.Filter,
                    ToDate = input.Request.ToDate,
                    CategoryCode = input.Request.CategoryCode,
                    ProductCode = input.Request.ProductCode,
                    AgentType = input.Request.AgentTypeFilter,
                    SaleType = input.Request.SaleTypeFilter,
                    ReceiverType = input.Request.ReceiverType,
                    ProviderResponseCode = input.Request.ProviderResponseCode,
                    ReceiverTypeResponse = input.Request.ReceiverTypeResponse,
                    ParentProvider = input.Request.ParentProvider,
                    CategoryCodes = input.Request.CategoryCodes,
                    ServiceCodes = input.Request.ServiceCodes,
                    ProductCodes = input.Request.ProductCodes,
                    ParentCode = input.Request.PartnerCodeFilter,                   
                };
                var rs = AsyncHelper.RunSync(() => _transactionManager.TopupListRequestAsync(request));
                if (rs.Total > 100000)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileLimit", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                var lst = rs.Payload.ConvertTo<List<TopupRequestResponseDto>>();
                if (rs.Total > input.Request.ItemPerPageConfig)
                {
                    var totalPage = Math.Ceiling(decimal.Parse(rs.Total.ToString()) / perpage);
                    _logger.LogInformation($"Get Data total page:{totalPage}");
                    for (var i = 1; i <= totalPage; i++)
                    {
                        if (i == 1)
                            request.Offset = input.Request.ItemPerPageConfig;
                        _logger.LogInformation($"Get Data request:{i}-{request.Offset}");
                        rs = AsyncHelper.RunSync(() => _transactionManager.TopupListRequestAsync(request));
                        _logger.LogInformation($"Get Data return:{i}-{rs.Total}");
                        request.Offset += input.Request.ItemPerPageConfig;
                        lst.AddRange(rs.Payload.ConvertTo<List<TopupRequestResponseDto>>());
                    }
                }

                _logger.LogInformation($"Get Data success");
                var index = 0;
                foreach (var item in lst)
                {
                    //_localizationSource.GetString("Enum_AgentType_" + (int) item.AgentType)
                    index++;
                    item.Index = index;
                    item.StatusName = item.Status == CommonConst.TopupStatus.Init ? "Khởi tạo"
                        : item.Status == CommonConst.TopupStatus.Success ? "Thành công"
                        : item.Status == CommonConst.TopupStatus.Canceled ? "Hủy"
                        : item.Status == CommonConst.TopupStatus.Failed ? "Lỗi"
                        : item.Status == CommonConst.TopupStatus.Paid ? "Đã thanh toán"
                        : item.Status == CommonConst.TopupStatus.InProcessing ? "Đang xử lý"
                        : item.Status == CommonConst.TopupStatus.ProcessTimeout ? "Timeout"
                        : item.Status == CommonConst.TopupStatus.WaitForResult ? "Chưa có kết quả"
                        : item.Status == CommonConst.TopupStatus.TimeOver ? "Chưa có kết quả"
                        : "";
                    item.AgentTypeName = item.AgentType == CommonConst.AgentType.Agent ? "Đại lý"
                        : item.AgentType == CommonConst.AgentType.AgentApi ? "Đại lý API"
                        : item.AgentType == CommonConst.AgentType.AgentCampany ? "Đại lý công ty"
                        : item.AgentType == CommonConst.AgentType.AgentGeneral ? "Đại lý Tổng"
                        : item.AgentType == CommonConst.AgentType.SubAgent ? "Đại lý cấp 1"
                        : item.AgentType == CommonConst.AgentType.WholesaleAgent ? "Đại lý sỉ"
                        : "";
                    item.ServiceName = item.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                        : item.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                        : item.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";
                    var startTime = item.RequestDate ?? item.CreatedTime;
                    if (item.ResponseDate != null)
                        item.TotalTime = (int)(item.ResponseDate.Value - startTime).TotalSeconds;


                    item.ReceiverType = item.ReceiverType == "PREPAID" ? "Trả trước" : item.ReceiverType == "POSTPAID" ? "Trả sau" : "";
                    item.ReceiverTypeResponse = item.ReceiverTypeResponse == "TT" ? "Trả trước" : item.ReceiverTypeResponse == "TS" ? "Trả sau" : "";
                }

                _logger.LogInformation($"Process get Excel file");
                var file = _fileCommon.GetFileExcel(lst, "assets/SampleFiles/ExportTransactionManagement.xlsx",
                    "Export", "QuanLyGD.xlsx");
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.Request.TenantId,
                    Bytes = _fileCommon.CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                //AsyncHelper.RunSync(() => ProcessExportResultAsync(input,file));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
                AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                    input.User,
                    new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                    null,
                    Abp.Notifications.NotificationSeverity.Warn));
            }
        }
    }

    public class ExportTransactionManagementJobArgs
    {
        public GetAllTopupRequestsForExcelInput Request { get; set; }
        public UserAccountInfoDto AccountInfo { get; set; }
        public UserIdentifier User { get; set; }
    }
}
