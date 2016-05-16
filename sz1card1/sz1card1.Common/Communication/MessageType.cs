using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Communication
{
    public enum MessageType
    {
        Request = 0x01,
        Response = 0x02,
        Push = 0x03,
        Exception=0x04
    }
}
