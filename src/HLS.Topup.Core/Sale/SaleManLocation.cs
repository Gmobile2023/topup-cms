using HLS.Topup.Authorization.Users;
using HLS.Topup.Address;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Sale
{
    [Table("SaleManLocations")]
    public class SaleManLocation : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public District DistrictFk { get; set; }

        public virtual int? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual int WardId { get; set; }

        [ForeignKey("WardId")]
        public Ward WardFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
    }
}
