using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;

namespace ClassifiedAds.CrossCuttingConcerns.Exceptions
{
    public class DataErrorException : CommonException
    {
        public string dataDetail { get; set; }

        public DataErrorException(ResponseData response)
            : base(response.Code, response.Message, response.HttpStatus) { }

        public DataErrorException(string code, string message, int httpStatus)
            : base(code, message, httpStatus) { }

        public DataErrorException(string code, string message, int httpStatus, Exception exception)
            : base(code, message, httpStatus, exception) { }

        public DataErrorException(string code, string message, int httpStatus, Exception exception, string dataDetail)
            : base(code, message, httpStatus, exception)
        {
            this.dataDetail = dataDetail;
        }
    }
}
