using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class TrackingAttribute : Attribute
    {
        public string Description
        {
            set;
            get;
        }
    }
}
