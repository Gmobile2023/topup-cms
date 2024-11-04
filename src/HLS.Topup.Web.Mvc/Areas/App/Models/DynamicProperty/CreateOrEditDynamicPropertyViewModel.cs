using System.Collections.Generic;
using HLS.Topup.DynamicEntityProperties.Dto;

namespace HLS.Topup.Web.Areas.App.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
