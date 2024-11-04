using System;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using JetBrains.Annotations;

namespace HLS.Topup.AgentManagerment
{
    public class AgentDetailView : EntityDto
    {
        public long? UserId { get; set; }
        [CanBeNull] public string AccountCode { get; set; }

        [CanBeNull] public string PhoneNumber { get; set; }
        
        [CanBeNull] public string Surname { get; set; }
        
        [CanBeNull] public string Name { get; set; }

        [CanBeNull] public string FullName { get; set; }

        public CommonConst.AgentType? AgentType { get; set; }

        [CanBeNull] public string ManagerName { get; set; }

        [CanBeNull] public string SaleLeadName { get; set; }

        public DateTime? CreationTime { get; set; }
        
        [CanBeNull] public string CreatorName { get; set; }

        public bool? Status { get; set; }

        [CanBeNull] public string Address { get; set; }
        
        [CanBeNull] public string AddressView { get; set; }

        [CanBeNull] public string Exhibit { get; set; }

        public bool? IsMapSale { get; set; }
        
        public DateTime? AssignTime { get; set; }
        
        public DateTime? VerifyTime { get; set; }
        
        [CanBeNull] public string AgentName { get; set; }
        
        public CommonConst.IdType IdIdentityType { get; set; }
        
        public DateTime? IdIdentityExpDate { get; set; }
        
        [CanBeNull] public string IdIdentityFront { get; set; }
        
        [CanBeNull] public string IdIdentityBack { get; set; }
        
        public int? Province { get; set; }
        
        public int? District { get; set; }
        
        public int? Ward { get; set; }
        
        public DateTime? IdentityIdExpireDate { get; set; }
        
        public string FrontPhoto { get; set; }
        public string BackSitePhoto { get; set; }
        public string IdIdentity { get; set; }
        public CommonConst.IdType IdType { get; set; }
        public DateTime? SigDate { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? ContractRegister { get; set; }
        public CommonConst.MethodReceivePassFile? MethodReceivePassFile { get; set; }
        public string ValueReceivePassFile { get; set; }
    }
}