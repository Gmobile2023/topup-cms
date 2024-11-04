using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Notifications
{
	[Table("NotificationSchedules")]
    public class NotificationSchedule : AuditedEntity , IMayHaveTenant
    {
			public int? TenantId { get; set; }


		[Required]
		[StringLength(NotificationScheduleConsts.MaxCodeLength, MinimumLength = NotificationScheduleConsts.MinCodeLength)]
		public virtual string Code { get; set; }

		[Required]
		[StringLength(NotificationScheduleConsts.MaxNameLength, MinimumLength = NotificationScheduleConsts.MinNameLength)]
		public virtual string Name { get; set; }

		[Required]
		[StringLength(NotificationScheduleConsts.MaxTitleLength, MinimumLength = NotificationScheduleConsts.MinTitleLength)]
		public virtual string Title { get; set; }

		[Required]
		[StringLength(NotificationScheduleConsts.MaxBodyLength, MinimumLength = NotificationScheduleConsts.MinBodyLength)]
		public virtual string Body { get; set; }

		public virtual DateTime DateSchedule { get; set; }

		public virtual DateTime? DateSend { get; set; }

		public virtual CommonConst.SendNotificationStatus Status { get; set; }

		public virtual CommonConst.SystemAccountType AccountType { get; set; }

		public virtual CommonConst.AgentType AgentType { get; set; }

		[StringLength(NotificationScheduleConsts.MaxExtraInfoLength, MinimumLength = NotificationScheduleConsts.MinExtraInfoLength)]
		public virtual string ExtraInfo { get; set; }

		[StringLength(NotificationScheduleConsts.MaxDescriptionLength, MinimumLength = NotificationScheduleConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }

		public virtual DateTime? DateApproved { get; set; }

		public virtual long? ApproverId { get; set; }


		public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		public string ScheduleId { get; set; }

    }
}
