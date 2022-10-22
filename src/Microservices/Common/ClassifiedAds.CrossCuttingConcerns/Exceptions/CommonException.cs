using System;
using System.Data.Common;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class CommonException : Exception
    {
        public int httpStatus { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Exception exception { get; set; }

        public CommonException(string code, string message, int httpStatus)
            : base(message)
        {
            this.httpStatus = httpStatus;
            this.code = code;
            this.message = message;
        }

        public CommonException(string code, string message, int httpStatus, Exception exception)
            : base(message, exception)
        {
            this.httpStatus = httpStatus;
            this.code = code;
            this.message = message;
            this.exception = exception;
        }
    }
}
