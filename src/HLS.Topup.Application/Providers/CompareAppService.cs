using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization.Sources;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Compare;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications;
using HLS.Topup.Providers.Dto;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Providers.Exporting;
using HLS.Topup.Reports;
using HLS.Topup.RequestDtos;
using HLS.Topup.Storage;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Transactions;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Hangfire;

namespace HLS.Topup.Providers
{
    [AbpAuthorize]
    public class CompareAppService : TopupAppServiceBase, ICompareAppService
    {
        private readonly ILogger<CompareAppService> _logger;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<User, long> _userRepository;
        private readonly IReportsManager _reportsManager;
        private readonly ITransactionManager _transactionManager;
        private readonly INotificationSender _appNotifierSender;
        private readonly IProvidersExcelExporter _excelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CompareAppService(IAppNotifier appNotifier,
            IRepository<User, long> userRepository,
            IReportsManager reportsManager,
            ITransactionManager transactionManager,
            INotificationSender appNotifierSender,
            IProvidersExcelExporter excelExporter,
            ILogger<CompareAppService> logger, IUnitOfWorkManager unitOfWorkManager)
        {
            _appNotifier = appNotifier;
            _userRepository = userRepository;
            _reportsManager = reportsManager;
            _transactionManager = transactionManager;
            _appNotifierSender = appNotifierSender;
            _excelExporter = excelExporter;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// Hàm lấy dữ liệu màn hình đối soát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDtoReport<CompareDtoReponse>> GetCompareServiceTotalList(
          GetCompareInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCompareListRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var rs = await _reportsManager.ReportCompareList(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<CompareDtoReponse>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new CompareDtoReponse();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<CompareDtoReponse>(0, new CompareDtoReponse(),
                        new List<CompareDtoReponse>());

                var lst = rs.Payload.ConvertTo<List<CompareDtoReponse>>();
                return new PagedResultDtoReport<CompareDtoReponse>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareServiceTotalList error: {e}");
                return new PagedResultDtoReport<CompareDtoReponse>(
                    0,
                    new CompareDtoReponse(),
                    new List<CompareDtoReponse>());
            }
        }

        /// <summary>
        /// Hàm trả về danh sách kết quả đối soát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDtoReport<CompareReponseDto>> GetCompareReponseList(
          GetCompareReponseInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCompareReonseRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var rs = await _reportsManager.ReportCompareReonseList(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<CompareReponseDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new CompareReponseDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<CompareReponseDto>(0, new CompareReponseDto(),
                        new List<CompareReponseDto>());

                var lst = rs.Payload.ConvertTo<List<CompareReponseDto>>();
                return new PagedResultDtoReport<CompareReponseDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareReponseList error: {e}");
                return new PagedResultDtoReport<CompareReponseDto>(
                    0,
                    new CompareReponseDto(),
                    new List<CompareReponseDto>());
            }
        }


