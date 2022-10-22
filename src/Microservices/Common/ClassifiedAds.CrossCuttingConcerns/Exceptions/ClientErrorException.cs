using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class ClientErrorException : CommonException
    {
        public ClientErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }

        public ClientErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public ClientErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }
    }
}