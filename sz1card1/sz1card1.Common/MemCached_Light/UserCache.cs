using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    [Serializable]
    public class UserCache
    {
        public Guid ChainStoreGuid { get; set; }

        public int? UserGroupType { get; set; }

        public string UserGroupXml { get; set; }
    }
}
