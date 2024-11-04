using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CardStockDto : EntityDto
    {
		public string StockCode { get; set; }
		public string KeyCode { get; set; }

		public int CardValue { get; set; }

		public int Inventory { get; set; }

		public int InventoryLimit { get; set; }

		public int MinimumInventoryLimit { get; set; }

		public CommonConst.CardStockStatus Status { get; set; }

		public string Description { get; set; }
		public string ProductName { get; set; }
		public string ProductCode { get; set; }
		// dịch vu
		public string ServiceName { get; set; }
		public string ServiceCode { get; set; }
		// loai sp
		public string CategoryName { get; set; }
		public string CategoryCode { get; set; }

    }
}
