using System.Collections.Generic;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Web.Areas.App.Models.AgentsManage
{
    public class CreateOrEditAgentViewModel
    {
        public UserProfileDto Agent { get; set; }
        
        public List<CityDto> Provinces { get; set; }
        
        public List<DistrictDto> Districts { get; set; }
        
        public List<WardDto> Wards { get; set; }
       
        public bool IsEditMode => Agent.Id.HasValue;
    }
}