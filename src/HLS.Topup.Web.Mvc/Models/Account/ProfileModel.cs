using System.Collections.Generic;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Products.Dtos;

namespace HLS.Topup.Web.Models.Account
{
    public class ProfileModel
    {
        public UserEditDto UserInfo { get; set; }
        public UserAccountInfoDto UserAccountInfo { get; set; }
        public List<ProductDiscountDto> Disouncts { get; set; }
        public bool IsLevel2Password { get; set; }
        public UserProfileDto AgentProfile { get; set; }
    }


    public class UserEditProfileModel
    {
        public string Surname { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public CommonConst.IdType? IdType { get; set; }//Loại giấy tờ
        public string IdentityId { get; set; }//Số giấy tờ
        public string FrontPhoto { get; set; }
        public string BackSitePhoto { get; set; }
        public string AgentName { get; set; }
        public bool IsUpdateVerify { get; set; }

        public string ExtraInfo { get; set; }
        public string url_before { get; set; }
        public string url_after { get; set; }
    }

    public class UserChangepasswordProfileModel
    {
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordConfirm { get; set; }
    }
    public class ProductDiscountAccount
    {
        public List<ProductDiscountDto> Products { get; set; }
        public string ServiceCode { get; set; }
    }
    public class ProductServiceDto
    {
        public List<ProductDto> Products { get; set; }
        public string ServiceCode { get; set; }
    }

    public class PaymentVerifyTransTypeModel
    {
        public CommonConst.VerifyTransType Method { get; set; }
    }
}
