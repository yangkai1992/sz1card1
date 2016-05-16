using System;
using System.Data;
using System.Data.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Memcached;
using sz1card1.Common.Enum;
using sz1card1.Common.Distributed;

namespace sz1card1.Common
{
    public class DistributedSessionUserManager
    {
        private static string keySessionUser = "SessionUser";

        private static string prefixUserLatSessionId = "UserLastSessionId_";

        private static string generateUserLatSessionId(SessionUser sessionUser)
        {
            return string.Format("{0}{1}@{2}", prefixUserLatSessionId, sessionUser.UserAccount, sessionUser.BusinessAccount);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sessionUser"></param>
        /// <returns></returns>
        public static void Login(SessionUser sessionUser)
        {
            Update(sessionUser);
            SingleOnline(sessionUser);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="sessionUser"></param>
        public static void Update(SessionUser sessionUser)
        {
            var session = DistributedContext.Current.Session;
            session.Timeout = 240;
            session[keySessionUser] = sessionUser;
        }

        /// <summary>
        /// 初始化会话信息（会导致其他端强制下线）
        /// </summary>
        /// <param name="sessionUser"></param>
        public static void Init(SessionUser sessionUser)
        {
            Update(sessionUser);
        }

        /// <summary>
        /// 初始化会话信息（不会导致其他端强制下线）
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="userAccount"></param>
        public static void Init(Binary sid, string userAccount)
        {
            SessionUser sessionUser = new SessionUser()
            {
                SID = sid,
                UserAccount = userAccount
            };
            Update(sessionUser);
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public static void Logout()
        {
            var sessionUser = GetSessionUser();
            if (sessionUser != null)
            {
                //清除Application
                var userLatSessionId = generateUserLatSessionId(sessionUser);
                DistributedContext.Current.Application[userLatSessionId] = null;
                //清除session
                var session = DistributedContext.Current.Session;
                session[keySessionUser] = null;
            }
        }

        /// <summary>
        /// 获取Session信息
        /// </summary>
        /// <returns></returns>
        public static SessionUser GetSessionUser()
        {
            var session = DistributedContext.Current.Session;
            try
            {
                SessionUser sessionUser = (SessionUser)DistributedContext.Current.Session[keySessionUser];
                return sessionUser;
            }
            catch { return null; }
        }

        /// <summary>
        /// 检查Session信息
        /// </summary>
        /// <returns>0,正常;-1,强制下线;-2,超时</returns>
        public static int CheckSessionId()
        {
            SessionUser sessionUser = GetSessionUser();
            if (sessionUser == null)
            {
                return -2;
            }
            else
            {
                if (sessionUser.SessionId == null)
                {
                    DistributedContext.Current.Session.Timeout = 30;
                    return -1;
                }
                var userLatSessionId = generateUserLatSessionId(sessionUser);
                var lastSessionId = DistributedContext.Current.Application[userLatSessionId];
                if (lastSessionId != null && lastSessionId.ToString() != sessionUser.SessionId)
                {
                    DistributedContext.Current.Session.Timeout = 30;
                    sessionUser.SessionId = null;
                    Update(sessionUser);
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 在线SessionUser总数
        /// </summary>
        public static int Count
        {
            get
            {
                if (DistributedContext.Current is RedisDistributedContext)
                {
                    var count = 0;
                    var redisStoreProvider = new RedisStoreProvider(typeof(RedisSessionDataState).Name);
                    var keys = redisStoreProvider.GetKeys(null);
                    foreach (string key in keys)
                    {
                        if (redisStoreProvider.HashGetString(key, keySessionUser) != null)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// 每个用户仅保持一个会话
        /// </summary>
        /// <param name="sessionUser"></param>
        public static void SingleOnline(SessionUser sessionUser)
        {
            if (!string.IsNullOrEmpty(sessionUser.SessionId) && !string.IsNullOrEmpty(sessionUser.BusinessAccount))
            {
                var userLatSessionId = generateUserLatSessionId(sessionUser);
                DistributedContext.Current.Application[userLatSessionId] = sessionUser.SessionId;
            }
        }
    }
}
