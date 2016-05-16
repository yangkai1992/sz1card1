using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// Lock data
    /// </summary>
    [Serializable]
    public class LockData : ISerializable
    {
        /// <summary>
        /// Gets or sets the lock status.
        /// </summary>
        /// <value>The lock status.</value>
        public LockStatus LockStatus { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="LockData"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected LockData(SerializationInfo info, StreamingContext context)
        {
            LockId = (Guid)info.GetValue("LockId", typeof(Guid));
            LockTime = (DateTime)info.GetValue("LockTime", typeof(DateTime));
            LockStatus = (LockStatus)info.GetValue("LockStatus", typeof(LockStatus));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockData"/> class.
        /// </summary>
        public LockData()
        {
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LockId", LockId);
            info.AddValue("LockTime", LockTime);
            info.AddValue("LockStatus", LockStatus);
        }
    }

}
