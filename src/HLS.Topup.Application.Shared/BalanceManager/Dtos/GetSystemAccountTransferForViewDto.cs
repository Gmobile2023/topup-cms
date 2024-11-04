using System;
using HLS.Topup.Dtos.Common;
namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetSystemAccountTransferForViewDto : AuditCommonDto
    {
        public SystemAccountTransferDto SystemAccountTransfer { get; set; }
    }
}
