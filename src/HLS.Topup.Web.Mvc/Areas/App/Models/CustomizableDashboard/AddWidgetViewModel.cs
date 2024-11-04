using System.Collections.Generic;
using HLS.Topup.DashboardCustomization.Dto;

namespace HLS.Topup.Web.Areas.App.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
