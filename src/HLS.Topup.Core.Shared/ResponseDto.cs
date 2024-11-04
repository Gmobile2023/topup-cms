using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;

namespace HLS.Topup
{
    public class ResponseMessages
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ExtraInfo { get; set; }
        public int Total { get; set; }
        public object Payload { get; set; }

        public ResponseMessages(){}
        public ResponseMessages(string responseCode)
        {
            this.ResponseCode = responseCode;
        }

        public ResponseMessages(string responseCode, string message) : this(responseCode)
        {
            this.ResponseMessage = message;
        }
    }

    public class TransactionResponse
    {
        public string Token { get; set; }
        public string TransCode { get; set; }
        public string ReturnUrl { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Payload { get; set; }
        public string ExtraInfo { get; set; }
        public decimal Balance { get; set; }
    }

    [DataContract]
    public class VttResponse
    {
        [DataMember(Name = "errorCode")] public long ErrorCode { get; set; }
        [DataMember(Name = "message")] public string Message { get; set; }

        [DataMember(Name = "cmnd")] public string Cmnd { get; set; }

        [DataMember(Name = "code")] public long Code { get; set; }
    }

    public class VttResponseData<T> : VttResponse
    {
        [DataMember(Name = "data")] public T Data { get; set; }
    }

    [DataContract]
    public class VttDataUrlCaptcha
    {
        [DataMember(Name = "url")] public string Url { get; set; }

        [DataMember(Name = "sid")] public string Sid { get; set; }
    }

        public class ReponseMessageResultBase<T>
    {
        public T Results { get; set; }
        public StatusResponse ResponseStatus { get; set; }
    }

    //Tạo riêng class này. dùng của serviceStack đang bị lỗi version
    public class StatusResponse
    {
        public StatusResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatus"/> class.
        /// A response status with an errorcode == failure
        /// </summary>
        public StatusResponse(string errorCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatus"/> class.
        /// A response status with an errorcode == failure
        /// </summary>
        public StatusResponse(string errorCode, string message)
            : this(errorCode)
        {
            this.Message = message;
        }

        /// <summary>
        /// Holds the custom ErrorCode enum if provided in ValidationException
        /// otherwise will hold the name of the Exception type, e.g. typeof(Exception).Name
        ///
        /// A value of non-null means the service encountered an error while processing the request.
        /// </summary>
        [DataMember(Order = 1)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// A human friendly error message
        /// </summary>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        /// <summary>
        /// The Server StackTrace when DebugMode is enabled
        /// </summary>
        [DataMember(Order = 3)]
        public string StackTrace { get; set; }

        /// <summary>
        /// For multiple detailed validation errors.
        /// Can hold a specific error message for each named field.
        /// </summary>
        //[DataMember(Order = 4)]
        //public List<ResponseError> Errors { get; set; }

        /// <summary>
        /// For additional custom metadata about the error
        /// </summary>
        [DataMember(Order = 5)]
        public Dictionary<string, string> Meta { get; set; }
    }

    public class ResponseMessageApi<T>
    {
        [DataMember(Name = "result")] public T Result { get; set; }
        [DataMember(Name = "success")] public bool Success { get; set; }
        [DataMember(Name = "error")] public ErrorMessage Error { get; set; }
    }
    public class ErrorMessage
    {
        [DataMember(Name = "code")] public int Code { get; set; }
        [DataMember(Name = "message")] public string Message { get; set; }
        [DataMember(Name = "details")] public string Details { get; set; }

        [DataMember(Name = "validationErrors")]
        public string ValidationErrors { get; set; }
    }

    public class NewMessageReponseBase<T>
    {
        public T Results { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
    public class ResponseProvider
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
