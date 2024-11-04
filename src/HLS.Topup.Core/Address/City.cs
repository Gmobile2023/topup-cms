using HLS.Topup.Common;
using HLS.Topup.Address;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Address
{
    [Table("Cities")]
    public class City : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(CityConsts.MaxCityCodeLength, MinimumLength = CityConsts.MinCityCodeLength)]
        public virtual string CityCode { get; set; }

        [Required]
        [StringLength(CityConsts.MaxCityNameLength, MinimumLength = CityConsts.MinCityNameLength)]
        public virtual string CityName { get; set; }

        public virtual CommonConst.CityStatus Status { get; set; }


        public virtual int? CountryId { get; set; }

        [ForeignKey("CountryId")] public Country CountryFk { get; set; }
    }
}
