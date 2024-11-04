using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace HLS.Topup.Security
{
    [Table("AbpSmsMessage")]
    public class OtpMessage : AuditedEntity, IMayHaveTenant
    {
        [Column(TypeName = "varchar(50)")] public string PhoneNumer { get; set; }
        [Column(TypeName = "nvarchar(255)")] public string Message { get; set; }
        [Column(TypeName = "nvarchar(255)")] public string Result { get; set; }
        [Column(TypeName = "varchar(50)")] public string Channel { get; set; }
        public int? TenantId { get; set; }
    }
}
