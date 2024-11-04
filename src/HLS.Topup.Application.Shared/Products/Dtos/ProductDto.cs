using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Products.Dtos
{
    public class ProductDto : EntityDto
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public string Image { get; set; }

        public int Order { get; set; }

        public decimal? ProductValue { get; set; }

        public CommonConst.ProductType ProductType { get; set; }

        public CommonConst.ProductStatus Status { get; set; }

        public string Unit { get; set; }

        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        public string CustomerSupportNote { get; set; }

        public string UserManualNote { get; set; }
        public string Description { get; set; }
    }

    public class ProductInfoDto : ProductDto
    {
        public string CategoryName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public int ServiceId { get; set; }
    }

    public class ProductInformationDto
    {
        public string ProductCode { get; set; }
        public decimal? ProductValue { get; set; }
        public int Status { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}