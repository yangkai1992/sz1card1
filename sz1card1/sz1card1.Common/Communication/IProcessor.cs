using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Communication
{
    public interface IProcessor
    {
        void Process(PollingMessage message,IClient client);
    }
}
