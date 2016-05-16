using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security;
using sz1card1.Common.IdentityImpersonate;

namespace sz1card1.Common.Distributed
{
    public class RedisWinIdentityDataState:RedisDataState
    {
        public RedisWinIdentityDataState()
            : base()
        {
            IdentityUser user = IdentityUser.GetIdentityUser();
            if (user == null)
            {
                throw new ArgumentException("Windows身份标识为空");
            }
            Impersonator.Impersonate(user);
            base.identity = WindowsIdentity.GetCurrent().User.ToString();
            base.timeout = 240;
            base.Initialize();
        }

        public override object this[string name]
        {
            get
            {
                base.ResetTimeout();
                return base[name];
            }
            set
            {
                base.ResetTimeout();
                base[name] = value;
            }
        }
    }
}
