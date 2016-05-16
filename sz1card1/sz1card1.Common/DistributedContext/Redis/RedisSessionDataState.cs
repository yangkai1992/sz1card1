using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Web.SessionState;
using System.Runtime.Remoting.Messaging;

namespace sz1card1.Common.Distributed
{
    public class RedisSessionDataState : RedisDataState
    {
        private Dictionary<string, object> _items;

        public Dictionary<string, object> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = (Dictionary<string, object>)CallContext.GetData("RedisSessionDataState_" + base.identity);
                    if (_items == null)
                    {
                        _items = new Dictionary<string, object>();
                        foreach (KeyValuePair<string, object> item in this)
                        {
                            _items.Add(item.Key, item.Value);
                        }
                        CallContext.SetData("RedisSessionDataState_" + base.identity, _items);
                    }
                }
                return _items;
            }
        }

        public RedisSessionDataState(): base()
        {
            if (HttpContext.Current == null)
            {
                if (string.IsNullOrEmpty(DistributedContext.Current.ContextID))
                {
                    throw new NullReferenceException("DistributedContext.ContextID不能为空!");
                }
                base.identity = DistributedContext.Current.ContextID;
                base.timeout = 240;

                Initialize();
            }
            else
            {
                base.identity = HttpContext.Current.Session.SessionID;
                base.timeout = HttpContext.Current.Session.Timeout;
            }
        }

        public RedisSessionDataState(string identity, int timeout)
            : base()
        {
            base.identity = identity;
            base.timeout = timeout;
        }

        public override int Timeout
        {
            get { return base.Timeout; }
            set
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Timeout = value;
                }
                base.Timeout = value;
            }
        }

        public override object this[string name]
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    if (Items.ContainsKey(name))
                    {
                        return Items[name];
                    }
                    else
                    {
                        return null;
                    }
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
                    Items[name] = value;
                }
                else
                {
                    HttpContext.Current.Session[name] = value;
                }
            }
        }

        /// <summary>
        /// 持久化到redis
        /// </summary>
        public override void Persistent()
        {
            foreach(var keyPair in Items)
            {
                base[keyPair.Key] = keyPair.Value;
            }
            base.ResetTimeout();
        }

        /// <summary>
        /// 通过标示符获取实例
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static RedisSessionDataState GetInstance(string identity)
        {
            using (var redisClient = new RedisStoreProvider(typeof(RedisSessionDataState).Name).RedisClient)
            {
                if (!redisClient.ContainsKey(identity)) return null;
                string value = redisClient.GetValueFromHash(identity, RedisStoreProvider.DataKey_Timeout);
                if (value == null) return null;
                var timeout = int.Parse(value);
                return new RedisSessionDataState(identity, timeout);
            }
        }
    }
}
