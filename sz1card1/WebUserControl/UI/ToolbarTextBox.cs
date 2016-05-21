using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using WebUserControl.Delegate;

namespace WebUserControl.UI
{
   
    /// <summary>
    /// Composite control that renders a simple textbox.
    /// </summary>
    [DefaultProperty("Text"),
    ToolboxData("<{0}:TextBoxItem runat=server></{0}:TextBoxItem>")]
    public class ToolbarTextBox : ToolbarItem, IPostBackToolbarItem
    {

        #region members

        /// <summary>
        /// Contained textbox control which is rendered to the client.
        /// </summary>
        protected TextBox textBox;


        /// <summary>
        /// Raised if the item's text was changed.
        /// </summary>
        public event ItemEventHandler ItemSubmitted;

        #endregion


        #region properties

        [Category("Toolbar")]
        [DefaultValue("")]
        [Description("Item text")]
        [Localizable(true)]
        public string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }


        /// <summary>
        /// Whether the control posts back to the
        /// server after being changed.
        /// </summary>
        [Category("Toolbar")]
        [DefaultValue(false)]
        [Description("Whether the control performs an automatic PostBack if changed.")]
        public bool AutoPostBack
        {
            get { return textBox.AutoPostBack; }
            set { this.textBox.AutoPostBack = value; }
        }


        /// <summary>
        /// Enables / disables the item's textbox.
        /// </summary>
        public override bool RenderDisabled
        {
            get { return base.RenderDisabled; }
            set
            {
                base.RenderDisabled = value;
                this.textBox.Enabled = !value;
            }
        }

        #endregion


        #region intialization

        /// <summary>
        /// Inits the control.
        /// </summary>
        public ToolbarTextBox()
        {
            this.textBox = new TextBox();
            this.textBox.TextChanged += new EventHandler(textBox_TextChanged);
        }


        /// <summary>
        /// Adds the internal TextBox control to the item's
        /// <c>Controls</c> collection.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Controls.Add(textBox);
        }

        #endregion


        #region rendering

        /// <summary>
        /// Prevents rendering of the control itself.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Prevents rendering of the control itself.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }


        /// <summary>
        /// Renders the contained textbox control.
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            this.textBox.ApplyStyle(this.ControlStyle);
            this.textBox.CopyBaseAttributes(this);
            base.RenderContents(writer);
        }

        #endregion


        #region event handling

        /// <summary>
        /// Bubbles a text change event of the contained textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (this.ItemSubmitted != null) ItemSubmitted(this);
        }

        #endregion


    }
}
