using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security;
using sz1card1.Common.IdentityImpersonate;

namespace sz1card1.Common.Distributed
{
    public class RedisApplicationDataState:RedisDataState
    {
        public RedisApplicationDataState()
            : base()
        {
            base.timeout = 240;
        }

        public override object this[string name]
        {
            get
            {
                base.provider.SetHashExpire(name,base.timeout);
                return base.provider.HashGetItem(name, "value");
            }
            set
            {
                base.provider.HashSetItem(name, "value", value);
                base.provider.SetHashExpire(name,base.timeout);
            }
        }
    }
}
