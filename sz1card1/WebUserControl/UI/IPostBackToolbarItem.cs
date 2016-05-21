using WebUserControl.Delegate;

namespace WebUserControl.UI
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
