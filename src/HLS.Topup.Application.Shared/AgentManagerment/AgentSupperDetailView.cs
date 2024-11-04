using HLS.Topup.Common;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Dtos.Partner;

namespace HLS.Topup.AgentManagerment
{
    public class AgentSupperDetailView
    {

        public long? UserId { get; set; }
        [CanBeNull] public string AccountCode { get; set; }

        [CanBeNull] public string PhoneNumber { get; set; }

        [CanBeNull] public string Surname { get; set; }

        [CanBeNull] public string Name { get; set; }

        [CanBeNull] public string FullName { get; set; }

        public CommonConst.AgentType? AgentType { get; set; }

        public DateTime? CreationTime { get; set; }

        [CanBeNull] public string CreatorName { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        [CanBeNull] public string Address { get; set; }

        [CanBeNull] public string AddressView { get; set; }

        [CanBeNull] public string AgentName { get; set; }

        public int? Province { get; set; }

        public int? District { get; set; }

        public int? Ward { get; set; }

        public string Contract { get; set; }

        public DateTime? ContractRegister { get; set; }

        public int CrossCheckPeriod { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string EmailCompare { get; set; }
        public string FolderFtp { get; set; }
        public string EmailTech { get; set; }

        public string Content { get; set; }

        #region  1.Giám đốc

        public string ManagerFullName { get; set; }

        public string ManagerPhone { get; set; }

        public string ManagerEmail { get; set; }

        #endregion

        #region 2.Kỹ thuật

        public string TechnicalFullName { get; set; }


        public string TechnicalPhone { get; set; }


        public string TechnicalEmail { get; set; }
        public string ChatId { get; set; }
        public int LimitChannel { get; set; }
        public bool IsApplySlowTrans { get; set; }
        public bool IsCheckReceiverType { get; set; }
        public bool IsNoneDiscount { get; set; }
        public bool IsCheckPhone { get; set; }

        #endregion

        #region 3.Đối soát
        public string CompareFullName { get; set; }

        public string ComparePhone { get; set; }

        public string CompareEmail { get; set; }

        #endregion

        #region 4.Kế toán
        public string AccountancyFullName { get; set; }

        public string AccountancyPhone { get; set; }

        public string AccountancyEmail { get; set; }

        #endregion
        public PartnerConfigTransDto PartnerConfig { get; set; }
        public IdentityServerStorageInputDto IdentityServerStorage { get; set; }
    }
}
