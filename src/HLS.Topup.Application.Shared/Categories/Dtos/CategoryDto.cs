using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Categories.Dtos
{
    public class CategoryDto : EntityDto
    {
        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public int Order { get; set; }

        public CommonConst.CategoryStatus Status { get; set; }

        public CommonConst.CategoryType Type { get; set; }
        public string Image { get; set; }

        public int? ParentCategoryId { get; set; }

        public int? ServiceId { get; set; }
    }
}