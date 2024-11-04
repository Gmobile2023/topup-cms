using Abp.Application.Services.Dto;

namespace HLS.Topup.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ProfilePictureId { get; set; }
        public string FullName =>Surname + " " +  Name ;
        public string AccountCode { get; set; }
        public string AccountType { get; set; }
        public string IsVerifyAccount { get; set; }
        public string AgentName { get; set; }
    }
}
