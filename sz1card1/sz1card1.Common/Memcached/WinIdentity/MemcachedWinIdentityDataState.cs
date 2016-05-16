using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security;
using System.Threading;
using sz1card1.Common.IdentityImpersonate;

namespace sz1card1.Common.Memcached
{
    /// <summary>
    /// Windows账号缓存数据对象
    /// </summary>
    public class MemcachedWinIdentityDataState : MemcachedDataState
    {
        public MemcachedWinIdentityDataState()
            : base()
        {
            IdentityUser user = IdentityUser.GetIdentityUser();
            if (user == null)
            {
                throw new ArgumentException("Windows身份标识为空");
            }
            Impersonator.Impersonate(user);
            identity = WindowsIdentity.GetCurrent().User.ToString();
            timeout = 240;
        }

        public override object this[string name]
        {
            get
            {
                string key = string.Format("{0}_{1}", identity, name);
                DataCollection dataCollection = provider.GetItem(key);
                if (dataCollection == null)
                {
                    return null;
                }
                ResetItemTimeout(key);
                return dataCollection[key];
            }
            set
            {
                lastOperationTime = DateTime.Now;
                string key = string.Format("{0}_{1}", identity, name);
                DataCollection dataCollection = new DataCollection(timeout);
                dataCollection[key] = value;
                provider.SetItem(key, dataCollection);
            }
        }
    }
}
