
namespace WebUserControl.UI
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
