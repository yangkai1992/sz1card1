using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// 会话缓存数据对象
    /// </summary>
    public class MemcachedSessionDataState : MemcachedDataState
    {
        public MemcachedSessionDataState()
            : base()
        {
            timeout = 240;
            if (HttpContext.Current == null)
            {
                if (string.IsNullOrEmpty(DistributedContext.Current.ContextID))
                {
                    throw new NullReferenceException("MemcachedContext.ContextID不能为空!");
                }
                identity = DistributedContext.Current.ContextID;
            }
            else
            {
                identity = HttpContext.Current.Session.SessionID;
            }
        }

        public override object this[string name]
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    DataCollection dataCollection = provider.GetItem(identity);
                    if (dataCollection == null)
                    {
                        return null;
                    }
                    ResetItemTimeout(identity);
                    return dataCollection[name];
                }
                else
                {
                    return HttpContext.Current.Session[name];
                }
            }
            set
            {
                if (HttpContext.Current == null)
                {
                    DataCollection dataCollection = provider.GetItem(identity);
                    if (dataCollection == null)
                    {
                        dataCollection = new DataCollection(timeout);
                        dataCollection.Add(name, value);
                    }
                    else
                    {
                        dataCollection[name] = value;
                    }
                    provider.SetItem(identity, dataCollection);
                    lastOperationTime = DateTime.Now;
                }
                else
                {
                    HttpContext.Current.Session[name] = value;
                }
            }
        }
    }
}
