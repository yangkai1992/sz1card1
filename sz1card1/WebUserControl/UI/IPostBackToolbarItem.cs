using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Delegate;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// Common interface of all toolbar items that
    /// provide postback capabilities.
    /// </summary>
    public interface IPostBackToolbarItem
    {

        /// <summary>
        /// Event which fires if the item is being
        /// submitted.
        /// </summary>
        event ItemEventHandler ItemSubmitted;

    }
}