        /// <summary>
        /// Hàm trả về chi tiết danh sách kết quả đối soát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDtoReport<CompareReponseDetailDto>> GetCompareReponseDetailList(
          GetCompareReponseDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCompareDetailReonseRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var rs = await _reportsManager.ReportCompareDetailReonseList(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<CompareReponseDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new CompareReponseDetailDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<CompareReponseDetailDto>(0, new CompareReponseDetailDto(),
                        new List<CompareReponseDetailDto>());

                var lst = rs.Payload.ConvertTo<List<CompareReponseDetailDto>>();
                return new PagedResultDtoReport<CompareReponseDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareReponseDetailList error: {e}");
                return new PagedResultDtoReport<CompareReponseDetailDto>(
                    0,
                    new CompareReponseDetailDto(),
                    new List<CompareReponseDetailDto>());
            }
        }


        /// <summary>
        /// Hàm trả về chi tiết danh sách hoàn tiền đối soát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDtoReport<CompareRefunDetailDto>> GetCompareRefundDetailList(
          GetCompareRefundDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCompareRefundDetailRequest>();
                request.Offset = input.SkipCount;
                request.Limit = int.MaxValue;//input.MaxResultCount;
                var rs = await _reportsManager.ReportCompareRefundDetailList(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<CompareRefunDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new CompareRefunDetailDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<CompareRefunDetailDto>(0, new CompareRefunDetailDto(),
                        new List<CompareRefunDetailDto>());

                var lst = rs.Payload.ConvertTo<List<CompareRefunDetailDto>>();

                return new PagedResultDtoReport<CompareRefunDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareRefundDetailList error: {e}");
                return new PagedResultDtoReport<CompareRefunDetailDto>(
                    0,
                    new CompareRefunDetailDto(),
                    new List<CompareRefunDetailDto>());
            }
        }

        /// <summary>
        /// Hàm trả bảng hoàn tiền đối soát
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDtoReport<CompareRefunDto>> GetCompareRefundList(
          GetCompareRefundInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCompareRefundRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var rs = await _reportsManager.ReportCompareRefundList(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<CompareRefunDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new CompareRefunDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<CompareRefunDto>(0, new CompareRefunDto(),
                        new List<CompareRefunDto>());


                var lst = rs.Payload.ConvertTo<List<CompareRefunDto>>();
                foreach (var item in lst)
                    item.TransDateSoft = item.TransDate.ToString("yyyyMMdd");

                return new PagedResultDtoReport<CompareRefunDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareRefundList error: {e}");
                return new PagedResultDtoReport<CompareRefunDto>(
                    0,
                    new CompareRefunDto(),
                    new List<CompareRefunDto>());
            }
        }

        public async Task<CompareRefunDto> GetCompareRefundSingle(
         GetCompareRefundSingleInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCompareRefundSingleRequest>();
                request.Offset = 0;
                request.Limit = 2;
                var rs = await _reportsManager.ReportCompareRefundSingle(request);
                return rs;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareRefundSingle error: {e}");
                return null;
            }
        }

        public async Task<ResponseMessages> CheckCompareProviderDate(
               ReportCheckCompareInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportCheckCompareRequest>();
                var rs = await _reportsManager.CheckProviderCompareDate(request);
                return rs;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCompareRefundSingle error: {e}");
                return null;
            }
        }

        public async Task RefundAmountCompare(RefundCompareAmountInput refunDto)
        {


            //await _reportsManager.RefundCompareProvinder(new CompareRefundCompareRequest()
            //{
            //    Items = new List<string>(),
            //    ProviderCode = refunDto.ProviderCode,
            //    TransDate = refunDto.TransDate,
            //});

            BackgroundJob.Enqueue<IReportsManager>((x) => x.RefundCompareProvinder(new CompareRefundCompareRequest()
            {
                Items = new List<string>(),
                ProviderCode = refunDto.ProviderCode,
                KeyCode = refunDto.KeyCode,
            }));

        }

        public async Task RefundAmoutSelectCompare(RefundCompareSelectInput refunDto)
        {
            if (refunDto != null && refunDto.TransCodes.Count > 0)
            {
                //await _reportsManager.RefundCompareProvinder(new CompareRefundCompareRequest()
                //{
                //    Items = refunDto.TransCodes,
                //    ProviderCode = string.Empty,
                //    TransDate = string.Empty,
                //});

                BackgroundJob.Enqueue<IReportsManager>((x) => x.RefundCompareProvinder(new CompareRefundCompareRequest()
                {
                    Items = refunDto.TransCodes,
                    ProviderCode = string.Empty,
                    KeyCode = refunDto.KeyCode,
                }));
            }
            else
            {
                throw new UserFriendlyException("Quý khách chưa chọn giao dịch để hoàn");

            }
        }


        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task ImportCompareJob(CompareProviderRequest data)
        {

            try
            {
                using var uow = _unitOfWorkManager.Begin();
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    await uow.CompleteAsync();
                    var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);
                    data.AccountCompare = user.UserName;

                    // var rs = await _reportsManager.CompareProviderDate(data);
                    BackgroundJob.Enqueue<IReportsManager>((x) => x.CompareProviderDate(data));
                }
            }
            catch (Exception e)
            {
                //await _appNotifier.SendMessageAsync(user, e.Message, Abp.Notifications.NotificationSeverity.Error);
                return;
            }
        }

        public async Task<FileDto> GetCompareServiceTotalListToExcel(GetCompareInput input)
        {
            var request = input.ConvertTo<ReportCompareListRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            var rs = await _reportsManager.ReportCompareList(request);
            var lst = rs.Payload.ConvertTo<List<CompareDtoReponse>>();

            return _excelExporter.ExportCompareToFile(lst);
        }

        public async Task<FileDto> GetCompareReponseDetailListToExcel(GetCompareReponseDetailInput input)
        {
            var request = input.ConvertTo<ReportCompareDetailReonseRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            var rs = await _reportsManager.ReportCompareDetailReonseList(request);
            var lst = rs.Payload.ConvertTo<List<CompareReponseDetailDto>>();

            return _excelExporter.ExportCompareDetailToFile(lst);
        }

        public async Task<FileDto> GetCompareRefundListToExcel(GetCompareRefundInput input)
        {
            var request = input.ConvertTo<ReportCompareRefundRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            var rs = await _reportsManager.ReportCompareRefundList(request);
            var lst = rs.Payload.ConvertTo<List<CompareRefunDto>>();

            return _excelExporter.ExportCompareRefundToFile(lst);
        }

        public async Task<FileDto> GetCompareRefundDetailListToExcel(GetCompareRefundDetailInput input)
        {
            var request = input.ConvertTo<ReportCompareRefundDetailRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            var rs = await _reportsManager.ReportCompareRefundDetailList(request);

            var lst = rs.Payload.ConvertTo<List<CompareRefunDetailDto>>();

            return _excelExporter.ExportCompareRefundDetailToFile(lst);

        }
    }
}
