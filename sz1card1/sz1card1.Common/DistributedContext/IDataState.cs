using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web.SessionState;

namespace sz1card1.Common
{
    public interface IDataState : ICollection, IEnumerable
    {
        string Identity { get; }

        int Timeout { get; set; }

        void Persistent();

        object this[int index] { get; set; }

        object this[string name] { get; set; }

        void Clear();

        void Remove(string name);
    }
}
