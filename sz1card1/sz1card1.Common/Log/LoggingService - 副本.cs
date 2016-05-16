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
            WriteLog(logInfo.ToString(), eventLogEntryType);
        }

        private static void WriteLog(object message, EventLogEntryType eventLogEntryType)
        {
            //EventLog log = new EventLog();
            //StringBuilder logInfo = new StringBuilder();
            //if (HttpContext.Current == null)
            //{
            //    //服务
            //    log.Source = Application.ProductName;
            //}
            //else
            //{
            //    //网站
            //    log.Source = HostingEnvironment.ApplicationHost.GetSiteName();
            //}
            //if (message != null)
            //{
            //    logInfo.AppendLine("Message:  " + message.ToString());
            //}
            //try
            //{
            //    SessionUser user = SessionManager.GetSessionUser();
            //    if (user != null)
            //    {
            //        if (!string.IsNullOrEmpty(user.LoginIP))
            //        {
            //            logInfo.AppendLine("LoginIP:  " + user.LoginIP);
            //        }
            //        if (!string.IsNullOrEmpty(user.SessionId))
            //        {
            //            logInfo.AppendLine("SessionId:  " + user.SessionId);
            //        }
            //        if (!string.IsNullOrEmpty(user.BusinessAccount))
            //        {
            //            logInfo.AppendLine("BusinessAccount:  " + user.BusinessAccount);
            //        }
            //        if (!string.IsNullOrEmpty(user.BusinessName))
            //        {
            //            logInfo.AppendLine("BusinessName:  " + user.BusinessName);
            //        }
            //        if (!string.IsNullOrEmpty(user.UserAccount))
            //        {
            //            logInfo.AppendLine("UserAccount:  " + user.UserAccount);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (baseDirectory[baseDirectory.Length - 1] == '\\')
                {
                    baseDirectory = baseDirectory.Remove(baseDirectory.Length - 1);
                }
                string path = string.Format("{0}\\Log\\{1}", baseDirectory, Environment.MachineName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = path + string.Format("\\{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                FileStream fs = new FileStream(@filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"), message));
                m_streamWriter.Flush();
                m_streamWriter.Close();
                fs.Close();
            //}
            //log.WriteEntry(logInfo.ToString(), eventLogEntryType);
        }
    }
}
