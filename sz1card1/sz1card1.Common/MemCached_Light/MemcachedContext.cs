using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    public class MemcachedLightContext
    {
        private static readonly string section = "sz1card1.memcached/MemcachedWinIdentityDataState";
        private static MemcachedClient client;
        private static readonly object lockObj = new object();


        public static MemcachedClient Client
        {
            get
            {
                if (client == null)
                {
                    lock (lockObj)
                    {
                        client = new MemcachedClient(section);
                    }
                }
                return client;
            }
        }
    }
}
