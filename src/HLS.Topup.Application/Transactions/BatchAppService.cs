using HLS.Topup.Dto;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.RequestDtos;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Transactions.Dtos;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HLS.Topup.Transactions
{
    public partial class TransactionsAppService
    {
        public async Task<PagedResultDtoReport<BatchItemDto>> GetBatchLotList(
            BatchListGetInput input)
        {
            try
            {
                var account = UserManager.GetAccountInfo();

                var request = input.ConvertTo<BatchListGetRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.IsStaff = account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.Staff ||
                                  account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.StaffApi;
                request.AccountCode = account.UserInfo.AccountCode;

                var rs = await _transactionManager.GetBatchLotListRequest(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "1")
                    return new PagedResultDtoReport<BatchItemDto>(0, new BatchItemDto(),
                        new List<BatchItemDto>());

                var sumList = rs.SumData.ConvertTo<List<BatchItemDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new BatchItemDto();
                var lst = rs.Payload.ConvertTo<List<BatchItemDto>>();
                foreach (var x in lst)
                {
                    if (x.Status == Common.CommonConst.BatchLotRequestStatus.Init)
                        x.StatusName = "Khởi tạo";
                    else if (x.Status == Common.CommonConst.BatchLotRequestStatus.Process)
                        x.StatusName = "Đang xử lý";
                    else if (x.Status == Common.CommonConst.BatchLotRequestStatus.Stop)
                        x.StatusName = "Đã dừng";
                    else if (x.Status == Common.CommonConst.BatchLotRequestStatus.Completed)
                        x.StatusName = "Hoàn tất";

                    if (x.BatchType == "PINCODE" || x.BatchType == "PINC_CODE")
                    {
                        x.BatchName = "Mua mã thẻ";
                        x.Link = "/BatchTopup/PinDetail?batchCode=" + x.BatchCode;
                    }
                    else if (x.BatchType == "TOPUP")
                    {
                        x.BatchName = "Nạp tiền điện thoại";
                        x.Link = "/BatchTopup/TopupDetail?batchCode=" + x.BatchCode;
                    }
                    else if (x.BatchType == "PAYBILL")
                    {
                        x.BatchName = "Thanh toán hóa đơn";
                        x.Link = "/BatchTopup/BillDetail?batchCode=" + x.BatchCode;
                    }
                }


                return new PagedResultDtoReport<BatchItemDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                // _logger.LogError($"GetBatchLotList error: {e}");
                return new PagedResultDtoReport<BatchItemDto>(
                    0,
                    new BatchItemDto(),
                    new List<BatchItemDto>());
            }
        }

        public async Task<BatchItemDto> GetBatchLotSingle(
            BatchSingleInput input)
        {
            try
            {
                var account = UserManager.GetAccountInfo();
                var request = input.ConvertTo<BatchSingleGetRequest>();
                request.IsStaff = account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.Staff ||
                                  account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.StaffApi;
                request.AccountCode = account.UserInfo.AccountCode;
                var x = await _transactionManager.GetBatchSingleRequest(request);


                if (x.Status == Common.CommonConst.BatchLotRequestStatus.Init)
                    x.StatusName = "Khởi tạo";
                else if (x.Status == Common.CommonConst.BatchLotRequestStatus.Process)
                    x.StatusName = "Đang xử lý";
                else if (x.Status == Common.CommonConst.BatchLotRequestStatus.Stop)
                    x.StatusName = "Đã dừng";
                else if (x.Status == Common.CommonConst.BatchLotRequestStatus.Completed)
                    x.StatusName = "Hoàn tất";

                if (x.BatchType == "PINCODE" || x.BatchType == "PINC_CODE")
                {
                    x.BatchName = "Mua mã thẻ";
                }
                else if (x.BatchType == "TOPUP")
                {
                    x.BatchName = "Nạp tiền điện thoại";
                }
                else if (x.BatchType == "PAYBILL")
                {
                    x.BatchName = "Thanh toán hóa đơn";
                }

                return x;
            }
            catch (Exception e)
            {
                // _logger.LogError($"GetBatchLotList error: {e}");
                return null;
            }
        }

        public async Task<PagedResultDtoReport<BatchDetailDto>> GetBatchLotDetailList(
            BatchDetailGetInput input)
        {
            try
            {
                var account = UserManager.GetAccountInfo();
                var request = input.ConvertTo<BatchDetailGetRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.IsStaff = false; // account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.Staff;
                request.AccountCode = account.NetworkInfo.AccountCode;

                var rs = await _transactionManager.GetBatchLotDetaiListLRequest(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "1")
                    return new PagedResultDtoReport<BatchDetailDto>(0, new BatchDetailDto(),
                        new List<BatchDetailDto>());


                var sumList = rs.SumData.ConvertTo<List<BatchDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new BatchDetailDto();
                var lst = rs.Payload.ConvertTo<List<BatchDetailDto>>();

                lst.ForEach(x =>
                {
                    if (x.BatchStatus == Common.CommonConst.BatchLotRequestStatus.Completed)
                        x.BatchName = "Hoàn tất";
                    else if (x.BatchStatus == Common.CommonConst.BatchLotRequestStatus.Stop)
                        x.BatchName = "Dừng";
                    else if (x.BatchStatus == Common.CommonConst.BatchLotRequestStatus.Process)
                        x.BatchName = "Đang xử lý";
                    else if (x.BatchStatus == Common.CommonConst.BatchLotRequestStatus.Init)
                        x.BatchName = "Khởi tạo";

                    if (x.Status == Common.CommonConst.SaleRequestStatus.Init)
                        x.StatusName = "";
                    else if (x.Status == Common.CommonConst.SaleRequestStatus.Success)
                        x.StatusName = "Thành công";
                    else if (x.Status == Common.CommonConst.SaleRequestStatus.Failed ||
                             x.Status == Common.CommonConst.SaleRequestStatus.Canceled)
                        x.StatusName = "Lỗi";
                    else
                        x.StatusName = "Chưa có kết quả";
                });
                return new PagedResultDtoReport<BatchDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                // _logger.LogError($"GetBatchLotList error: {e}");
                return new PagedResultDtoReport<BatchDetailDto>(
                    0,
                    new BatchDetailDto(),
                    new List<BatchDetailDto>());
            }
        }

        public async Task<ResponseMessages> BatchLotStopRequest(
            BatchLotStopInput input)
        {
            try
            {
                var account = UserManager.GetAccountInfo();
                var request = input.ConvertTo<BatchLotStopRequest>();
                request.IsStaff = account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.Staff ||
                                  account.UserInfo.AccountType == Common.CommonConst.SystemAccountType.StaffApi;
                request.AccountCode = account.UserInfo.AccountCode;
                var rs = await _transactionManager.StopBatchLotAsync(request);
                return rs;
            }
            catch (Exception e)
            {
                return new ResponseMessages()
                {
                    ResponseCode = "0",
                    ResponseMessage = "Lỗi"
                };
            }
        }


        public async Task<FileDto> GetBatchLotExportToFile(
           BatchListGetInput input)
        {
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var reponse = await GetBatchLotList(input);
            var list = reponse.Items.ConvertTo<List<BatchItemDto>>();
            return _transactionsExcelExporter.BatchLotExportToFile(list);
        }

        public async Task<FileDto> GetBatchLotTopupDetailExportToFile(
          BatchDetailGetInput input)
        {
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var reponse = await GetBatchLotDetailList(input);
            var list = reponse.Items.ConvertTo<List<BatchDetailDto>>();
            return _transactionsExcelExporter.BatchLotTopupDetailExportToFile(list);
        }

        public async Task<FileDto> GetBatchLotBillDetailExportToFile(
          BatchDetailGetInput input)
        {
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var reponse = await GetBatchLotDetailList(input);
            var list = reponse.Items.ConvertTo<List<BatchDetailDto>>();
            return _transactionsExcelExporter.BatchLotBillDetailExportToFile(list);
        }

        public async Task<FileDto> GetBatchLotPinCodeDetailExportToFile(
        BatchDetailGetInput input)
        {
            input.SkipCount = 0;
            input.MaxResultCount = int.MaxValue;
            var reponse = await GetBatchLotDetailList(input);
            var list = reponse.Items.ConvertTo<List<BatchDetailDto>>();
            return _transactionsExcelExporter.BatchLotPinCodeDetailExportToFile(list);
        }
    }
}
