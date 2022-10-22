using System;
namespace ClassifiedAds.CrossCuttingConcerns.BaseResponse
{
    public class StatusResponse
    {
        public string code { get; set; }
        public string message { get; set; }

        public StatusResponse()
        {
            this.code = "0";
            this.message = "Success";
        }

        public StatusResponse(string code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
