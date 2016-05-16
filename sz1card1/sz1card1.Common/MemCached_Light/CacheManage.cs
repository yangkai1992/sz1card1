using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using System.Security.Principal;
using sz1card1.Common.IdentityImpersonate;

namespace sz1card1.Common
{
    public class CacheManage
    {
        public static BusinessCache GetBusiness()
        {
            string key = string.Format("{0}_{1}", getIdentity(), "BusinessCache");
            BusinessCache oldValuexx = (BusinessCache)MemcachedLightContext.Client.Get(key);
            if (oldValuexx != null)
            {
                return oldValuexx;
            }
            else
            {
                sz1card1.Common.Log.LoggingService.Info("GetBusiness====Null");
                return null;
            }
        }
        public static void SetBusiness(BusinessCache value)
        {
            string key = string.Format("{0}_{1}", getIdentity(), "BusinessCache");
            BusinessCache oldValue = (BusinessCache)MemcachedLightContext.Client.Get(key);
            if (oldValue == null)
            {
                MemcachedLightContext.Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value);
            }
            else
            {
                if (value.Parameter != null)
                {
                    oldValue.Parameter = value.Parameter;
                }
                MemcachedLightContext.Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, oldValue);
            }
        }

        public static void SetUser(string userAccount, UserCache value)
        {
            string key = string.Format("{0}_{1}", getIdentity(), userAccount);
            UserCache userCache = (UserCache)MemcachedLightContext.Client.Get(key);
            if (userCache == null)
            {
                MemcachedLightContext.Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value);
            }
            else
            {

                if (value.ChainStoreGuid != null && !string.IsNullOrEmpty(value.ChainStoreGuid.ToString()))
                {
                    userCache.ChainStoreGuid = value.ChainStoreGuid;
                }
                else if (value.UserGroupType != null && !string.IsNullOrEmpty(value.UserGroupType.ToString()))
                {
                    userCache.UserGroupType = value.UserGroupType;
                }
                else if (value.UserGroupXml != null && !string.IsNullOrEmpty(value.UserGroupXml.ToString()))
                {
                    userCache.UserGroupXml = value.UserGroupXml;
                }
                MemcachedLightContext.Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, userCache);
            }
        }

        public static UserCache GetUser(string userAccount)
        {
            string key = string.Format("{0}_{1}", getIdentity(), userAccount);
            UserCache userCache = (UserCache)MemcachedLightContext.Client.Get(key);
            if (userCache != null)
            {
                return userCache;
            }
            else
            {
                sz1card1.Common.Log.LoggingService.Info("GetBusiness====Null");
                return null;
            }
        }

        private static string getIdentity()
        {
            IdentityUser user = IdentityUser.GetIdentityUser();
            if (user == null)
            {
                throw new ArgumentException("Windows身份标识为空");
            }
            Impersonator.Impersonate(user);
            return WindowsIdentity.GetCurrent().User.ToString();
        }
    }
}
