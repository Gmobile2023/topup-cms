using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Common;
using HLS.Topup.Compare;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;
using static HLS.Topup.Report.ReportComparePartnerExportInfo;

namespace HLS.Topup.Reports
{
    public class ReportsManager : TopupDomainServiceBase, IReportsManager
    {
        private readonly ILogger<ReportsManager> _logger;
        private readonly string _serviceApi;
        private readonly TokenHepper _tokenHepper;

        public ReportsManager(IWebHostEnvironment env, TokenHepper commonManager, ILogger<ReportsManager> logger)
        {
            _tokenHepper = commonManager;
            _logger = logger;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
            //_serviceApi = "http://192.168.33.248:6780";
        }

        public async Task<ApiResponseDto<List<ReportDetailDto>>> ReportDetailGetRequest(
            ReportDetailRequest request)
        {
            _logger.LogInformation($"ReportDetailGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportDetailDto>>>(request);
                _logger.LogInformation($"ReportDetailGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportDetailGetRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportBalanceTotalDto>>> BalanceTotalGetRequest(
            BalanceTotalRequest request)
        {
            _logger.LogInformation($"BalanceTotalGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportBalanceTotalDto>>>(request);
                _logger.LogInformation($"BalanceTotalGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"BalanceTotalGetRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportBalanceTotalDto>>> BalanceGroupTotalGetRequest(
            BalanceGroupTotalRequest request)
        {
            _logger.LogInformation($"BalanceGroupTotalGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportBalanceTotalDto>>>(request);
                _logger.LogInformation($"BalanceGroupTotalGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"BalanceGroupTotalGetRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<SimBalanceHistoriesDto>>> ReportSimTransactionDetailRequest(
            SimBalanceHistoriesRequest request)
        {
            _logger.LogInformation($"ReportSimTransactionDetailRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<SimBalanceHistoriesDto>>>(request);
                _logger.LogInformation($"ReportSimTransactionDetailRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportSimTransactionDetailRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<SimBalanceByDateDto>>> ReportSimTransactionByDateRequest(
            SimBalanceDateRequest request)
        {
            _logger.LogInformation($"ReportSimTransactionByDateRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<SimBalanceByDateDto>>>(request);
                _logger.LogInformation($"ReportSimTransactionByDateRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportSimTransactionByDateRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCardStockHistoriesDto>>> ReportCardStockHistories(
            ReportCardStockHistoriesRequest request)
        {
            _logger.LogInformation($"ReportCardStockHistories request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCardStockHistoriesDto>>>(request);
                _logger.LogInformation($"ReportCardStockHistories return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCardStockHistories error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCardStockImExPortDto>>> ReportCardStockImExPort(
            ReportCardStockImExPortRequest request)
        {
            _logger.LogInformation($"ReportCardStockImExPort request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCardStockImExPortDto>>>(request);
                _logger.LogInformation($"ReportCardStockImExPort return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCardStockImExPort error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCardStockImExPortDto>>> ReportCardStockImExProvider(
            ReportCardStockImExProviderRequest request)
        {
            _logger.LogInformation($"ReportCardStockImExProvider request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCardStockImExPortDto>>>(request);
                _logger.LogInformation($"ReportCardStockImExProvider return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCardStockImExProvider error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCardStockAutoDto>>> ReportCardStockAuto(
            ReportCardStockAutoRequest request)
        {
            _logger.LogInformation($"ReportCardStockAuto request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCardStockAutoDto>>>(request);
                _logger.LogInformation($"ReportCardStockAuto return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCardStockAuto error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCardStockInventoryDto>>> ReportCardStockInventory(
            ReportCardStockInventoryRequest request)
        {
            _logger.LogInformation($"ReportCardStockInventory request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCardStockInventoryDto>>>(request);
                _logger.LogInformation($"ReportCardStockInventory return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCardStockInventory error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportTransDetailDto>>> ReportTransDetailReport(
            ReporttransDetailRequest request)
        {
            _logger.LogInformation($"ReportTransDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportTransDetailDto>>>(request);
                _logger.LogInformation($"ReportTransDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTransDetailReport error: {ex}");
                return null;
            }
        }

        public async Task<ReportItemDetailDto> ReportTransDetailQuery(
            TransDetailByTransCodeRequest request)
        {
            _logger.LogInformation($"ReportTransDetailQuery request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ReportItemDetailDto>(request);
                _logger.LogInformation($"ReportTransDetailQuery return: {rs?.TransCode}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTransDetailQuery error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportItemTotalDay>>> ReportTotalDayReport(
            ReportTotalDayRequest request)
        {
            _logger.LogInformation($"ReportTotalDayReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportItemTotalDay>>>(request);
                _logger.LogInformation($"ReportTotalDayReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTotalDayReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportDebtDetailDto>>> ReportDebtDetailReport(
            ReportDebtDetailRequest request)
        {
            _logger.LogInformation($"ReportDebtDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportDebtDetailDto>>>(request);
                _logger.LogInformation($"ReportDebtDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportDebtDetailReport error: {ex}");
                return null;
            }
        }


        public async Task<ApiResponseDto<List<ReportRefundDetailDto>>> ReportRefundDetailReport(
            ReportRefundDetailRequest request)
        {
            _logger.LogInformation($"ReportRefundDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportRefundDetailDto>>>(request);
                _logger.LogInformation($"ReportRefundDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportRefundDetailReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportTransferDetailDto>>> ReportTransferDetailReport(
            ReportTransferDetailRequest request)
        {
            _logger.LogInformation($"ReportTransferDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportTransferDetailDto>>>(request);
                _logger.LogInformation($"ReportTransferDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTransferDetailReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportServiceDetailDto>>> ReportServiceDetailReport(
            ReportServiceDetailRequest request)
        {
            _logger.LogInformation($"ReportServiceDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(15)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportServiceDetailDto>>>(request);
                _logger.LogInformation($"ReportServiceDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportServiceDetailReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportServiceTotalDto>>> ReportServiceTotalReport(
            ReportServiceTotalRequest request)
        {
            _logger.LogInformation($"ReportServiceTotalReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportServiceTotalDto>>>(request);
                _logger.LogInformation($"ReportServiceTotalReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportServiceTotalReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportServiceProviderDto>>> ReportServiceProviderReport(
            ReportServiceProviderRequest request)
        {
            _logger.LogInformation($"ReportServiceProviderReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportServiceProviderDto>>>(request);
                _logger.LogInformation($"ReportServiceProviderReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportServiceProviderReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportAgentBalanceDto>>> ReportAgentBalanceReport(
            ReportAgentBalanceRequest request)
        {
            _logger.LogInformation($"ReportAgentBalanceReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportAgentBalanceDto>>>(request);
                _logger.LogInformation($"ReportAgentBalanceReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportAgentBalanceReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportRevenueAgentDto>>> ReportRevenueAgentReport(
            ReportRevenueAgentRequest request)
        {
            _logger.LogInformation($"ReportRevenueAgentReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportRevenueAgentDto>>>(request);
                _logger.LogInformation($"ReportRevenueAgentReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportRevenueAgentReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportRevenueCityDto>>> ReportRevenueCityReport(
            ReportRevenueCityRequest request)
        {
            _logger.LogInformation($"ReportRevenueCityReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportRevenueCityDto>>>(request);
                _logger.LogInformation($"ReportRevenueCityReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportRevenueCityReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportTotalSaleAgentDto>>> ReportTotalSaleAgentReport(
            ReportTotalSaleAgentRequest request)
        {
            _logger.LogInformation($"ReportTotalSaleAgentReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportTotalSaleAgentDto>>>(request);
                _logger.LogInformation($"ReportTotalSaleAgentReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTotalSaleAgentReport error: {ex}");
                return null;
            }
        }


        public async Task<ApiResponseDto<List<ReportRevenueActiveDto>>> ReportRevenueActiveReport(
            ReportRevenueActiveRequest request)
        {
            _logger.LogInformation($"ReportRevenueActiveReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportRevenueActiveDto>>>(request);
                _logger.LogInformation($"ReportRevenueActiveReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportRevenueActiveReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportRevenueDashboardDay>>> ReportRevenueDashboardDayList(
            ReportRevenueDashBoardDayRequest request)
        {
            _logger.LogInformation($"ReportRevenueDashboardDayList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportRevenueDashboardDay>>>(request);
                _logger.LogInformation($"ReportRevenueDashboardDayList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportRevenueDashboardDayList error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportRevenueCommistionDashDay>>> ReportAgentGeneralDashDayList(
            ReportAgentGeneralDashRequest request)
        {
            _logger.LogInformation($"ReportAgentGeneralDashDayList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportRevenueCommistionDashDay>>>(request);
                _logger.LogInformation($"ReportAgentGeneralDashDayList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportAgentGeneralDashDayList error: {ex}");
                return null;
            }
        }


        public async Task<GetRevenueInDayDto> GetRevenueInDayRequest(GetRevenueInDayRequest request)
        {
            _logger.LogInformation($"GetRevenueInDayRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<GetRevenueInDayDto>(request);
                //_logger.LogInformation($"GetRevenueInDayRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetRevenueInDayRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportItemTotalDebt>>> ReportTotalDebtReport(
            ReportTotalDebtRequest request)
        {
            _logger.LogInformation($"ReportTotalDebtReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportItemTotalDebt>>>(request);
                //_logger.LogInformation($"ReportTotalDebtReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTotalDebtReport error: {ex}");
                return null;
            }
        }

        public async Task ReportSyncAccountReport(SyncAccountRequest request)
        {
            _logger.LogInformation($"SyncAccountRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.PostAsync<object>(request);
                _logger.LogInformation($"SyncAccountRequest return: {rs}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SyncAccountRequest error: {ex}");
            }
        }

        public async Task InsertSmsMessage(SmsMessageRequest request)
        {
            _logger.LogInformation($"SmsMessageRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.PostAsync<object>(request);
                _logger.LogInformation($"SmsMessageRequest return: {rs}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SmsMessageRequest error: {ex}");
            }
        }

        public async Task<ApiResponseDto<List<CompareDtoReponse>>> ReportCompareList(
            ReportCompareListRequest request)
        {
            _logger.LogInformation($"ReportCompareList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CompareDtoReponse>>>(request);
                _logger.LogInformation($"ReportCompareList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCompareList error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CompareRefunDetailDto>>> ReportCompareDetailReonseList(
            ReportCompareDetailReonseRequest request)
        {
            _logger.LogInformation($"ReportCompareDetailReonseList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CompareRefunDetailDto>>>(request);
                _logger.LogInformation($"ReportCompareDetailReonseList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCompareDetailReonseList error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CompareReponseDto>>> ReportCompareReonseList(
            ReportCompareReonseRequest request)
        {
            _logger.LogInformation($"ReportCompareReonseList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CompareReponseDto>>>(request);
                _logger.LogInformation($"ReportCompareReonseList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCompareReonseList error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CompareRefunDetailDto>>> ReportCompareRefundDetailList(
            ReportCompareRefundDetailRequest request)
        {
            _logger.LogInformation($"ReportCompareRefundDetailList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CompareRefunDetailDto>>>(request);
                _logger.LogInformation($"ReportCompareRefundDetailList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCompareRefundDetailList error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CompareRefunDto>>> ReportCompareRefundList(
            ReportCompareRefundRequest request)
        {
            _logger.LogInformation($"ReportCompareRefundList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CompareRefunDto>>>(request);
                _logger.LogInformation($"ReportCompareRefundList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCompareRefundList error: {ex}");
                return null;
            }
        }

        public async Task<CompareRefunDto> ReportCompareRefundSingle(
            ReportCompareRefundSingleRequest request)
        {
            _logger.LogInformation($"ReportCompareRefundSingle request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ResponseMessages>(request);
                _logger.LogInformation($"ReportCompareRefundSingle return: {rs.ResponseCode} - {rs.Payload}");
                if (rs.ResponseCode == ResponseCodeConst.ResponseCode_Success)
                {
                    return rs.ExtraInfo.FromJson<CompareRefunDto>();
                }

                return null;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCompareRefundSingle error: {ex}");
                return null;
            }
        }

        public async Task<ResponseMessages> CompareProviderDate(
            CompareProviderRequest request)
        {
            _logger.LogInformation($"CompareProviderDate request: {request.Items.Count.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(15)
            };
            try
            {
                var rs = await client.PostAsync<ResponseMessages>(request);
                _logger.LogInformation($"CompareProviderDate return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CompareProviderDate error: {ex}");
                return null;
            }
        }

        public async Task<ResponseMessages> CheckProviderCompareDate(
            ReportCheckCompareRequest request)
        {
            _logger.LogInformation($"CheckProviderCompareDate request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ResponseMessages>(request);
                //_logger.LogInformation($"CheckProviderCompareDate return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CheckProviderCompareDate error: {ex}");
                return null;
            }
        }

        public async Task<ResponseMessages> RefundCompareProvinder(
            CompareRefundCompareRequest request)
        {
            _logger.LogInformation($"RefundSetCompare request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.PostAsync<ResponseMessages>(request);
                _logger.LogInformation($"RefundCompareProvinder return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"RefundCompareProvinder error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportComparePartnerDto>>> ReportComparePartner(
            ReportComparePartnerRequest request)
        {
            _logger.LogInformation($"ReportComparePartner request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(15)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportComparePartnerDto>>>(request);
                _logger.LogInformation($"ReportComparePartner return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportComparePartner error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportBalancePartnerDto>>> ReportBalancePartner(
            ReportComparePartnerRequest request)
        {
            _logger.LogInformation($"ReportBalancePartner request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportBalancePartnerDto>>>(request);
                _logger.LogInformation($"ReportBalancePartner return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportBalancePartner error: {ex}");
                return null;
            }
        }

        public async Task<bool> SendmailReportComparePartner(
            SendMailComparePartnerRequest request)
        {
            _logger.LogInformation($"SendmailReportComparePartner request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.PostAsync<bool>(request);
                _logger.LogInformation($"SendmailReportComparePartner return: {rs}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SendmailReportComparePartner error: {ex}");
                return false;
            }
        }

        public async Task<ApiResponseDto<List<ReportCommissionDetailDto>>> ReportCommissionDetailReport(
            ReportCommissionDetailRequest request)
        {
            _logger.LogInformation($"ReportCommissionDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = new System.TimeSpan(0, 15, 0)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCommissionDetailDto>>>(request);
                _logger.LogInformation($"ReportCommissionDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCommissionDetailReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCommissionTotalDto>>> ReportCommissionTotalReport(
            ReportCommissionTotalRequest request)
        {
            _logger.LogInformation($"ReportCommissionTotalReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = new System.TimeSpan(0, 15, 0)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCommissionTotalDto>>>(request);
                _logger.LogInformation($"ReportCommissionTotalReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCommissionTotalReport error: {ex}");
                return null;
            }
        }


        public async Task<ApiResponseDto<List<ReportCommissionAgentDetailDto>>> ReportCommissionAgentDetailReport(
            ReportCommissionAgentDetailRequest request)
        {
            _logger.LogInformation($"ReportCommissionAgentDetailReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = new System.TimeSpan(0, 15, 0)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCommissionAgentDetailDto>>>(request);
                _logger.LogInformation($"ReportCommissionAgentDetailReport return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCommissionAgentDetailReport error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportCommissionAgentTotalDto>>> ReportCommissionAgentTotalReport(
            ReportCommissionAgentTotalRequest request)
        {
            _logger.LogInformation($"ReportCommissionAgentTotalReport request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = new System.TimeSpan(0, 15, 0)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportCommissionAgentTotalDto>>>(request);
                _logger.LogInformation($"ReportCommissionAgentTotalDto return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportCommissionAgentTotalDto error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<ReportTopupRequestLogDto>>> ReportTopupRequestLogList(ReportTopupRequestLogRequest request)
        {
            _logger.LogInformation($"ReportTopupRequestLogList request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(15)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<ReportTopupRequestLogDto>>>(request);
                _logger.LogInformation($"ReportTopupRequestLogList return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"ReportTopupRequestLogList error: {ex}");
                return null;
            }
        }
    }
}