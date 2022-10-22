using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class SystemErrorException : CommonException
    {

        public SystemErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }

        public SystemErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public SystemErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }
    }
}
