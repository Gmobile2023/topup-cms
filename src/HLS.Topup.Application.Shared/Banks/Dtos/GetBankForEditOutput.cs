using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Banks.Dtos
{
    public class GetBankForEditOutput
    {
		public CreateOrEditBankDto Bank { get; set; }


    }
}