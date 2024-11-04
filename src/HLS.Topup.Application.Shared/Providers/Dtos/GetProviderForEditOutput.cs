using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Providers.Dtos
{
    public class GetProviderForEditOutput
    {
		public CreateOrEditProviderDto Provider { get; set; }
        public ProviderUpdateInfo ProviderUpdate { get; set; }       
    }
}
