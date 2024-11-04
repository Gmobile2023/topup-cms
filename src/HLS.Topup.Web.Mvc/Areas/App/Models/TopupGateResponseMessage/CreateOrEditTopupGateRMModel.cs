using System;
using HLS.Topup.TopupGateResponseMessage;

namespace HLS.Topup.Web.Areas.App.Models.TopupGateResponseMessage
{
    public class CreateOrEditTopupGateRMModel
    {
        public  TopupGateResponseMessageDto TopupGateResponseMessage { get; set; }
        public bool IsEditMode => TopupGateResponseMessage.Id != Guid.Empty;
    }
}

