using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetSystemAccountTransferForEditOutput
    {
		public CreateOrEditSystemAccountTransferDto SystemAccountTransfer { get; set; }


    }
}