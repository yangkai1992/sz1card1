using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Security;
using System.Web;

namespace sz1card1.Common.IdentityImpersonate
{
    public class Impersonator
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private extern static bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private extern static bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        static extern bool RevertToSelf();

        // Test harness.
        // If you incorporate this code into a DLL, be sure to demand FullTrust.
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        private static SafeTokenHandle GetSafeTokenHandle(string userName, string password)
        {
            SafeTokenHandle safeTokenHandle = null;
            // Get the user token for the specified user, domain, and password using the
            // unmanaged LogonUser method.
            // The local machine name can be used for the domain name to impersonate a user on this machine.
            const int LOGON32_PROVIDER_WINNT50 = 3;
            //This parameter causes LogonUser to create a primary token.
            const int LOGON32_LOGON_INTERACTIVE = 2;
            // Call LogonUser to obtain a handle to an access token.
            string domainName = "SZ1CARD1";
            bool returnValue = LogonUser(userName, domainName, password.FromDes(), LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_WINNT50, out safeTokenHandle);
            if (false == returnValue)
            {
                if (safeTokenHandle.DangerousGetHandle() != IntPtr.Zero)
                {
                    CloseHandle(safeTokenHandle.DangerousGetHandle());
                }
                int ret = Marshal.GetLastWin32Error();
                sz1card1.Common.Log.LoggingService.Warn(string.Format("域账号模拟失败：用户名：{0}，密码：{1}", userName, password.FromDes()));
                throw new System.ComponentModel.Win32Exception(ret);
            }
            return safeTokenHandle;
        }

        /// <summary>
        /// windows身份的模拟
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="password">密码(Des加密)</param>
        public static void Impersonate(IdentityUser user)
        {

            using (SafeTokenHandle safeTokenHandle = GetSafeTokenHandle(user.Name, user.Password))
            {
                IntPtr safeHandle = safeTokenHandle.DangerousGetHandle();
                WindowsImpersonationContext impersonatedUser = WindowsIdentity.Impersonate(safeHandle);
            }
        }

        /// <summary>
        /// 获取windows身份
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static WindowsIdentity GetWindowsIdentity(IdentityUser user)
        {
            using (SafeTokenHandle safeTokenHandle = GetSafeTokenHandle(user.Name, user.Password))
            {
                return new WindowsIdentity(safeTokenHandle.DangerousGetHandle());
            }
        }

        /// <summary>
        /// Web端Windows身份的模拟
        /// </summary>
        public static void WebImpersonate()
        {
            if (HttpContext.Current == null)
            {
                throw new NullReferenceException("HttpContext.Current 为 Null");
            }
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (authCookie == null)
            {
                throw new NullReferenceException("身份验证Cookie为空");
            }
            IdentityUser user = authCookie.Value.FromDes().FromJson<IdentityUser>();
            Impersonate(user);
        }

        /// <summary>
        /// 退出模拟
        /// </summary>
        public static void Exit()
        {
            RevertToSelf();
        }
    }
}
