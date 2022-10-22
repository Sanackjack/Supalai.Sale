using System;
namespace ClassifiedAds.CrossCuttingConcerns.BaseResponse
{
    public class BaseResponse
    {
        public StatusResponse status { get; set; }
        public Object? data { get; set; }

        public BaseResponse(StatusResponse status)
        {
            this.status = status;
        }

        public BaseResponse(StatusResponse status, Object data)
        {
            this.status = status;
            this.data = data;
        }
    }
}
