﻿using Abp.Application.Services.Dto;

namespace HLS.Topup.Vendors.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}