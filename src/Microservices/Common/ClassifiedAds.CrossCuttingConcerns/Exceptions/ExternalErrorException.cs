using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class ExternalErrorException : CommonException
    {
        public string partnerName { get; set; }

        public ExternalErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }

        public ExternalErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public ExternalErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }

        public ExternalErrorException(string code, string message, int httpStatus, Exception exception, string partnerName)
            : base(code, message, httpStatus, exception)
        {
            this.partnerName = partnerName;
        }
    }
}