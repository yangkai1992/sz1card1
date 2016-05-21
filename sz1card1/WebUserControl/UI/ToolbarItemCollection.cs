using System;
using System.Collections;
using System.Collections.Specialized;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using sz1card1.Common.Delegate;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// Maintains a collection of <see cref="TollbarItem"/>
    /// objects.
    /// </summary>
    [Editor(typeof(ItemCollectionEditor), typeof(UITypeEditor))]
    public class ToolbarItemCollection : CollectionBase
    {

        /// <summary>
        /// Raised if an item is was added.
        /// </summary>
        public event ItemEventHandler ItemAdded;

        /// <summary>
        /// Raised if an item was removed.
        /// </summary>
        public event ItemEventHandler ItemRemoved;

        /// <summary>
        /// Raised if the controls were removed.
        /// </summary>
        public event EventHandler ItemsCleared;


        public ToolbarItemCollection()
        {
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (ItemRemoved != null) ItemRemoved(value as ToolbarItem);

            base.OnRemoveComplete(index, value);
        }


        protected override void OnInsertComplete(int index, object value)
        {
            if (ItemAdded != null) ItemAdded(value as ToolbarItem);

            base.OnInsertComplete(index, value);
        }

        protected override void OnClear()
        {
            if (ItemsCleared != null) ItemsCleared(this, null);
            base.OnClear();
        }




        /// <summary>
        /// Gets a toolbar item by its unique
        /// item ID.
        /// </summary>
        public ToolbarItem this[string itemId]
        {
            get
            {
                foreach (ToolbarItem item in this.List)
                {
                    if (item.ItemId == itemId) return item;
                }

                return null;
            }
        }


        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(ToolbarItem item)
        {
            if (item.ItemId == String.Empty) item.ItemId = item.ID;
            this.List.Add(item);
        }



        /// <summary>
        /// Adds an item at a given position.
        /// </summary>
        /// <param name="index">Zero-based index of the item.</param>
        /// <param name="item">Item to be added.</param>
        public void Insert(int index, ToolbarItem item)
        {
            this.List.Insert(index, item);
        }


        /// <summary>
        /// Gets the index of a toolbar item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(ToolbarItem item)
        {
            return this.List.IndexOf(item);
        }


        public int IndexOf(string itemId)
        {
            ToolbarItem item = this[itemId];
            if (item == null)
            {
                return -1;
            }
            else
            {
                return this.IndexOf(item);
            }
        }



        /// <summary>
        /// Removes an item from the toolbar.
        /// </summary>
        /// <param name="itemId">The unique
        /// <see cref="ToolbarItem.ItemId"/> of the
        /// item.</param>
        public void RemoveItem(string itemId)
        {
            ToolbarItem item = this[itemId];
            if (item != null) this.List.Remove(item);
        }


    }
}
