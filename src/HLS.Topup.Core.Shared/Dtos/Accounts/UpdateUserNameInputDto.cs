namespace HLS.Topup.Dtos.Accounts
{
    public class UpdateUserNameInputDto
    {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
    }
}
