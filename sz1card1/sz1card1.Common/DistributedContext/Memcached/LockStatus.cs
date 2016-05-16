using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// Status of a lock
    /// </summary>
    public enum LockStatus
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Item is already locked.
        /// </summary>
        AlreadyLocked,
        /// <summary>
        /// Lock is aquired successful.
        /// </summary>
        LockAcquired
    }

}
