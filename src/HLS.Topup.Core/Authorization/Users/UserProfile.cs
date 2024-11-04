using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Address;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static HLS.Topup.Common.CommonConst;

namespace HLS.Topup.Authorization.Users
{
    [Table("AbpUserProfile")]
    public class UserProfile : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual int? CityId { get; set; }
        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual int? DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public District DistrictFk { get; set; }

        public virtual int? WardId { get; set; }

        [ForeignKey("WardId")]
        public Ward WardFk { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }

        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(500)]
        public string AddressDetails { get; set; }

        [StringLength(255)]
        public string FrontPhoto { get; set; }
        [StringLength(255)]
        public string BackSitePhoto { get; set; }
        [StringLength(255)]
        public string IdIdentity { get; set; }

        public IdType IdType { get; set; }

        [StringLength(255)]
        public string Desscription { get; set; }
        public string ExtraInfo { get; set; }
        public DateTime? IdentityIdExpireDate { get; set; }
        [StringLength(50)]
        public string ChatId { get; set; }
        public int LimitChannel { get; set; }
        public bool? IsApplySlowTrans { get; set; }


        public DateTime? SigDate { get; set; } //Ngày ký HĐ
        public int PeriodCheck { get; set; } //Kỳ đối soát
        [StringLength(255)]
        public string ContractNumber { get; set; } //Số HĐ
        [StringLength(255)]
        public string TaxCode { get; set; } //Mã số thuế
        [StringLength(500)]
        public string EmailReceives { get; set; } //Email nhận đối soát
        [StringLength(500)]
        public string ContactInfos { get; set; }//Danh sách contact

        [StringLength(500)]
        public string EmailTech { get; set; }

        public MethodReceivePassFile? MethodReceivePassFile { get; set; }

        [StringLength(255)]
        public string ValueReceivePassFile { get; set; }
        [StringLength(50)]
        public string FolderFtp { get; set; }
    }
}
