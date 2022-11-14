using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.Infrastructure.Logging;

namespace ClassifiedAds.Infrastructure.Web.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger _logger;
        private readonly GlobalExceptionHandlerMiddlewareOptions _options;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next,
            IAppLogger logger,
            GlobalExceptionHandlerMiddlewareOptions options)
        {
            _next = next;
            _logger = logger;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string code = string.Empty;
                string msg = string.Empty;
                int httpStatus;
                switch (ex)
                {
                    case AuthenicationErrorException e:
                        code = e.code;
                        msg = string.IsNullOrEmpty(e.userName)
                            ? e.message : string.Format("[{0}]{1}", e.userName, e.message);
                        httpStatus = e.httpStatus;
                        break;
                    case ClientErrorException e:
                        code = e.code;
                        msg = e.message;
                        httpStatus = e.httpStatus;
                        break;
                    case ValidationErrorException e:
                        code = e.code;
                        msg = string.IsNullOrEmpty(e.massageDetailInValid)
                            ? e.message : string.Format("{0} The problem is {1}", e.message, e.massageDetailInValid);
                        httpStatus = e.httpStatus;
                        break;
                    case TokenErrorException e:
                        code = e.code;
                        msg = e.message;
                        httpStatus = e.httpStatus;
                        break;
                    case ExternalErrorException e:
                        code = e.code;
                        msg = string.IsNullOrEmpty(e.partnerName)
                            ? e.message : string.Format("[{0}]{1}", e.partnerName, e.message);
                        httpStatus = e.httpStatus;
                        break;
                    case SystemErrorException e:
                        code = e.code;
                        msg = e.message;
                        httpStatus = e.httpStatus;

                        if (e.exception != null)
                        {
                            _logger.Error(e.exception.Message);
                        }

                        break;
                    default:
                        code = ResponseData.SYSTEM_ERROR.Code;
                        msg = ResponseData.SYSTEM_ERROR.Message;
                        httpStatus = ResponseData.SYSTEM_ERROR.HttpStatus;
                        _logger.Error(ex.Message);
                        break;
                }

                var baseResponse = new BaseResponse(new StatusResponse(code, msg));
                var result = JsonSerializer.Serialize(baseResponse);
                response.StatusCode = httpStatus;
                await response.WriteAsync(result);
            }
        }

        private string GetErrorMessage(Exception ex)
        {
            if (ex is ValidationException)
            {
                return ex.Message;
            }

            return _options.DetailLevel switch
            {
                GlobalExceptionDetailLevel.None => "An internal exception has occurred.",
                GlobalExceptionDetailLevel.Message => ex.Message,
                GlobalExceptionDetailLevel.StackTrace => ex.StackTrace,
                GlobalExceptionDetailLevel.ToString => ex.ToString(),
                _ => "An internal exception has occurred.",
            };
        }
    }

    public class GlobalExceptionHandlerMiddlewareOptions
    {
        public GlobalExceptionDetailLevel DetailLevel { get; set; }
    }

    public enum GlobalExceptionDetailLevel
    {
        None,
        Message,
        StackTrace,
        ToString,
        Throw,
    }
}
