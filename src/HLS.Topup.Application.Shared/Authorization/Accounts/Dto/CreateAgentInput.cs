namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class CreateAgentInput
    {
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Họ
        /// </summary>
        public string SurName { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string EmailAddress { get; set; }

        //public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        
    }
}