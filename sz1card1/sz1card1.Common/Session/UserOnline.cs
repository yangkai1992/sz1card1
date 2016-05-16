using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Threading;

namespace sz1card1.Common
{
    public class UserOnline
    {
        private static UserOnline current;
        private List<UserData> users = new List<UserData>();
        //private static CacheItemRemovedCallback userOutCallback=null;

        /// <summary>
        /// 当前实例
        /// </summary>
        public static UserOnline Current
        {
            get
            {
                if (current == null)
                    current = new UserOnline();
                return current;
            }
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="data">用户数据</param>
        public void In(UserData data)
        {
            Out(data.UserName, data.SessionId);
            users.Add(data);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="userName">用户名</param>
        public void Out(string userName)
        {
            foreach (UserData user in users)
            {
                if (user.UserName.ToLower() == userName.ToLower())
                {
                    users.Remove(user);
                    break;
                }
            }
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="userName">用户名</param>
        public void Out(string userName, string sessionId)
        {
            users.RemoveAll(u => u.UserName.ToLower() == userName.ToLower() && u.SessionId.ToLower() == sessionId.ToLower());
        }

        /// <summary>
        /// 获取在线用户数据
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public UserData Get(string userName)
        {
            foreach (UserData user in users)
            {
                if (user.UserName.ToLower() == userName.ToLower())
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取在线用户数据
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public UserData Get(string userName, string sessionId)
        {
            foreach (UserData user in users)
            {
                if (user.UserName.ToLower() == userName.ToLower() && user.SessionId.ToLower() == sessionId.ToLower())
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查用户是否在线
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>状态(true,在线;false,离线)</returns>
        public bool CheckOnline(string userName)
        {
            if (Get(userName) != null)
                return true;
            return false;
        }

        /// <summary>
        /// 获取所有在线用户信息
        /// </summary>
        /// <returns></returns>
        public List<UserData> GetAllUsers()
        {
            return users;
        }

        /// <summary>
        /// 检查用户会话
        /// </summary>
        /// <param name="sessionId">会话编号</param>
        /// <returns>状态(true,已有人强制登录;false,没有)</returns>
        public bool CheckSessionId(string userName, string sessionId)
        {
            UserData obj = Get(userName);
            if (obj != null)
            {
                if (obj.SessionId != sessionId)
                    return true;
            }
            return false;
        }
    }
}
