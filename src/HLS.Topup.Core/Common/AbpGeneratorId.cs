using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace HLS.Topup.Common
{
    [Table("AbpGeneratorIds")]
    public class AbpGeneratorId : Entity<long>
    {
        public virtual string Type { get; set; }

        public virtual int Order { get; set; }
    }
}