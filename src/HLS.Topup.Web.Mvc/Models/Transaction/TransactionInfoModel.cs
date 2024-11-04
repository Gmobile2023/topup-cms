using System;
using System.Collections.Generic;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Vendors.Dtos;

namespace HLS.Topup.Web.Models.Transaction
{
    public class TransactionInfoModel : IShouldNormalize
    {
        public string Account { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string c { get; set; }
        public string TransType { get; set; }
        public string TransCode { get; set; }
        public string RequestCode { get; set; }
        public TopupRequestResponseDto TransInfo { get; set; }

        public void Normalize()
        {
            ResolveParameters();
        }

        protected virtual void ResolveParameters()
        {
            try
            {
                if (!string.IsNullOrEmpty(c))
                {
                    var parameters = SimpleStringCipher.Instance.Decrypt(c);
                    var query = HttpUtility.ParseQueryString(parameters);

                    if (query["description"] != null)
                    {
                        Description = query["description"];
                    }

                    if (query["code"] != null)
                    {
                        Code = query["code"];
                    }

                    if (query["account"] != null)
                    {
                        Account = query["account"];
                    }

                    if (query["transCode"] != null)
                    {
                        TransCode = query["transCode"];
                    }

                    if (query["message"] != null)
                    {
                        Message = query["message"];
                    }
                    if (query["amount"] != null)
                    {
                        Amount = decimal.Parse(query["amount"]);
                    }

                    if (query["transType"] != null && query["transType"] != "0")
                    {
                        TransType = query["transType"];
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }

    public class TransactionDetailsInfoModel
    {
        public UserInfoDto Network { get; set; }
        public List<VendorDto> Vendors { get; set; }
        public List<TopupDetailResponseDTO> Items { get; set; }
        public TopupRequestResponseDto TransactionInfo { get; set; }
        public string Transcode { get; set; }

        public string Address { get; set; }
    }

    public class BatchLotInfoModel : IShouldNormalize
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string c { get; set; }
        public string TransCode { get; set; }
        public TopupRequestResponseDto TransInfo { get; set; }

        public void Normalize()
        {
            ResolveBatchLotParameters();
        }

        protected virtual void ResolveBatchLotParameters()
        {
            try
            {
                if (!string.IsNullOrEmpty(c))
                {
                    var parameters = SimpleStringCipher.Instance.Decrypt(c);
                    var query = HttpUtility.ParseQueryString(parameters);


                    if (query["code"] != null)
                    {
                        Code = query["code"];
                    }


                    if (query["transCode"] != null)
                    {
                        TransCode = query["transCode"];
                    }

                    if (query["message"] != null)
                    {
                        Message = query["message"];
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
