using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Web.Hosting;
using System.Web;
using System.Windows.Forms;

namespace sz1card1.Common.Log
{
    public static class LoggingService
    {

        public static void Debug(object message, Exception exception)
        {
            WriteLog(message, exception, EventLogEntryType.Information);
        }

        public static void Debug(object message)
        {
            WriteLog(message, EventLogEntryType.Information);
        }

        private static string RequestInfo(HttpContext context)
        {
            try
            {
                StringBuilder requestInfo = new StringBuilder();
                requestInfo.Append(string.Format("{0}  {1}", context.Request.HttpMethod, context.Request.Url));
                if (context.Request.Form.AllKeys.Length > 0)
                {
                    requestInfo.Append(" Data: ");
                    List<string> formData = new List<string>();
                    for (int i = 0; i < context.Request.Form.AllKeys.Length; i++)
                    {
                        formData.Add(string.Format("{0}:{1},", context.Request.Form.GetKey(i), string.Join("", context.Request.Form.GetValues(i))));
                    }
                    requestInfo.Append(string.Join(",", formData.ToArray()));
                }
                return requestInfo.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message +  ex.StackTrace;
            }
        }

        public static void Info(object message, Exception exception)
        {
            WriteLog(message, exception, EventLogEntryType.Information);
        }

        public static void Info(object message)
        {
            WriteLog(message, EventLogEntryType.Information);
        }

        public static void Warn(object message, Exception exception)
        {
            WriteLog(message, exception, EventLogEntryType.Warning);
        }

        public static void Warn(object message)
        {
            WriteLog(message, EventLogEntryType.Warning);
        }

        public static void Error(object message, Exception exception)
        {
            WriteLog(message, exception, EventLogEntryType.Error);
        }

        public static void Error(object message)
        {
            WriteLog(message, EventLogEntryType.Error);
        }

        public static void Fatal(object message, Exception exception)
        {
            WriteLog(message, exception, EventLogEntryType.Error);
        }

        public static void Fatal(object message)
        {
            WriteLog(message, EventLogEntryType.Error);
        }

        private static void WriteLog(object message, Exception exception, EventLogEntryType eventLogEntryType)
        {
            StringBuilder logInfo = new StringBuilder();
            if (message != null)
            {
                logInfo.AppendLine("SourceMessage:  " + message.ToString());
            }
            if (exception != null)
            {
                logInfo.AppendLine("ExceptionMessage:  " + exception.Message);
                logInfo.AppendLine("ExceptionStackTrace:  " + exception.StackTrace);
            }
            if (HttpContext.Current != null && eventLogEntryType == EventLogEntryType.Warning)
            {
                logInfo.AppendLine("HttpContext: " + RequestInfo(HttpContext.Current));
            }
            WriteLog(logInfo.ToString(), eventLogEntryType);
        }

        private static void WriteLog(object message, EventLogEntryType eventLogEntryType)
        {
            string sourceName = HttpContext.Current == null ? Application.ProductName : HostingEnvironment.ApplicationHost.GetSiteName();
            WriteLog(sourceName, message, eventLogEntryType);
        }

        private static void WriteLog(string sourceName, object message, EventLogEntryType eventLogEntryType)
        {
            EventLog log = new EventLog();
            StringBuilder logInfo = new StringBuilder();
            log.Source = sourceName;
            if (message != null)
            {
                logInfo.AppendLine(message.ToString());
            }
            try
            {
                SessionUser user = DistributedSessionUserManager.GetSessionUser();
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.LoginIP))
                    {
                        logInfo.AppendLine("LoginIP:  " + user.LoginIP);
                    }
                    if (!string.IsNullOrEmpty(user.SessionId))
                    {
                        logInfo.AppendLine("SessionId:  " + user.SessionId);
                    }
                    if (!string.IsNullOrEmpty(user.BusinessAccount))
                    {
                        logInfo.AppendLine("BusinessAccount:  " + user.BusinessAccount);
                    }
                    if (!string.IsNullOrEmpty(user.BusinessName))
                    {
                        logInfo.AppendLine("BusinessName:  " + user.BusinessName);
                    }
                    if (!string.IsNullOrEmpty(user.UserAccount))
                    {
                        logInfo.AppendLine("UserAccount:  " + user.UserAccount);
                    }
                }
            }
            catch
            {
            }
            log.WriteEntry(logInfo.ToString(), eventLogEntryType);
        }
    }
}
