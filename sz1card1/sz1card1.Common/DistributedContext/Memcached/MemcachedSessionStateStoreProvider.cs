﻿using System;
using System.Configuration.Provider;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// This custom SessionState provider uses Memcached to store data; use it for very shortlived session data (two subsequent requests)
    /// </summary>
    public class MemcachedSessionStateStoreProvider : SessionStateStoreProviderBase, IDisposable
    {
        private string applicationName;
        private MemcachedClient memcachedClient;
        private const string sectionName = "sz1card1.memcached/memcachedsessionstate";
        private TimeSpan lockTimeout = new TimeSpan(0, 0, 0, 30);

        /// <summary>
        /// Initializes a new instance of the <see cref = "MemcachedSessionStateStoreProvider" /> class.
        /// </summary>
        public MemcachedSessionStateStoreProvider()
        {
            applicationName = "sz1card1";
            memcachedClient = HttpContext.Current.Application["MemcachedClient"] as MemcachedClient;
            if (memcachedClient == null)
            {
                memcachedClient = new MemcachedClient(sectionName);
                HttpContext.Current.Application.Add("MemcachedClient", memcachedClient);
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        /// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.</exception>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            applicationName = config.Get("applicationname") ?? "sz1card1";
            lockTimeout = new TimeSpan(0, 0, 0, 25);
            base.Initialize(name, config);
        }

        /// <summary>
        /// Sets a reference to the <see cref = "T:System.Web.SessionState.SessionStateItemExpireCallback" /> delegate for the Session_OnEnd event defined in the Global.asax file.
        /// </summary>
        /// <returns>
        /// true if the session-state store provider supports calling the Session_OnEnd event; otherwise, false.
        /// </returns>
        /// <param name = "expireCallback">The <see cref = "T:System.Web.SessionState.SessionStateItemExpireCallback" />  delegate for the Session_OnEnd event defined in the Global.asax file.</param>
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }

        /// <summary>
        /// Called by the <see cref = "T:System.Web.SessionState.SessionStateModule" /> object for per-request initialization.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        public override void InitializeRequest(HttpContext context)
        {
            //todo
        }

        /// <summary>
        /// Returns read-only session-state data from the session data store.
        /// </summary>
        /// <returns>
        /// A <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> populated with session values and information from the session data store.
        /// </returns>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The <see cref = "P:System.Web.SessionState.HttpSessionState.SessionID" /> for the current request.</param>
        /// <param name = "locked">When this method returns, contains a Boolean value that is set to true if the requested session item is locked at the session data store; otherwise, false.</param>
        /// <param name = "lockAge">When this method returns, contains a <see cref = "T:System.TimeSpan" /> object that is set to the amount of time that an item in the session data store has been locked.</param>
        /// <param name = "lockId">When this method returns, contains an object that is set to the lock identifier for the current request. For details on the lock identifier, see "Locking Session-Store Data" in the <see cref = "T:System.Web.SessionState.SessionStateStoreProviderBase" /> class summary.</param>
        /// <param name = "actions">When this method returns, contains one of the <see cref = "T:System.Web.SessionState.SessionStateActions" /> values, indicating whether the current session is an uninitialized, cookieless session.</param>
        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            actions = SessionStateActions.None;
            LockData lockData = GetLockStatus(id);//Check the lock status
            SessionStateDataContainer sessionStateDataContainer = memcachedClient.Get<SessionStateDataContainer>(id);
            locked = lockData.LockStatus == LockStatus.AlreadyLocked;

            lockId = lockData.LockId;
            lockAge = lockData.LockTime == DateTime.MinValue ? new TimeSpan(0) : DateTime.Now.Subtract(lockData.LockTime);

            SessionStateStoreData storeData = new SessionStateStoreData(sessionStateDataContainer.SessionStateItemCollection, context.Session.StaticObjects, sessionStateDataContainer.Timeout);
            return storeData;
        }

        private LockData GetLockStatus(string sessionId)
        {
            LockData lockData = memcachedClient.Get<LockData>(GetLockKey(sessionId));
            if (lockData == null)
            {
                lockData = new LockData { LockStatus = LockStatus.None };
            }
            return lockData;
        }

        /// <summary>
        /// Returns read-only session-state data from the session data store.
        /// </summary>
        /// <returns>
        /// A <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> populated with session values and information from the session data store.
        /// </returns>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The <see cref = "P:System.Web.SessionState.HttpSessionState.SessionID" /> for the current request.</param>
        /// <param name = "locked">When this method returns, contains a Boolean value that is set to true if a lock is successfully obtained; otherwise, false.</param>
        /// <param name = "lockAge">When this method returns, contains a <see cref = "T:System.TimeSpan" /> object that is set to the amount of time that an item in the session data store has been locked.</param>
        /// <param name = "lockId">When this method returns, contains an object that is set to the lock identifier for the current request. For details on the lock identifier, see "Locking Session-Store Data" in the <see cref = "T:System.Web.SessionState.SessionStateStoreProviderBase" /> class summary.</param>
        /// <param name = "actions">When this method returns, contains one of the <see cref = "T:System.Web.SessionState.SessionStateActions" /> values, indicating whether the current session is an uninitialized, cookieless session.</param>
        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            lockId = null;
            lockAge = new TimeSpan(0);
            actions = SessionStateActions.None;

            //first, always aquire lock
            LockData data = AquireLock(id);

            SessionStateDataContainer sessionStateDataContainer = memcachedClient.Get<SessionStateDataContainer>(id);

            if (sessionStateDataContainer == null)
            {
                //There is no data present, so release the lock
                ReleaseLock(id);
                locked = false;//no data present, failed to aquire lock
                return null;
            }

            sessionStateDataContainer.LockId = data.LockId;
            sessionStateDataContainer.LockTime = data.LockTime;

            //Create to object that should be returned
            SessionStateStoreData storeData = new SessionStateStoreData(sessionStateDataContainer.SessionStateItemCollection, SessionStateUtility.GetSessionStaticObjects(context), sessionStateDataContainer.Timeout);

            lockId = data.LockId;

            switch (data.LockStatus)
            {
                case LockStatus.AlreadyLocked:
                    //When the item is already locked, return and do not aquire lock)
                    lockAge = DateTime.Now.Subtract(data.LockTime);
                    locked = false; //no lock could be aquired
                    break;
                case LockStatus.LockAcquired:
                    //the lock was successfull
                    lockAge = DateTime.Now.Subtract(data.LockTime).Add(new TimeSpan(1));  //always add one tick                
                    locked = true;//Lock aquired      
                    break;
                default:
                    locked = false;
                    break;
            }
            return storeData;
        }

        private LockData AquireLock(string sessionId)
        {
            string lockKey = GetLockKey(sessionId);

            //set the lockstatus to already locked, so this will be stored if the store succeeds
            LockData lockData = new LockData { LockId = Guid.NewGuid(), LockStatus = LockStatus.AlreadyLocked, LockTime = DateTime.Now };

            if (!memcachedClient.Store(StoreMode.Add, lockKey, lockData, lockTimeout))
            {
                //when the lock could not be aquired, return the lock data
                return GetLockStatus(sessionId);
            }

            //Set to lockstatus to aquired.
            lockData.LockStatus = LockStatus.LockAcquired;

            return lockData;
        }

        private string GetLockKey(string sessionId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}_lock", sessionId, applicationName);
        }

        /// <summary>
        ///   Releases a lock on an item in the session data store.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The session identifier for the current request.</param>
        /// <param name = "lockId">The lock identifier for the current request. </param>
        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            ReleaseLock(id);
        }

        private void ReleaseLock(string sessionId)
        {
            string lockKey = GetLockKey(sessionId);
            memcachedClient.Remove(lockKey);
        }

        /// <summary>
        /// Updates the session-item information in the session-state data store with values from the current request, and clears the lock on the data.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The session identifier for the current request.</param>
        /// <param name = "item">The <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> object that contains the current session values to be stored.</param>
        /// <param name = "lockId">The lock identifier for the current request. </param>
        /// <param name = "newItem">true to identify the session item as a new item; false to identify the session item as an existing item.</param>
        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            AquireLock(id);
            try
            {
                SessionStateDataContainer container = new SessionStateDataContainer(item.Items, item.Timeout);
                if (newItem)
                {
                    bool storeResult = memcachedClient.Store(StoreMode.Add, id, container, TimeSpan.FromMinutes(item.Timeout));
                    if (!storeResult)
                    {
                        //this is not a new item
                        throw new ProviderException("This session wants to be stored as new. But a session with this key was already stored.");
                    }
                }
                else
                {
                    bool storeResult = memcachedClient.Store(StoreMode.Set, id, container, TimeSpan.FromMinutes(item.Timeout));
                }
            }
            finally
            {
                ReleaseLock(id);
            }
        }

        /// <summary>
        /// Deletes item data from the session data store.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The session identifier for the current request.</param>
        /// <param name = "lockId">The lock identifier for the current request.</param>
        /// <param name = "item">The <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> that represents the item to delete from the data store.</param>
        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            try
            {
                memcachedClient.Remove(id);//Throw provider exception when failed?
            }
            finally
            {
                ReleaseLock(id);
            }
        }

        /// <summary>
        /// Updates the expiration date and time of an item in the session data store.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The session identifier for the current request.</param>
        public override void ResetItemTimeout(HttpContext context, string id)
        {
            LockData aquireLock = AquireLock(id);
            try
            {
                if (aquireLock.LockStatus != LockStatus.AlreadyLocked)
                {
                    SessionStateDataContainer sessionStateDataContainer = memcachedClient.Get<SessionStateDataContainer>(id);
                    if (sessionStateDataContainer != null)
                    {
                        bool res = memcachedClient.Store(StoreMode.Add, id, sessionStateDataContainer, TimeSpan.FromMinutes(sessionStateDataContainer.Timeout));
                    }
                }
            }
            finally
            {
                if (aquireLock.LockStatus == LockStatus.LockAcquired)
                {
                    ReleaseLock(id);
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> object to be used for the current request.
        /// </summary>
        /// <returns>
        /// A new <see cref = "T:System.Web.SessionState.SessionStateItemCollection" /> for the current request.
        /// </returns>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "timeout">The session-state <see cref = "P:System.Web.SessionState.HttpSessionState.Timeout" /> value for the new <see cref = "T:System.Web.SessionState.SessionStateItemCollection" />.</param>
        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            SessionStateStoreData sessionStateStoreData = new SessionStateStoreData(new SerializableSessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), timeout);
            return sessionStateStoreData;
        }

        /// <summary>
        /// Adds a new session-state item to the data store.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        /// <param name = "id">The <see cref = "P:System.Web.SessionState.HttpSessionState.SessionID" /> for the current request.</param>
        /// <param name = "timeout">The session <see cref = "P:System.Web.SessionState.HttpSessionState.Timeout" /> for the current request.</param>
        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            SessionStateDataContainer container = new SessionStateDataContainer(new SerializableSessionStateItemCollection(), timeout);

            if (!memcachedClient.Store(StoreMode.Set, id, container, TimeSpan.FromMinutes(timeout)))
            {
                throw new ProviderException("Unable to create new session in CreateUninitializedItem.");
            }
        }

        /// <summary>
        /// Called by the <see cref = "T:System.Web.SessionState.SessionStateModule" /> object at the end of a request.
        /// </summary>
        /// <param name = "context">The <see cref = "T:System.Web.HttpContext" /> for the current request.</param>
        public override void EndRequest(HttpContext context)
        {

        }

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by the <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/> implementation.
        /// </summary>
        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // The bulk of the clean-up code is implemented in this method
        }
        #endregion
    }

}
