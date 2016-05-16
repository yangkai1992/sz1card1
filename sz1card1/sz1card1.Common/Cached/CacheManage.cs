//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Enyim.Caching;
//using System.Security.Principal;
//using sz1card1.Common.IdentityImpersonate;
//using Enyim.Caching.Memcached;
//using System.Data.Linq;
//using sz1card1.Common.Redis;
//using System.Runtime.Serialization;

//namespace sz1card1.Common
//{
//    public class CacheManage
//    {
//        private static readonly string section = "sz1card1.memcached/MemcachedWinIdentityDataState";
//        private static MemcachedClient client;
//        private static readonly object lockObj = new object();


//        public static MemcachedClient Client
//        {
//            get
//            {
//                if (client == null)
//                {
//                    lock (lockObj)
//                    {
//                        //client = new MemcachedClient(section);
//                        client = new MemcachedClient();
//                    }
//                }
//                return client;
//            }
//        }
//        private static string getIdentity()
//        {
//            IdentityUser user = IdentityUser.GetIdentityUser();
//            if (user == null)
//            {
//                throw new ArgumentException("Windows身份标识为空");
//            }
//            Impersonator.Impersonate(user);
//            return WindowsIdentity.GetCurrent().User.ToString();
//        }

//        public static void SetBusinessCache<T>(string keyName, T t)
//        {
//            string key = string.Format("{0}_{1}", getIdentity(), keyName);
//            Client.Store(StoreMode.Set, key, t);
//        }


//        /// <summary>
//        ///  缓存失效
//        /// </summary>
//        /// <param name="keyName"></param>
//        public static void RemoveBusinessCache(string keyName)
//        {
//            string key = string.Format("{0}_{1}", getIdentity(), keyName);
//            Client.Remove(key);
//        }
//        public static T GetBusinessCache<T>(string keyName)
//        {
//            string key = string.Format("{0}_{1}", getIdentity(), keyName);
//            return (T)Client.Get(key);
//        }

//        public static void SetChainStoreCache<T>(ChainStoreCacheKeys keyName, T t)
//        {
//            string key = string.Empty;
//            switch (keyName)
//            {
//                case ChainStoreCacheKeys.BirthdayDiscountRate:
//                    key = "BirthdayDiscountRate";
//                    break;
//                case ChainStoreCacheKeys.BirthdayPointRate:
//                    key = "BirthdayPointRate";
//                    break;
//                case ChainStoreCacheKeys.ChainStoreGuid:
//                    key = "ChainStoreGuid";
//                    break;
//                case ChainStoreCacheKeys.ChainStoreName:
//                    key = "ChainStoreName";
//                    break;
//                case ChainStoreCacheKeys.EnableBillRules:
//                    key = "EnableBillRules";
//                    break;
//                case ChainStoreCacheKeys.EnableGoodsItemDiscount:
//                    key = "EnableGoodsItemDiscount";
//                    break;
//                case ChainStoreCacheKeys.EnableGoodsItemPoint:
//                    key = "EnableGoodsItemPoint";
//                    break;
//                case ChainStoreCacheKeys.EnableMemberGroupDiscount:
//                    key = "EnableMemberGroupDiscount";
//                    break;
//                case ChainStoreCacheKeys.EnableMemberGroupPoint:
//                    key = "EnableMemberGroupPoint";
//                    break;
//                case ChainStoreCacheKeys.EnableTimelyRules:
//                    key = "EnableTimelyRules";
//                    break;
//                case ChainStoreCacheKeys.StoreDiscountRate:
//                    key = "StoreDiscountRate";
//                    break;
//                case ChainStoreCacheKeys.StorePointRate:
//                    key = "StorePointRate";
//                    break;
//            }
//            key = string.Format("{0}_{1}", SessionManager.GetSessionUser().ChainStoreGuid, keyName);
//            Client.Store(StoreMode.Set, key, t);
//        }


