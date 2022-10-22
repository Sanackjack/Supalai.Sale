using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class AuthenicationErrorException : CommonException
    {
        public string userName { get; set; }

        public AuthenicationErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }
        public AuthenicationErrorException(ResponseData response, string userName)
            : base(response.Code, response.Message, response.HttpStatus)
        {
            this.userName = userName;
        }

        public AuthenicationErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public AuthenicationErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }

        public AuthenicationErrorException(string code, string message, int httpStatus, string userName)
            : base(code, message, httpStatus)
        {
            this.userName = userName;
        }
    }
}