using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// Abstract base class for all items of the toolbar.
    /// A toolbar item can be provided with different images
    /// to reflect its <see cref="ToolbarItemState"/> and
    /// supports hover images.
    /// </summary>
    public abstract class ToolbarItem : WebControl, INamingContainer
    {

        #region members

        /// <summary>
        /// The unique ID of the item. Can be used
        /// to determine the selected item on postback
        /// events.
        /// </summary>
        protected string m_itemId = String.Empty;

        #endregion


        #region properties

        /// <summary>
        /// The unique ID of the item. Can be used
        /// to determine the selected item on postback
        /// events.
        /// </summary>
        [Category("Toolbar")]
        [Description("A user-defined unique ID of the item.")]
        public string ItemId
        {
            get { return m_itemId; }
            set
            {
                this.m_itemId = value;
            }
        }


        /// <summary>
        /// Whether the item shall be rendered disabled or not. If this
        /// property is <c>true</c>, the disabled image is rendered
        /// rather than the standard one and no further functionality
        /// is available (tooltips, rollovers etc.).
        /// </summary>
        [NotifyParentProperty(true)]
        [Category("Toolbar")]
        [DefaultValue(false)]
        [Description("Whether the item should be rendered disabled or not.")]
        public virtual bool RenderDisabled
        {
            get
            {
                if (ViewState["Disabled"] == null)
                    return false;
                else
                    return (bool)ViewState["Disabled"];
            }
            set { ViewState["Disabled"] = value; }
        }


        /// <summary>
        /// The width (horizontal toolbar) or height (vertical toolbar)
        /// of a cell that contains a <see cref="ToolbarItem"/>.
        /// </summary>
        [Category("Toolbar")]
        [Description("Width or height of the item cell (depending on Toolbar orientation).")]
        [Localizable(true)]
        public Unit ItemCellDistance
        {
            get
            {
                object obj = this.ViewState["ItemCellWidth"];
                return obj == null ? Unit.Empty : (Unit)obj;
            }
            set { ViewState["ItemCellWidth"] = value; }
        }


        [NotifyParentProperty(true)]
        [Category("Toolbar")]
        [DefaultValue(HorizontalAlign.Center)]
        [Description("Horizontal alignment of the item within its cell.")]
        public HorizontalAlign HorizontalAlign
        {
            get
            {
                object obj = this.ViewState["HorizontalAlign"];
                return obj == null ? HorizontalAlign.Center : (HorizontalAlign)obj;
            }
            set { ViewState["HorizontalAlign"] = value; }
        }


        [NotifyParentProperty(true)]
        [Category("Toolbar")]
        [DefaultValue(VerticalAlign.Middle)]
        [Description("Vertical alignment of the item within its cell.")]
        public VerticalAlign VerticalAlign
        {
            get
            {
                object obj = this.ViewState["VerticalAlign"];
                return obj == null ? VerticalAlign.Middle : (VerticalAlign)obj;
            }
            set { ViewState["VerticalAlign"] = value; }
        }


        /// <summary>
        /// Determines whether the control is in design mode or not.
        /// </summary>
        protected bool IsDesignMode
        {
            get { return System.Web.HttpContext.Current == null; }
        }


        #endregion


        #region initialization

        /// <summary>
        /// Empty default constructor.
        /// </summary>
        public ToolbarItem()
        {
        }

        /// <summary>
        /// Constructs an item which is renderd as a specific
        /// HTML tag.
        /// </summary>
        /// <param name="tag">Html tag of the item control.</param>
        public ToolbarItem(HtmlTextWriterTag tag)
            : base(tag)
        {
        }

        #endregion

    }
}
