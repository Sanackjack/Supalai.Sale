using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ClassifiedAds.Infrastructure.Logging;
using Twilio.TwiML.Messaging;

namespace ClassifiedAds.Infrastructure.Middleware
{
    public class LogMiddleware
    {

        private readonly IAppLogger _logger ; 
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next, IAppLogger appLogger)
        {
            _next = next;
            _logger = appLogger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                EndpointMetadataCollection endpointMetaData = context.Features.Get<IEndpointFeature>()?.Endpoint.Metadata;

                context.Request.EnableBuffering();

                context.TraceIdentifier = Guid.NewGuid().ToString();

                //var auditLog = await LogRequest(context);
                //await LogResponse(context, auditLog);

                var watch = new Stopwatch();
                watch.Start();
                await LogRequest(context);
                await LogResponse(context, watch);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                //Custom exception logging here
            }
        }

        public async Task LogRequest(HttpContext context)
        {
            IHttpRequestFeature features = context.Features.Get<IHttpRequestFeature>();
            IFormCollection form = null;
            string formString = string.Empty;

            if (context.Request.HasFormContentType)
            {
                form = context.Request.Form;
            }
            else
            {
                formString = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var injectedRequestStream = new MemoryStream();
                byte[] bytesToWrite = Encoding.UTF8.GetBytes(formString);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = injectedRequestStream;
            }
            var body = form != null ? Newtonsoft.Json.JsonConvert.SerializeObject(form) : formString;

            _logger.Info("[Request] " + body);

        }

        public async Task LogResponse(HttpContext context, Stopwatch watch)
        {
            //if (auditLog == null)
            //{
            //    await _next(context);
            //    return;
            //}

            Stream originalBody = context.Response.Body;

            try
            {

                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await _next(context);

                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();

                    watch.Stop();

                    _logger.Info("[Response] " + responseBody);

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
