using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Categories
{
    public class CategoryModel
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int Order { get; set; }
        public CommonConst.CategoryStatus Status { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string ServiceCode { get; set; }
    }
}