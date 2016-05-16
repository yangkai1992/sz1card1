using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;

namespace sz1card1.Common.Distributed
{
    public class RedisSessionStateStoreProvider : SessionStateStoreProviderBase
    {
        public static readonly string TimeoutKey = "_timeout";

        public static readonly string SessionIdPrefixKey = "_sessionIdPrefix";

        private string _sessionIdPrefix;

        private string GetSessionId(string sessionId)
        {
            if (string.IsNullOrEmpty(_sessionIdPrefix)) return sessionId;
            return _sessionIdPrefix + "_" + sessionId;
        }

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return CreateLegitStoreData(context, null, null, timeout);
        }

        public override void CreateUninitializedItem(HttpContext context, string sessionId, int timeout)
        {
        }

        private static SessionStateStoreData CreateLegitStoreData(HttpContext context, ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout)
        {
            if (sessionItems == null)
                sessionItems = new SessionStateItemCollection();
            if (staticObjects == null && context != null)
                staticObjects = SessionStateUtility.GetSessionStaticObjects(context);
            return new SessionStateStoreData(sessionItems, staticObjects, timeout);
        }

        private SessionStateStoreData GetStoreData(HttpContext context, string sessionId, bool exclusive, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
        {
            locked = false;
            lockId = null;
            lockAge = TimeSpan.Zero;
            actionFlags = SessionStateActions.None;
            var redisSessionDataState = RedisSessionDataState.GetInstance(GetSessionId(sessionId));
            if (redisSessionDataState == null)
            {
                return null;
            }
            var timeout = redisSessionDataState.Timeout;
            var sessionItems = new SessionStateItemCollection();
            foreach (KeyValuePair<string, object> item in redisSessionDataState.Items)
            {
                sessionItems[item.Key] = item.Value;
            }
            return CreateLegitStoreData(context, sessionItems, null, timeout);
        }

        public override SessionStateStoreData GetItem(HttpContext context, string sessionId, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
        {
            return GetStoreData(context, sessionId, false, out locked, out lockAge, out lockId, out actionFlags);
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string sessionId, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
        {
            return GetStoreData(context, sessionId, true, out locked, out lockAge, out lockId, out actionFlags);
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string sessionId, SessionStateStoreData storeData,
            object lockId, bool newItem)
        {
            //第一次遍历，获取_sessionIdPrefix
            for (int i = 0; i < storeData.Items.Keys.Count; i++)
            {
                if (storeData.Items.Keys[i] == SessionIdPrefixKey)
                {
                    _sessionIdPrefix = storeData.Items[i].ToString();
                    storeData.Items.RemoveAt(i);//不产生真实dataKey
                    break;
                }
            }
            var redisSessionDataState = new RedisSessionDataState(GetSessionId(sessionId), storeData.Timeout);
            if (newItem)
            {
                redisSessionDataState.Initialize();
            }
            var keys = storeData.Items.Keys;
            //赋新值
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                redisSessionDataState.Items[key] = storeData.Items[key];
            }
            //清空被删除的key
            var keys2 = redisSessionDataState.Items.Keys.ToList();
            for (int i = 0; i < keys2.Count; i++)
            {
                if (storeData.Items[keys2[i]] == null)
                {
                    redisSessionDataState.Items[keys2[i]] = null;
                }

            }
            //写入到redis
            redisSessionDataState.Persistent();
        }

        public override void RemoveItem(HttpContext context, string sessionId, object lockId, SessionStateStoreData item)
        {
            var redisSessionDataState = RedisSessionDataState.GetInstance(GetSessionId(sessionId));
            if (redisSessionDataState == null)
            {
                return;
            }
            redisSessionDataState.Clear();
        }

        public override void ResetItemTimeout(HttpContext context, string sessionId)
        {
            var redisSessionDataState = RedisSessionDataState.GetInstance(GetSessionId(sessionId));
            if (redisSessionDataState == null)
            {
                return;
            }
            redisSessionDataState.ResetTimeout();
        }


        #region "未实现方法"

        public override void Dispose()
        {
        }

        public override void EndRequest(HttpContext context)
        {
        }

        public override void InitializeRequest(HttpContext context)
        {
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            ResetItemTimeout(context, id);
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return true;
        }

        #endregion
    }
}
