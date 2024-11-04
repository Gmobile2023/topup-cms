using System.ComponentModel.DataAnnotations;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Sale
{
    public class CreateOrUpdateSaleDto
    {
        public long? Id { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        //[Required]
        public string UserName { get; set; }

        /// <summary>
        /// Họ
        /// </summary>
        public string SurName { get; set; }

        //public string SaleName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string EmailAddress { get; set; }

        //public int CityId { get; set; }
        //public List<int?> CityIds { get; set; }
        //public List<int?> DistrictIds { get; set; }
        //public List<int> WardIds { get; set; }

        public CommonConst.SystemAccountType SaleType { get; set; }
        //public CommonConst.SaleManStatus Status { get; set; }

        /// <summary>
        /// UserId của sale lead nếu có
        /// </summary>
        public long? SaleLeadUserId { get; set; }

        public string Description { get; set; }
        public int? TenantId { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string SaleLeadName { get; set; }
    }
}
