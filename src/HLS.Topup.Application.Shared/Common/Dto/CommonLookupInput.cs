using System.Collections.Generic;

namespace HLS.Topup.Common.Dto
{
    public class CategorySearchInput
    {
        public string CategoryCode { get; set; }
        public string ServiceCode { get; set; }
        public List<string> ServiceCodes { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategorySearchTwoInput
    {
        public int CategoryId { get; set; }
        public int ServiceId { get; set; }
        public List<int> ServiceIds { get; set; }
        public bool IsActive { get; set; }
    }
    public class ProductSearchInput
    {
        public string CategoryCode { get; set; }
        public int? CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ProductInfoInput
    {
        public string ProductCode { get; set; }
    }
    public class ServiceInfoInput
    {
        public string ServiceCode { get; set; }
    }
    public class ProviderInfoInput
    {
        public string ProviderCode { get; set; }
    }
    public class VendorTransInfoInput
    {
        public string Code { get; set; }
    }

    public class GetProductDiscountInput
    {
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
    }
}
