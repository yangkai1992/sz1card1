using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using sz1card1.Common.Enum;

namespace sz1card1.Common
{
    /// <summary>
    /// 会话用户
    /// </summary>
    [Serializable]
    public class SessionUser : ISerializable
    {
        private string sessionId;
        private Binary sid;
        private string businessAccount;
        private string businessName;
        private string userAccount;
        private string loginIp;
        private LoginTypes loginType;
        private string latlong;
        private DateTime loginTime;

        public SessionUser()
        {
        }
        protected SessionUser(SerializationInfo info, StreamingContext context)
        {
            sessionId = info.GetString("sessionId");
            sid = (Binary)info.GetValue("sid", typeof(Binary));
            businessAccount = info.GetString("businessAccount");
            businessName = info.GetString("businessName");
            userAccount = info.GetString("userAccount");
            loginIp = info.GetString("loginIp");
            loginType = (LoginTypes)info.GetInt32("loginType");
            latlong = info.GetString("latlong");
            loginTime = info.GetDateTime("loginTime");
        }

        /// <summary>
        /// 用户会话编号
        /// </summary>
        [DataMember]
        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = value;
            }
        }

        /// <summary>
        /// 商家唯一标识
        /// </summary>
        [DataMember]
        public Binary SID
        {
            get
            {
                return sid;
            }
            set
            {
                sid = value;
            }
        }

        /// <summary>
        /// 商家账号
        /// </summary>
        [DataMember]
        public string BusinessAccount
        {
            get
            {
                return businessAccount;
            }
            set
            {
                businessAccount = value;
            }
        }

        /// <summary>
        /// 商家名称
        /// </summary>
        [DataMember]
        public string BusinessName
        {
            get
            {
                return businessName;
            }
            set
            {
                businessName = value;
            }
        }

        /// <summary>
        /// 工号
        /// </summary>
        [DataMember]
        public string UserAccount
        {
            get
            {
                return userAccount;
            }
            set
            {
                userAccount = value;
            }
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        [DataMember]
        public DateTime LoginTime
        {
            get
            {
                return loginTime;
            }
            set
            {
                loginTime = value;
            }
        }

        /// <summary>
        /// 登录IP
        /// </summary>
        [DataMember]
        public string LoginIP
        {
            get
            {
                return loginIp;
            }
            set
            {
                loginIp = value;
            }
        }

        /// <summary>
        /// 登录方式
        /// </summary>
        [DataMember]
        public LoginTypes LoginType
        {
            get
            {
                return loginType;
            }
            set
            {
                loginType = value;
            }
        }

        /// <summary>
        /// 经纬度
        /// </summary>
        [DataMember]
        public string LatLong
        {
            get
            {
                return latlong;
            }
            set
            {
                latlong = value;
            }
        }

        #region ISerializable 成员

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("sessionId", sessionId);
            info.AddValue("sid", sid);
            info.AddValue("userAccount", userAccount);
            info.AddValue("businessAccount", businessAccount);
            info.AddValue("businessName", businessName);
            info.AddValue("loginTime", loginTime);
            info.AddValue("loginIp", loginIp);
            info.AddValue("loginType", (int)loginType);
            info.AddValue("latlong", latlong);
        }

        #endregion
    }
}
