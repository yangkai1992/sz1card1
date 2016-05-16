using System;
using System.Data;
using System.Data.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Memcached;
using sz1card1.Common.Enum;
namespace sz1card1.Common
{
    /// <summary>
    /// 会话管理类
    /// </summary>
    public class SessionManager
    {
        public static readonly string sessionUserKeyName = typeof(SessionUser).Name.ToDes();
        private static readonly string sectionName = "MemcachedSessionDataState";
        private static MemcachedStoreProvider provider;
        private static int total;
        static SessionManager()
        {
            provider = new MemcachedStoreProvider(sectionName);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sessionUser"></param>
        /// <returns></returns>
        public static void Login(SessionUser sessionUser)
        {
            //provider.RemoveItem(sessionUser.SessionId);
            MemcachedContext.Current.MemcachedSession[sessionUserKeyName] = sessionUser;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="sessionUser"></param>
        public static void Update(SessionUser sessionUser)
        {
            MemcachedContext.Current.MemcachedSession[sessionUserKeyName] = sessionUser;
        }

        /// <summary>
        /// 初始化会话信息（会导致其他端强制下线）
        /// </summary>
        /// <param name="sessionUser"></param>
        public static void Init(SessionUser sessionUser)
        {
            MemcachedContext.Current.MemcachedSession[sessionUserKeyName] = sessionUser;
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
            MemcachedContext.Current.MemcachedSession[sessionUserKeyName] = sessionUser;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public static void Logout()
        {
            provider.RemoveItem(MemcachedContext.Current.ContextID);
        }

        /// <summary>
        /// 获取Session信息
        /// </summary>
        /// <returns></returns>
        public static SessionUser GetSessionUser()
        {

            return MemcachedContext.Current.MemcachedSession[sessionUserKeyName] as SessionUser;
        }

        /// <summary>
        /// 清除缓存中sessionUser
        /// </summary>
        /// <param name="sessionId"></param>
        public static void RemoveSessionUser(string sessionId)
        {
            provider.RemoveItem(sessionId);
        }

        /// <summary>
        /// 检查Session信息
        /// </summary>
        /// <returns>0,正常;-1,强制下线;-2,超时</returns>
        public static int CheckSessionId()
        {
            SessionUser user = GetSessionUser();
            if (user == null)
            {
                return -2;
            }
            else if (user.SessionId == null)
            {
                Logout();
                return -1;
            }
            return 0;
        }

        public static int GetCount(string where)
        {
            return total;
        }

        public static DataTable GetSessionUserPaged(string where, string orderBy, int start, int limit)
        {
            return GetSessionUserPaged(where, orderBy, start, limit, out total);
        }

        /// <summary>
        /// 获取会话用户分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">第几页（从0开始）</param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static DataTable GetSessionUserPaged(string where, string orderBy, int pageIndex, int pageSize, out int total)
        {
            MemcachedManager memcachedManager = new MemcachedManager(sectionName);
            DataTable dt = memcachedManager.GetItems<SessionUser>(sessionUserKeyName).ToDataTable();
            memcachedManager.Dispose();
            if (string.IsNullOrEmpty(where))
            {
                where = "SessionId IS NOT NULL AND SessionId <> ''";
            }
            else
            {
                where += " AND SessionId IS NOT NULL AND SessionId <> ''";
            }
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "LoginTime DESC";
            }
            string fields = "BusinessAccount,BusinessName,UserAccount,LoginIP,LoginTime,LoginType,LatLong";
            dt = dt.Paged(fields, where, orderBy, pageIndex, pageSize, out total);
            dt.Columns.Add("LoginTypeName", typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                dr["LoginTypeName"] = ((LoginTypes)int.Parse(dr["LoginType"].ToString())).ToString();
            }
            return dt;
        }
    }
}
