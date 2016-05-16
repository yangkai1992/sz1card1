using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    public class UserData
    {
        private Dictionary<string, object> others;

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最后会话时间
        /// </summary>
        public DateTime LastSessionTime
        {
            get;
            set;
        }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP
        {
            get;
            set;
        }

        /// <summary>
        /// 用户会话编号
        /// </summary>
        public string SessionId
        {
            get;
            set;
        }

        /// <summary>
        /// 登录用户其他信息
        /// </summary>
        public Dictionary<string, object> Others
        {
            get
            {
                if (others == null)
                    others = new Dictionary<string, object>();
                return others;
            }
            set
            {
                others = value;
            }
        }

        public UserData()
        {
            LastSessionTime = LoginTime;
        }
    }
}
