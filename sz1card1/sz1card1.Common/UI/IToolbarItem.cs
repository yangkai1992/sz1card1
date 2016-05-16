using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// Common interface of all items that can be plugged into
    /// the <see cref="Toolbar"/> component.
    /// </summary>
    public interface IToolbarItem
    {

        /// <summary>
        /// The unique ID of the item.
        /// </summary>
        string ItemId
        {
            get;
            set;
        }


        bool RenderDisabled
        {
            get;
            set;
        }

    }
}
