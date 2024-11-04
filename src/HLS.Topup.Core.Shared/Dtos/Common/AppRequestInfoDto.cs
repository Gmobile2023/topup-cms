using System.Runtime.Serialization;

namespace HLS.Topup.Dtos.Common
{
    public class AppRequestInfoDto
    {
        [DataMember(Name = "app_version")]
        public string AppVersion { get; set; }
        [DataMember(Name = "app_code")]
        public string AppCode { get; set; }
        [DataMember(Name = "app_user_id")]
        public string AppUserId { get; set; }
        [DataMember(Name = "app_user_accountCode")]
        public string AppUserAccountCode { get; set; }
        [DataMember(Name = "app_request_date")]
        public string AppRequestDate { get; set; }
        [DataMember(Name = "channel")]
        public string Channel { get; set; }
        [DataMember(Name = "app_channel")]
        public int ChannelId { get; set; }
    }
}
