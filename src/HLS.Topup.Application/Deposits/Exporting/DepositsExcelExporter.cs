using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Deposits.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Deposits.Exporting
{
    public class DepositsExcelExporter : NpoiExcelExporterBase, IDepositsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepositsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepositForViewDto> deposits)
        {
            string fileName = string.Format("Bao cao nap tien_{0}.xlsx", System.DateTime.Now.ToString("ddMMyyyy"));
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Deposits"));

                    AddHeader(
                        sheet,
                        L("Status"),
                        L("Deposit_Agent"),
                        L("Amount"),
                        L("TransactionType"),
                        L("Deposit_MoneySource"),
                        L("Deposit_RequestCode"),
                        L("TransCode"),
                        L("TransCodeBank"),
                        L("AgentType"),
                        L("Sale_Leader"),
                        L("Sale_Emp"),
                        L("CreationTime"),
                        L("Deposit_DateApproved"),
                        L("Deposit_Approved")
                    );

                    AddObjects(
                        sheet, 2, deposits,
                        _ => L("Enum_DepositStatus_" + (int) _.Deposit.Status),
                        _ => _.AgentName,
                        _ => CellOption.Create(_.Deposit.Amount, "Number"),
                        _ => L("Enum_DepositType_" + (int) _.Deposit.Type),
                        _ => _.Deposit.RecipientInfo != null ? _.Deposit.RecipientInfo : _.BankBankName,
                        _ => _.Deposit.RequestCode,
                        _ => _.Deposit.TransCode,
                        _ => _.Deposit.TransCodeBank,
                        _ => L("Enum_AgentType_" + (int) _.AgentType),
                        _ => _.SaleLeader,
                        _ => _.SaleMan,
                        _ => CellOption.Create(_.Deposit.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => CellOption.Create(_.Deposit.ApprovedDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.UserName2
                    );
                });
        }
    }
}