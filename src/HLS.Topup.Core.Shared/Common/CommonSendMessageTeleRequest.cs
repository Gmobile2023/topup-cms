using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Common
{
    [Route("/api/v1/common/tele/send", "Post")]
    public class CommonSendMessageTeleRequest : IPost, IReturn<NewMessageReponseBase<object>>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public string Code { get; set; }
        public BotMessageType MessageType { get; set; }
        public BotType BotType { get; set; }
    }
    public enum BotMessageType
    {
        Error = 1,
        Wraning = 2,
        Message = 3
    }
    public enum BotType
    {
        Dev = 1,
        Sale = 2,
        CardMapping = 3,
        Provider = 4,
        Transaction = 5,
        Stock = 6,
        Deposit = 7,
        Channel = 8,
        Private = 9
    }
}
