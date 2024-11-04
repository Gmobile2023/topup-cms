using HLS.Topup.Common;
using HLS.Topup.Address;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Address
{
    [Table("Districts")]
    public class District : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(DistrictConsts.MaxDistrictCodeLength, MinimumLength = DistrictConsts.MinDistrictCodeLength)]
        public virtual string DistrictCode { get; set; }

        [Required]
        [StringLength(DistrictConsts.MaxDistrictNameLength, MinimumLength = DistrictConsts.MinDistrictNameLength)]
        public virtual string DistrictName { get; set; }

        public virtual CommonConst.DistrictStatus Status { get; set; }


        public virtual int CityId { get; set; }

        [ForeignKey("CityId")] public City CityFk { get; set; }
    }
}
