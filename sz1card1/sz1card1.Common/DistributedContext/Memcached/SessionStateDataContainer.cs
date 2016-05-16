﻿using System;
using System.Runtime.Serialization;
using System.Web.SessionState;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// Wrapper object to put sessionstate in memcached.
    /// </summary>
    [Serializable]
    public class SessionStateDataContainer : ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "SessionStateDataContainer" /> class.
        /// </summary>
        /// <param name = "info">The info.</param>
        /// <param name = "context">The context.</param>
        protected SessionStateDataContainer(SerializationInfo info, StreamingContext context)
        {
            SessionStateItemCollection = (ISessionStateItemCollection)info.GetValue("SessionStateItemCollection", typeof(SerializableSessionStateItemCollection));
            Timeout = info.GetInt32("Timeout");
            IsLocked = info.GetBoolean("IsLocked");
            LockId = (Guid)info.GetValue("LockId", typeof(Guid));
            LockTime = (DateTime)info.GetValue("LockTime", typeof(DateTime));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SessionStateDataContainer" /> class.
        /// </summary>
        /// <param name = "sessionStateItemCollection">The session state store data.</param>
        /// <param name = "timeout">The timeout.</param>
        public SessionStateDataContainer(ISessionStateItemCollection sessionStateItemCollection, int timeout)
        {
            SessionStateItemCollection = sessionStateItemCollection;
            Timeout = timeout;
        }

        /// <summary>
        /// Gets or sets the session state store data.
        /// </summary>
        /// <value>The session state store data.</value>
        public ISessionStateItemCollection SessionStateItemCollection { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>        
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is locked.
        /// </summary>
        /// <value><c>true</c> if this instance is locked; otherwise, <c>false</c>.</value>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the lock id.
        /// </summary>
        /// <value>The lock id.</value>
        public Guid LockId { get; set; }

        /// <summary>
        /// Gets or sets the lock time.
        /// </summary>
        /// <value>The lock time.</value>
        public DateTime LockTime { get; set; }

        #region ISerializable Members
        /// <summary>
        /// Populates a <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
        /// <param name = "context">The destination (see <see cref = "T:System.Runtime.Serialization.StreamingContext" />) for this serialization. </param>
        /// <exception cref = "T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SessionStateItemCollection", SessionStateItemCollection);
            info.AddValue("Timeout", Timeout);
            info.AddValue("IsLocked", IsLocked);
            info.AddValue("LockId", LockId);
            info.AddValue("LockTime", LockTime);
        }
        #endregion
    }

}
