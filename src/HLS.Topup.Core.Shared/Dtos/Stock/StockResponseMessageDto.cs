using System;
using System.Collections.Generic;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Stock
{
    public class CardCreateResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BatchCode { get; set; }
        public string Description { get; set; }
        public string StockType { get; set; }
        public byte Status { get; set; }
    }


    public class ApiResponseDto<T> : ResponseMessages
    {
        public T Payload { get; set; }

        public T SumData { get; set; }
    }
}