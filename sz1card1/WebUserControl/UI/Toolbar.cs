using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing.Design;
using sz1card1.Common.Enum;
using sz1card1.Common.Delegate;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// Renders a toolbar control which acts as a container
    /// for <see cref="ToolbarItem"/> objects.
    /// </summary>
    [ParseChildren(true, "Items")]
    [Designer(typeof(ToolbarDesigner))]
    [ToolboxData("<{0}:Toolbar runat=server></{0}:Toolbar>")]
    [DefaultEvent("ItemPostBack")]
    public class Toolbar : WebControl, INamingContainer
    {

        #region members

        /// <summary>
        /// Contains the items which were assigned to the toolbar.
        /// </summary>
        protected ToolbarItemCollection m_items;

        #endregion


        #region properties

        /// <summary>
        /// Contains the items which were assigned to the toolbar.
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        [Category("Toolbar")]
        [Description("Items of the toolbar")]
        public ToolbarItemCollection Items
        {
            get { return this.m_items; }
        }


        /// <summary>
        /// Orientation of the toolbar.
        /// </summary>
        [Category("Toolbar")]
        [DefaultValue(ToolbarOrientation.Horizontal)]
        [Description("Orientation of the toolbar")]
        public ToolbarOrientation Orientation
        {
            get
            {
                object obj = this.ViewState["Orientation"];
                return obj == null ? ToolbarOrientation.Horizontal : (ToolbarOrientation)obj;
            }
            set { ViewState["Orientation"] = value; }
        }


        /// <summary>
        /// The default width (horizontal toolbar) or height (vertical orientation)
        /// of all toolbar item cells.
        /// </summary>
        [Category("Toolbar")]
        [Description("Default width (horizontal Toolbar) or height (vertical Toolbar) of the Toolbar's item cells.")]
        [Localizable(true)]
        public Unit ItemCellDistance
        {
            get
            {
                object obj = this.ViewState["ItemCellDistance"];
                return obj == null ? Unit.Empty : (Unit)obj;
            }
            set { ViewState["ItemCellDistance"] = value; }
        }


        /// <summary>
        /// The width of a cell that contains a <see cref="ToolbarSeparator"/>.
        /// </summary>
        [Category("Toolbar")]
        [Description("Default width (horizontal Toolbar) or height (vertical toolbar) of the separator cells.")]
        public Unit SeparatorCellDistance
        {
            get
            {
                object obj = this.ViewState["SeparatorCellDistance"];
                return obj == null ? Unit.Empty : (Unit)obj;
            }
            set { ViewState["SeparatorCellDistance"] = value; }
        }

        /// <summary>
        /// Image of a toolbar separator. If not defined, a separator just
        /// renders as a space between toolbar items.
        /// <seealso cref="SeparatorCellDistance"/>
        /// </summary>
        [Category("Toolbar")]
        [Description("Image to be used for the toolbar separators.")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string SeparatorImageUrl
        {
            get
            {
                string url = (string)this.ViewState["SeparatorImageUrl"];
                return url == null ? String.Empty : url;
            }
            set { ViewState["SeparatorImageUrl"] = value; }
        }


        /// <summary>
        /// Optional background image of the toolbar. Tiled horizontally
        /// or vertically depending on toolbar orientation.
        /// </summary>
        [Category("Toolbar")]
        [Description("Image to be used as a background of the toolbar.")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        [Localizable(true)]
        public string BackgroundImageUrl
        {
            get
            {
                string url = (string)this.ViewState["BackgroundImageUrl"];
                return url == null ? String.Empty : url;
            }
            set { ViewState["BackgroundImageUrl"] = value; }
        }

        #endregion


        #region item event

        /// <summary>
        /// Raised if an item of the toolbar that posts back to
        /// the server is being clicked.
        /// </summary>
        public event ItemEventHandler ItemPostBack;

        #endregion


        #region intialization

        /// <summary>
        /// Inits the control.
        /// </summary>
        public Toolbar()
            : base(HtmlTextWriterTag.Table)
        {
            //register event handler to synchronize the controls collection
            //with the items
            this.m_items = new ToolbarItemCollection();
            m_items.ItemAdded += new ItemEventHandler(items_ItemAdded);
            m_items.ItemRemoved += new ItemEventHandler(items_ItemRemoved);
            m_items.ItemsCleared += new EventHandler(items_ItemsCleared);
        }


        #endregion


        #region rendering

        /// <summary>
        /// Adds table attributes to the rendered output.
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");

            if (this.BackgroundImageUrl != String.Empty)
            {
                string url = String.Format("url({0})", ResolveUrl(BackgroundImageUrl));
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, url);

                //repeat horizontally or vertically depending on orientation
                string repeat = Orientation == ToolbarOrientation.Horizontal ? "repeat-x" : "repeat-y";
                writer.AddStyleAttribute("background-repeat", repeat);
            }

            base.AddAttributesToRender(writer);
        }


        /// <summary>
        /// Creates the table content with all items of the toolbar.
        /// </summary>
        /// <returns></returns>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Orientation == ToolbarOrientation.Horizontal)
            {
                this.RenderHorizontally(writer);
            }
            else
            {
                this.RenderVertically(writer);
            }
        }


        /// <summary>
        /// Renders a horizontally oriented toolbar which contains
        /// all toolbar items in a single table row.
        /// </summary>
        /// <param name="writer"></param>
        protected void RenderHorizontally(HtmlTextWriter writer)
        {
            //open table row
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            //create cell for each item
            foreach (ToolbarItem item in this.m_items)
            {
                if (!item.Visible) continue;

                //render all items in the same row.
                RenderItemCell(item, writer);
            }

            //close table row
            writer.RenderEndTag();
        }


        /// <summary>
        /// Renders a vertically oriented toolbar which contains
        /// every toolbar item in a single table row.
        /// </summary>
        /// <param name="writer"></param>
        protected void RenderVertically(HtmlTextWriter writer)
        {
            foreach (ToolbarItem item in this.m_items)
            {
                if (!item.Visible) continue;

                //open table row
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                //render every item in a separate row
                RenderItemCell(item, writer);

                //close table row
                writer.RenderEndTag();
            }
        }



        /// <summary>
        /// Renders a table cell of a given toolbar item.
        /// </summary>
        /// <param name="item">Item to be rendered.</param>
        /// <param name="writer">Writer that streams output to the client.</param>
        protected void RenderItemCell(ToolbarItem item, HtmlTextWriter writer)
        {
            TableCell cell = new TableCell();
            cell.HorizontalAlign = item.HorizontalAlign;
            cell.VerticalAlign = item.VerticalAlign;

            if (item is ToolbarSeparator)
            {
                //set separator height (custom if set or toolbar property)
                if (this.Orientation == ToolbarOrientation.Horizontal)
                    //set the cell width on a horizontal toolbar
                    cell.Width = item.ItemCellDistance == Unit.Empty ? this.SeparatorCellDistance : item.ItemCellDistance;
                else
                    //set the cell height on a vertical toolbar
                    cell.Height = item.ItemCellDistance == Unit.Empty ? this.SeparatorCellDistance : item.ItemCellDistance;


                if (this.SeparatorImageUrl != String.Empty)
                {
                    Image img = new Image();
                    img.ImageUrl = this.SeparatorImageUrl;
                    cell.Controls.Add(img);
                }
                else
                {
                    cell.Text = "&nbsp;";
                }
                cell.RenderControl(writer);
            }
            else
            {
                if (this.Orientation == ToolbarOrientation.Horizontal)
                    //set the cell width on a horizontal toolbar
                    cell.Width = item.ItemCellDistance == Unit.Empty ? this.ItemCellDistance : item.ItemCellDistance;
                else
                    //set the cell height on a vertical toolbar
                    cell.Height = item.ItemCellDistance == Unit.Empty ? this.ItemCellDistance : item.ItemCellDistance;

                //render the item
                cell.RenderBeginTag(writer);
                item.RenderControl(writer);
                cell.RenderEndTag(writer);
            }
        }

        #endregion


        #region event handlers

        /// <summary>
        /// Adds a new item to the internal <c>Controls</c>
        /// collection.
        /// </summary>
        /// <param name="item"></param>
        protected void items_ItemAdded(ToolbarItem item)
        {
            this.Controls.Add(item);

            if (item is IPostBackToolbarItem)
            {
                //register click event handler
                ((IPostBackToolbarItem)item).ItemSubmitted += new ItemEventHandler(Items_ItemSubmitted);
            }
        }

        /// <summary>
        /// Removes an item from the internal <c>Controls</c>
        /// collection.
        /// </summary>
        /// <param name="item"></param>
        protected void items_ItemRemoved(ToolbarItem item)
        {
            this.Controls.Remove(item);
        }

        /// <summary>
        /// Removes all controls from the internal control collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void items_ItemsCleared(object sender, EventArgs e)
        {
            this.Controls.Clear();
        }

        /// <summary>
        /// Handles toolbar item events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Items_ItemSubmitted(ToolbarItem item)
        {
            //bubble event
            if (ItemPostBack != null) ItemPostBack(item);
        }

        #endregion

    }
}
