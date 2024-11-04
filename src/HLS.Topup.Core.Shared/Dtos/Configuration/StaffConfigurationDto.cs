using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Dtos.Configuration
{
    public class CreateStaffUserInput : UserInputDto
    {
        [Required]
        public decimal LimitAmount { get; set; }
        [Required]
        public decimal LimitPerTrans { get; set; }
        public string Description { get; set; }
        [Required]
        public List<int> Days { get; set; }
        [Required]
        public StaffTime FromTime { get; set; }
        [Required]
        public StaffTime ToTime { get; set; }
        [Required]
        public string Password { get; set; }
        public long? UserId { get; set; }
        public long? ParentUserId { get; set; }
    }

    public class UpdateStaffUserInput : UserInputDto
    {
        [Required]
        public decimal LimitAmount { get; set; }
        [Required]
        public decimal LimitPerTrans { get; set; }
        public string Description { get; set; }
        [Required]
        public List<int> Days { get; set; }
        [Required]
        public StaffTime FromTime { get; set; }
        [Required]
        public StaffTime ToTime { get; set; }
        public string Password { get; set; }
        public long? UserId { get; set; }
    }

    public class StaffTime
    {
        public int FromTime { get; set; }
        public int ToTime { get; set; }
    }

    public class UserStaffDto:UserInfoDto
    {
        public decimal LimitAmount { get; set; }
        public decimal LimitPerTrans { get; set; }
        public string Description { get; set; }
        public List<int> Days { get; set; }
        public StaffTime FromTime { get; set; }
        public StaffTime ToTime { get; set; }
    }

    public class LockUnlockRequest
    {
        public long UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
