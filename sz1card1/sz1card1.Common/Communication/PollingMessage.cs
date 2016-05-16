using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sz1card1.Common.Communication
{
    [DataContract(Namespace="sz1card1")]
    public class PollingMessage
    {
        [DataMember]
        public int Id
        {
            get;
            set;
        }

        [DataMember]
        public string UserName
        {
            get;
            set;
        }

        [DataMember]
        public int Type
        {
            get;
            set;
        }

        [DataMember]
        public string Message
        {
            get;
            set;
        }
    }
}
