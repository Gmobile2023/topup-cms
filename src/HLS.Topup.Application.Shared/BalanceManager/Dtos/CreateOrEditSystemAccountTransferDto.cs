using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class CreateOrEditSystemAccountTransferDto : EntityDto<int?>
    {
        [Required]
        [StringLength(SystemAccountTransferConsts.MaxSrcAccountLength,
            MinimumLength = SystemAccountTransferConsts.MinSrcAccountLength)]
        public string SrcAccount { get; set; }
        public string TransCode { get; set; }


        [Required]
        [StringLength(SystemAccountTransferConsts.MaxDesAccountLength,
            MinimumLength = SystemAccountTransferConsts.MinDesAccountLength)]
        public string DesAccount { get; set; }


        public decimal Amount { get; set; }


        [StringLength(SystemAccountTransferConsts.MaxAttachmentsLength,
            MinimumLength = SystemAccountTransferConsts.MinAttachmentsLength)]
        public string Attachments { get; set; }


        [StringLength(SystemAccountTransferConsts.MaxDescriptionLength,
            MinimumLength = SystemAccountTransferConsts.MinDescriptionLength)]
        public string Description { get; set; }


        public CommonConst.SystemTransferStatus Status { get; set; }
    }
}
