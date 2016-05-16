using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Serialization;

namespace sz1card1.Common
{
    [DataContract]
    public class JsonMessage
    {
        private int status = 0;
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success
        {
            get;
            set;
        }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "message")]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "status")]
        public int Status
        {
            get
            { return status; }
            set
            { status = value; }
        }

        public JsonMessage()
        {
        }

        public JsonMessage(bool success)
        {
            this.Success = success;
        }

        /// <summary>
        /// Json消息构造函数
        /// </summary>
        /// <param name="success">状态</param>
        /// <param name="message">消息</param>
        public JsonMessage(bool success, string message)
            : this(success)
        {
            this.Message = message;
        }

        public JsonMessage(bool success, string message, int status)
            : this(success, message)
        {
            this.status = status;
        }
    }
}
