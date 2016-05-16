using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using sz1card1.Common.IdentityImpersonate;
using sz1card1.Component.YFSClient;

namespace sz1card1.Common.FileServer
{
    public class FileServerClient
    {
        private string hostUrl;
        private string hostDomain;

        public FileServerClient()
        {
        }

        public FileServerClient(string hostUrl)
        {
            this.hostUrl = hostUrl;
            this.hostDomain = hostUrl.ToLower().Replace("http://", "");
        }

        /// <summary>
        /// 上传文件到文件服务器
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="isTemp"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Upload(string originalUrl, string fileName, byte[] fileBytes, bool isTemp, out string message)
        {
            return Upload(originalUrl, fileName, fileBytes, isTemp, Encoding.UTF8, out message);
        }

        /// <summary>
        /// 上传文件到文件服务器
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="isTemp"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Upload(string fileName, byte[] fileBytes, bool isTemp, out string message)
        {
            return Upload(null, fileName, fileBytes, isTemp, Encoding.UTF8, out message);
        }

        /// <summary>
        /// 上传文件到文件服务器
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Upload(string fileName, byte[] fileBytes, out string message)
        {
            return Upload(null, fileName, fileBytes, false, Encoding.UTF8, out message);
        }

        /// <summary>
        /// 上传文件到文件服务器 update 2014-12-22 by hjw
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="isTemp"></param>
        /// <param name="encoding"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Upload(string originalUrl, string fileName, byte[] fileBytes, bool isTemp, Encoding encoding, out string message)
        {
            return Upload(null, fileBytes, fileName, "", out message, isTemp);
        }

        /// <summary>
        /// 获取文件流 update 2014-12-22 by hjw
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileBytes">返回的文件流</param>
        /// <returns></returns>
        public bool GetFileBytes(string filePath, out byte[] fileBytes)
        {
            string message = string.Empty;
            return GetFileBytes(filePath, out fileBytes, out message);
        }

        /// <summary>
        /// 删除文件 update 2014-12-22 by hjw
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="encoding"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Delete(string fileUrl, Encoding encoding, out string message)
        {
            return Delete(fileUrl, out message);
        }

        /// <summary>
        /// 返回与文件服务器上的指定虚拟路径相对应的物理文件路径。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            List<string> paths = path.Split('/').ToList();
            string machineName = ConfigurationManager.AppSettings["sz1card1.MachineName"];
            if (paths.Count > 2 && paths[2].ToLower() == "temp")
            {
                if (string.IsNullOrEmpty(machineName)) //大服务器：未加 sz1card1.MachineName
                {
                    return @"\\sz1card1-m2\Website\FileServer\Files" + path.Replace("/", "\\");
                }
                else //小服务器
                {
                    return @"\\" + machineName + @"\Website\FileServer\Files" + path.Replace("/", "\\");
                }
            }
            else
            {
                paths.Insert(2, "persistent");
                if (string.IsNullOrEmpty(machineName))
                {
                    return @"\\sz1card1-m2\Website\FileServer\Files" + string.Join("\\", paths.ToArray());
                }
                else
                {
                    return @"\\" + machineName + @"\Website\FileServer\Files" + string.Join("\\", paths.ToArray());
                }
            }
        }

        /// <summary>
        /// 获取身份验证Cookie
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private CookieContainer GetCookieContainer()
        {
            string cookieName;
            string cookieValue;
            IdentityUser user = IdentityUser.GetIdentityUser();
            if (user == null)
            {
                throw new NullReferenceException("Windows身份标识为空!");
            }
            cookieName = ".ASPXAUTH";
            cookieValue = user.ToJson().ToDes();
            CookieContainer container = new CookieContainer();
            Cookie cookie = new Cookie(cookieName, cookieValue);
            cookie.Domain = hostDomain;
            container.Add(cookie);
            return container;
        }

        /// <summary>
        /// 上传文件 add 2014-12-22 by hjw
        /// </summary>
        /// <param name="sid">商家sid，无商家时NULL</param>
        /// <param name="fileBytes">二进制文件</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="extension">扩展名</param>
        /// <param name="isTemp">是否是临时文件，默认否</param>
        /// <returns></returns>
        public bool Upload(Binary sid, byte[] fileBytes, string fileName, string extension, out string message, bool isTemp = false)
        {
            CustomResult cr = new CustomResult();
            if (string.IsNullOrEmpty(extension) && fileName.IndexOf('.') > 0)
            {
                //如果未指明扩展名，则从文件名中获取
                extension = fileName.Substring(fileName.LastIndexOf('.') + 1, fileName.Length - fileName.LastIndexOf('.') - 1);
            }
            extension = extension.Replace(".", "");
            string userid = sid != null ? userid = sid.ToBase64String() : string.Empty;
            cr = FileAPI.Post(userid, fileBytes, fileName, extension, isTemp);
            message = cr.ResultStatus ? cr.FilePath : cr.ResultMessage;
            if (!cr.ResultStatus)
            {
                if (message.IndexOf("non-connected") >= 0)
                {
                    cr = FileAPI.Post(userid, fileBytes, fileName, extension, isTemp);
                    message = cr.ResultStatus ? cr.FilePath : cr.ResultMessage;
                }
                if (!cr.ResultStatus)
                {
                    sz1card1.Common.Log.LoggingService.Info("上传文件失败：" + message + "\r\n【userid:" + userid + ",fileBytes:" + fileBytes.LongLength.ToString() + ",fileName:" + fileName + ",extension:" + extension + ",isTemp:" + isTemp.ToString() + "】");
                }
            }
            return cr.ResultStatus;
        }

        /// <summary>
        /// 删除文件 add 2014-12-22 by hjw
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="message">返回消息</param>
        /// <returns>是否成功</returns>
        public bool Delete(string filePath, out string message)
        {
            CustomResult cr = FileAPI.Delete(filePath);
            message = cr.ResultMessage;
            if (!cr.ResultStatus)
            {
                if (message.IndexOf("non-connected") >= 0)
                {
                    cr = FileAPI.Delete(filePath);
                    message = cr.ResultMessage;
                }
                if (!cr.ResultStatus)
                {
                    sz1card1.Common.Log.LoggingService.Info("删除文件失败：" + message + "\r\n【filePath:" + filePath + "】");
                }
            }
            return cr.ResultStatus;
        }

        /// <summary>
        /// 获取文件流 add 2014-12-22 by hjw
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileBytes">返回的文件流</param>
        /// <param name="message">返回的消息</param>
        /// <returns></returns>
        public bool GetFileBytes(string filePath, out byte[] fileBytes, out string message)
        {
            CustomResult cr = FileAPI.GetFileBytes(filePath, out fileBytes);
            message = cr.ResultMessage;
            if (!cr.ResultStatus)
            {
                if (message.IndexOf("non-connected") >= 0)
                {
                    cr = FileAPI.GetFileBytes(filePath, out fileBytes);
                    message = cr.ResultMessage;
                }
                if (!cr.ResultStatus)
                {
                    sz1card1.Common.Log.LoggingService.Info("获取文件流失败：" + message + "\r\n【filePath:" + filePath + "】");
                }
            }
            return cr.ResultStatus;
        }
    }
}
