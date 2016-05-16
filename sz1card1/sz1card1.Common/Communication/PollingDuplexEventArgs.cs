using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Communication
{
    public class PollingDuplexEventArgs : EventArgs
    {
        public IClient Client { get; set; }
        public PollingMessage Message { get; set; }
    }
}
