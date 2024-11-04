using HLS.Topup.AgentsManage.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.AgentsManage
{
    public class AgentsViewModel : AgentsDto
    {
        
    }

    public class LockOrUnlockModel
    {
        public int? Id { get; set; }
        
        public string Type { get; set; }
    }
}