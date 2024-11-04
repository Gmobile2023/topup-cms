using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.Editions.Dto;

namespace HLS.Topup.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}