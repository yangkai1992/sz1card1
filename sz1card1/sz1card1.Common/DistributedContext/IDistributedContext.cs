using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    public interface IDistributedContext
    {
        IDistributedContext Current { get; }

        string ContextID { get; }

        IDataState Session { get; }

        IDataState WinIdentity { get; }

        IDataState Application { get; }
    }
}
