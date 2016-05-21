
namespace WebUserControl.UI
{
    /// <summary>
    /// Common interface of all items that provide
    /// an image and optional rollovers.
    /// </summary>
    public interface IImageItem
    {
        /// <summary>
        /// The image to be rendered by the item.
        /// </summary>
        string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// An optional image which is displayed if
        /// the item's state is set to
        /// <see cref="ToolbarItemState.Disabled"/>.
        /// If not set, the standard image is being used.
        /// </summary>
        string DisabledImageUrl
        {
            get;
            set;
        }


        /// <summary>
        /// An optional image which is displayed if
        /// the user moves the mouse over the item.
        /// If not set, the standard image is being used.
        /// </summary>
        string RollOverImageUrl
        {
            get;
            set;
        }

    }
}
