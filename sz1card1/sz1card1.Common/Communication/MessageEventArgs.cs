using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Communication
{
    public class MessageEventArgs : EventArgs
    {
        private string identity;

        public MessageEventArgs(string identity)
        {
            this.identity = identity;
        }
        public string Identity
        {
            get
            {
                return identity;
            }
        }
    }
}