//        public static T GetChainStoreCache<T>(ChainStoreCacheKeys keyName)
//        {
//            string key = string.Empty;
//            switch (keyName)
//            {
//                case ChainStoreCacheKeys.BirthdayDiscountRate:
//                    key = "BirthdayDiscountRate";
//                    break;
//                case ChainStoreCacheKeys.BirthdayPointRate:
//                    key = "BirthdayPointRate";
//                    break;
//                case ChainStoreCacheKeys.ChainStoreGuid:
//                    key = "ChainStoreGuid";
//                    break;
//                case ChainStoreCacheKeys.ChainStoreName:
//                    key = "ChainStoreName";
//                    break;
//                case ChainStoreCacheKeys.EnableBillRules:
//                    key = "EnableBillRules";
//                    break;
//                case ChainStoreCacheKeys.EnableGoodsItemDiscount:
//                    key = "EnableGoodsItemDiscount";
//                    break;
//                case ChainStoreCacheKeys.EnableGoodsItemPoint:
//                    key = "EnableGoodsItemPoint";
//                    break;
//                case ChainStoreCacheKeys.EnableMemberGroupDiscount:
//                    key = "EnableMemberGroupDiscount";
//                    break;
//                case ChainStoreCacheKeys.EnableMemberGroupPoint:
//                    key = "EnableMemberGroupPoint";
//                    break;
//                case ChainStoreCacheKeys.EnableTimelyRules:
//                    key = "EnableTimelyRules";
//                    break;
//                case ChainStoreCacheKeys.StoreDiscountRate:
//                    key = "StoreDiscountRate";
//                    break;
//                case ChainStoreCacheKeys.StorePointRate:
//                    key = "StorePointRate";
//                    break;
//            }
//            key = string.Format("{0}_{1}", SessionManager.GetSessionUser().ChainStoreGuid, keyName);
//            return (T)Client.Get(key);
//        }
//        /// <summary>
//        /// 缓存失效
//        /// </summary>
//        /// <param name="keyName"></param>
//        public static void RemoveChainStoreCache(ChainStoreCacheKeys keyName)
//        {
//            string key = string.Empty;
//            switch (keyName)
//            {
//                case ChainStoreCacheKeys.BirthdayDiscountRate:
//                    key = "BirthdayDiscountRate";
//                    break;
//                case ChainStoreCacheKeys.BirthdayPointRate:
//                    key = "BirthdayPointRate";
//                    break;
//                case ChainStoreCacheKeys.ChainStoreGuid:
//                    key = "ChainStoreGuid";
//                    break;
//                case ChainStoreCacheKeys.ChainStoreName:
//                    key = "ChainStoreName";
//                    break;
//                case ChainStoreCacheKeys.EnableBillRules:
//                    key = "EnableBillRules";
//                    break;
//                case ChainStoreCacheKeys.EnableGoodsItemDiscount:
//                    key = "EnableGoodsItemDiscount";
//                    break;
//                case ChainStoreCacheKeys.EnableGoodsItemPoint:
//                    key = "EnableGoodsItemPoint";
//                    break;
//                case ChainStoreCacheKeys.EnableMemberGroupDiscount:
//                    key = "EnableMemberGroupDiscount";
//                    break;
//                case ChainStoreCacheKeys.EnableMemberGroupPoint:
//                    key = "EnableMemberGroupPoint";
//                    break;
//                case ChainStoreCacheKeys.EnableTimelyRules:
//                    key = "EnableTimelyRules";
//                    break;
//                case ChainStoreCacheKeys.StoreDiscountRate:
//                    key = "StoreDiscountRate";
//                    break;
//                case ChainStoreCacheKeys.StorePointRate:
//                    key = "StorePointRate";
//                    break;
//            }
//            key = string.Format("{0}_{1}", SessionManager.GetSessionUser().ChainStoreGuid, keyName);
//            Client.Remove(key);
//        }

//        public static void SetUserCache<T>(UserCacheKeys keyName, T t)
//        {
//            string key = string.Empty;
//            switch (keyName)
//            {
//                case UserCacheKeys.ChainStoreGuid:
//                    key = "ChainStoreGuid";
//                    break;
//                case UserCacheKeys.UserChainStoreXml:
//                    key = "UserChainStoreXml";
//                    break;
//                case UserCacheKeys.UserGroupPermitionXml:
//                    key = "UserGroupPermitionXml";
//                    break;
//                case UserCacheKeys.UserName:
//                    key = "UserName";
//                    break;
//                case UserCacheKeys.UserGroupType:
//                    key = "UserGroupType";
//                    break;
//                default:
//                    throw new ArgumentException("GetUserCache,Key不能为空");
//            }
//            key = string.Format("{0}_{1}", SessionManager.GetSessionUser().UserGuid, keyName);
//            Client.Store(StoreMode.Set, key, t);
//        }
//        public static T GetUserCache<T>(UserCacheKeys keyName)
//        {
//            string key = string.Empty;
//            switch (keyName)
//            {
//                case UserCacheKeys.ChainStoreGuid:
//                    key = "ChainStoreGuid";
//                    break;
//                case UserCacheKeys.UserChainStoreXml:
//                    key = "UserChainStoreXml";
//                    break;
//                case UserCacheKeys.UserGroupPermitionXml:
//                    key = "UserGroupPermitionXml";
//                    break;
//                case UserCacheKeys.UserName:
//                    key = "UserName";
//                    break;
//                case UserCacheKeys.UserGroupType:
//                    key = "UserGroupType";
//                    break;
//                default:
//                    throw new ArgumentException("GetUserCache,Key不能为空");
//            }
//            key = string.Format("{0}_{1}", SessionManager.GetSessionUser().UserGuid, keyName);
//            return (T)Client.Get(key);
//        }
//        /// <summary>
//        /// 缓存失效
//        /// </summary>
//        /// <param name="keyName"></param>
//        public static void RemoveUserCache(UserCacheKeys keyName)
//        {
//            string key = string.Empty;
//            switch (keyName)
//            {
//                case UserCacheKeys.ChainStoreGuid:
//                    key = "ChainStoreGuid";
//                    break;
//                case UserCacheKeys.UserChainStoreXml:
//                    key = "UserChainStoreXml";
//                    break;
//                case UserCacheKeys.UserGroupPermitionXml:
//                    key = "UserGroupPermitionXml";
//                    break;
//                case UserCacheKeys.UserName:
//                    key = "UserName";
//                    break;
//                case UserCacheKeys.UserGroupType:
//                    key = "UserGroupType";
//                    break;
//                default:
//                    throw new ArgumentException("GetUserCache,Key不能为空");
//            }
//            key = string.Format("{0}_{1}", SessionManager.GetSessionUser().UserGuid, keyName);
//            Client.Remove(key);
//        }
//    }
//}
