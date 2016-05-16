using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Principal;
using Microsoft.Reporting.WebForms;
using sz1card1.Common.IdentityImpersonate;

namespace sz1card1.Common
{
    /// <summary>
    /// 实现一个ReportServerCredentials的接口
    /// </summary>
    [Serializable]
    public class ReportServerCredentials : IReportServerCredentials
    {
        private IdentityUser user;
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        private static Dictionary<string, WindowsIdentity> userTokens = new Dictionary<string, WindowsIdentity>();

        /// <summary>
        /// 使用window集成身份验证的时候用无参数构造函数
        /// </summary>
        public ReportServerCredentials()
        {
            user = IdentityUser.GetIdentityUser();
            if (user == null)
            {
                user = new IdentityUser();
            }
            if (!userTokens.Keys.Contains(user.Name))
            {
                lock (userTokens)
                {
                    if (!userTokens.Keys.Contains(user.Name))
                    {
                        userTokens.Add(user.Name, null);
                    }
                }
            }
        }

        /// <summary>
        /// 使用Network Service验证的时候，需要提供用户名和密码以及域名
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="DomainName"></param>
        public ReportServerCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                if (user == null)
                {
                    user = new IdentityUser();
                }
                if (!userTokens.Keys.Contains(user.Name))
                {
                    lock (userTokens)
                    {
                        if (!userTokens.Keys.Contains(user.Name))
                        {
                            userTokens.Add(user.Name, null);
                        }
                    }
                }
                if (userTokens[user.Name] == null)
                {
                    lock (userTokens)
                    {
                        if (userTokens[user.Name] == null)
                        {
                            userTokens[user.Name] = Impersonator.GetWindowsIdentity(user);
                        }
                    }
                }
                return userTokens[user.Name];
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                return null;
            }
        }

        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string user, out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}
