using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.Editions.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}