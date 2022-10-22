using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class TokenErrorException : CommonException
    {
        public TokenErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }

        public TokenErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public TokenErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }
    }
}