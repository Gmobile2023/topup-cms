using System;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Users
{
    public class UserProfileDto : AgentProfile
    {
        public int? Id { get; set; }
        public int? TenantId { get; set; }
        public virtual long? ParentId { get; set; }
        public string ParentAccount { get; set; }
        public string ParentName { get; set; }
        public virtual int? CityId { get; set; }
        public virtual int? DistrictId { get; set; }
        public virtual int? WardId { get; set; }
        public string Address { get; set; }
        public string FrontPhoto { get; set; }
        public string BackSitePhoto { get; set; }
        public string IdIdentity { get; set; }
        public CommonConst.IdType IdType { get; set; }
        public string Desscription { get; set; }
        public long UserId { get; set; }
        public string AgentName { get; set; }
        public string ExtraInfo { get; set; }
        public DateTime? IdentityIdExpireDate { get; set; }
        public string ChatId { get; set; }
        public byte LimitChannel { get; set; }
        public bool IsApplySlowTrans { get; set; }

        public DateTime? SigDate { get; set; } //Ngày ký HĐ.
        public int PeriodCheck { get; set; } //Kỳ đối soát
        public string ContractNumber { get; set; } //Số HĐ
        public string TaxCode { get; set; } //Mã số thuế
        public string EmailReceives { get; set; } //Email nhận đối soát
        public string ContactInfos { get; set; }//Danh sách contact
        public string EmailTech { get; set; }//Danh sách contact
        public string FolderFtp { get; set; }

        public string AccountCode { get; set; }
        public string PhoneNumber { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public CommonConst.AgentType? AgentType { get; set; }

        public DateTime? CreationTime { get; set; }

        public string CreatorName { get; set; }

        public bool IsActive { get; set; }
        public string Password { get; set; }
        public bool IsUserStaff { get; set; }
        
        public DateTime? ContractRegister { get; set; }
        
        public CommonConst.MethodReceivePassFile? MethodReceivePassFile { get; set; }
        
        public string ValueReceivePassFile { get; set; }
    }

    public class AgentProfile
    {
        public string Contract { get; set; }

        public DateTime? ContractRegister { get; set; }

        public int CrossCheckPeriod { get; set; }
    }
}
