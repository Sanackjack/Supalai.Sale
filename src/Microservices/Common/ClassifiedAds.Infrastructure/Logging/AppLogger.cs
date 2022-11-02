using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Twilio.TwiML.Messaging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ClassifiedAds.Infrastructure.Logging
{
    public class AppLogger : IAppLogger
    {

        private readonly ILogger _log;

        private readonly IHttpContextAccessor _context;

        public AppLogger(IHttpContextAccessor context, ILogger<Type> logger)
        {
            _context = context;
            _log = logger;
        }

        private void Log(LogLevel level, string? message, string? className)
        {
            var remoteHost = _context.HttpContext.Connection.RemoteIpAddress.ToString();
            string currTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mmzzz");
            string username = _context.HttpContext.Items["UserName"] != null ? _context.HttpContext.Items["UserName"].ToString() : "Anonymous";
            string correlationId = _context.HttpContext.TraceIdentifier;
            string buildMsg = string.Join("|", new string[] { currTime, level.ToString() , "spl-om-backend", correlationId, remoteHost, username, className, "message: " + message });

            _log.Log(level, buildMsg, null);
        }

        public void Info(string? message, string className = null)
        {
            if (className.IsNullOrEmpty())
            {
                var stackTrace = new System.Diagnostics.StackTrace(1);
                var fullnameArr = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName.Split('.');
                var classname = fullnameArr[fullnameArr.Length - 1];
                var methodname = stackTrace.GetFrame(0).GetMethod().Name;
                string[] name = { classname, methodname };
                className = string.Join('.', name);
            }

            this.Log(LogLevel.Information, message, className);
        }

        public void Debug(string? message, string className = null)
        {

            if (className.IsNullOrEmpty())
            {
                var stackTrace = new System.Diagnostics.StackTrace(1);
                var fullnameArr = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName.Split('.');
                var classname = fullnameArr[fullnameArr.Length - 1];
                var methodname = stackTrace.GetFrame(0).GetMethod().Name;
                string[] name = { classname, methodname };
                className = string.Join('.', name);
            }

            this.Log(LogLevel.Debug, message, className);
        }

        public void Error(string? message, string className = null)
        {
            if (className.IsNullOrEmpty())
            {
                var stackTrace = new System.Diagnostics.StackTrace(1);
                var fullnameArr = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName.Split('.');
                var classname = fullnameArr[fullnameArr.Length - 1];
                var methodname = stackTrace.GetFrame(0).GetMethod().Name;
                string[] name = { classname, methodname };
                className = string.Join('.', name);
            }

            this.Log(LogLevel.Error, message, className);
        }

        public void Warn(string? message, string className = null)
        {
            if (className.IsNullOrEmpty())
            {
                var stackTrace = new System.Diagnostics.StackTrace(1);
                var fullnameArr = stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName.Split('.');
                var classname = fullnameArr[fullnameArr.Length - 1];
                var methodname = stackTrace.GetFrame(0).GetMethod().Name;
                string[] name = { classname, methodname };
                className = string.Join('.', name);
            }

            this.Log(LogLevel.Warning, message, className);
        }

    }
}
