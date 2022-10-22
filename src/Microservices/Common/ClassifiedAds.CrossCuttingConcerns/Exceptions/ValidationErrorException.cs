using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class ValidationErrorException : CommonException
    {
        public string massageDetailInValid { get; set; }

        public ValidationErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }

        public ValidationErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public ValidationErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }

        public ValidationErrorException(string code, string message, int httpStatus, Exception exception, string massageDetailInValid)
            : base(code, message, httpStatus, exception)
        {
            this.massageDetailInValid = massageDetailInValid;
        }
    }
}