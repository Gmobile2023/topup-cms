using System;
using System.Collections.Generic;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Dtos.Partner;

namespace HLS.Topup.Dtos.Accounts
{
    public class CreateOrUpdateAgentPartnerInput
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string EmailTech { get; set; }
        public string FolderFtp { get; set; }
        
        public string Address { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public byte? Gender { get; set; }
        public DateTime? DoB { get; set; }
        public bool IsActive { get; set; }//Trạng thái user chỉ có khóa hoặc hoạt động
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string ChatId { get; set; }
        public byte LimitChannel { get; set; }
        public bool IsApplySlowTrans { get; set; }
        public DateTime? SigDate { get; set; } //Ngày ký HĐ
        public int PeriodCheck { get; set; } //Kỳ đối soát
        public string ContractNumber { get; set; } //Số HĐ
        public string TaxCode { get; set; } //Mã số thuế
        public string EmailReceives { get; set; } //Email nhận đối soát
        public List<AgentPartnerContactInfo> ContactInfos { get; set; }//Danh sách contact
        public PartnerConfigTransDto PartnerConfig { get; set; }
        public IdentityServerStorageInputDto IdentityServerStorage { get; set; }
    }

    public class AgentPartnerContactInfo
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public CommonConst.AgentPartnerContactInfoType ContactType { get; set; }
    }
}
