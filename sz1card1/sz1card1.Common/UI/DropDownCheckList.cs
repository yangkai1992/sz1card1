using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using sz1card1.Common.Enum;
using sz1card1.Common.UI.CssStyle;

[assembly: WebResource("sz1card1.Common.UI.Resources.expand.gif", "image/gif")]
[assembly: WebResource("sz1card1.Common.UI.Resources.DropDownCheckList.js", "text/javascript")]
namespace sz1card1.Common.UI
{
    [ToolboxData("<{0}:DropDownCheckList runat=server></{0}:DropDownCheckList>"), DefaultProperty("TextWhenNoneChecked")]
    public class DropDownCheckList : CheckBoxList
    {
        #region Constants
        private const string kCATEGORY_DISPLAYBOX = "DisplayBox";
        private const string kCATEGORY_DISPLAYTEXT = "DisplayText";
        private const string kCATEGORY_CHECKLIST = "CheckList";
        private const string kCATEGORY_CONFIGURATION = "Configuration";
        private const string kCATEGORY_OTHER = "Other Inherited CheckBoxList Properties";

        private const DropDownModeEnum kDEFAULT_DROPDOWNMODE = DropDownModeEnum.OnTopWithShim;
        private const string kDEFAULT_DISPLAYBOXCSSCLASS = "";
        private const string kDEFAULT_DISPLAYBOXCSSSTYLE = "border: 1px solid #9999CC; cursor: pointer;";
        private const DropImagePositionEnum kDEFAULT_DROPIMAGEPOSITION = DropImagePositionEnum.Right;
        private const int kDEFAULT_DISPLAYTEXTWIDTH = 150;
        private const string kDEFAULT_CHECKLISTCSSCLASS = "";
        private const string kDEFAULT_CHECKLISTCSSSTYLE = "overflow: auto; border: 1px solid black; padding: 4px; background-color: #FFFFFF;";
        private const string kDEFAULT_TRUNCATESTRING = "...";
        private const string kDEFAULT_SEPARATOR = ", ";
        private const string kDEFAULT_TEXTWHENNONECHECKED = "";
        private const string kDEFAULT_DISPLAYTEXTCSSCLASS = "";
        private const string kDEFAULT_DISPLAYTEXTCSSSTYLE = "";
        private const int kDEFAULT_DISPLAYTEXTPADDINGBOTTOM = 0;
        private const int kDEFAULT_DISPLAYTEXTPADDINGTOP = 0;
        private const int kDEFAULT_DISPLAYTEXTPADDINGRIGHT = 4;
        private const int kDEFAULT_DISPLAYTEXTPADDINGLEFT = 4;
        private const DisplayTextListEnum kDEFAULT_DISPLAYTEXTLIST = DisplayTextListEnum.Labels;

        #endregion

        #region Private Members

        private DropDownModeEnum _dropDownMode = kDEFAULT_DROPDOWNMODE;
        private string _displayBoxCssClass = kDEFAULT_DISPLAYBOXCSSCLASS;
        private string _displayBoxCssStyle = kDEFAULT_DISPLAYBOXCSSSTYLE;
        private string _dropImageSrc = "";
        private DropImagePositionEnum _dropImagePosition = kDEFAULT_DROPIMAGEPOSITION;
        private int _displayTextWidth = kDEFAULT_DISPLAYTEXTWIDTH;
        private string _checkListCssClass = kDEFAULT_CHECKLISTCSSCLASS;
        private string _checkListCssStyle = kDEFAULT_CHECKLISTCSSSTYLE;
        private string _clientCodeLocation = "";
        private string _truncateString = kDEFAULT_TRUNCATESTRING;
        private string _separator = kDEFAULT_SEPARATOR;
        private string _textWhenNoneChecked = kDEFAULT_TEXTWHENNONECHECKED;
        private string _displayTextCssClass = kDEFAULT_DISPLAYTEXTCSSCLASS;
        private string _displayTextCssStyle = kDEFAULT_DISPLAYTEXTCSSSTYLE;
        private int _displayTextPaddingBottom = kDEFAULT_DISPLAYTEXTPADDINGBOTTOM;
        private int _displayTextPaddingRight = kDEFAULT_DISPLAYTEXTPADDINGRIGHT;
        private int _displayTextPaddingTop = kDEFAULT_DISPLAYTEXTPADDINGTOP;
        private int _displayTextPaddingLeft = kDEFAULT_DISPLAYTEXTPADDINGLEFT;
        private DisplayTextListEnum _displayTextList = kDEFAULT_DISPLAYTEXTLIST;

        #endregion

        #region DisplayBox properties

        /// <summary>
        /// Specifies whether the drop-down checklist will appear inline or on top of other
        /// HTML content
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYBOX),
        DefaultValue(kDEFAULT_DROPDOWNMODE)]
        public DropDownModeEnum DropDownMode
        {
            get { return _dropDownMode; }
            set { _dropDownMode = value; }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class used when rendering
        /// the display box
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYBOX),
        DefaultValue(kDEFAULT_DISPLAYBOXCSSCLASS)]
        public string DisplayBoxCssClass
        {
            get { return _displayBoxCssClass; }
            set { _displayBoxCssClass = value; }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) style attribute used
        /// when rendering the display box
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYBOX),
        DefaultValue(kDEFAULT_DISPLAYBOXCSSSTYLE)]
        public string DisplayBoxCssStyle
        {
            get { return _displayBoxCssStyle; }
            set { _displayBoxCssStyle = value; }
        }


        /// <summary>
        /// Gets or sets the <b>src</b> attribute of the image that is rendered when 
        /// <see cref="DropImagePosition">DropImagePosition</see> is set to
        /// <see cref="DropImagePositionEnum.Left">Left</see>,
        /// <see cref="DropImagePositionEnum.Right">Right</see>, or
        /// <see cref="DropImagePositionEnum.Both">Both</see>
        /// </summary>
        /// <remarks>
        /// By default, this property is <see cref="String.Empty">String.Empty</see>,
        /// which means that no image is rendered.
        /// </remarks>
        [Browsable(true), Category(kCATEGORY_DISPLAYBOX)]
        public string DropImageSrc
        {
            get { return _dropImageSrc; }
            set { _dropImageSrc = value; }
        }

        /// <summary>
        /// Gets or sets the position relative to the display box where the 
        /// drop-down image is rendered
        /// </summary>
        /// <remarks>
        /// To specify the <b>src</b> attribute for the drop-down image,
        /// set the <see cref="DropImageSrc">DropImageSrc</see> property.
        /// </remarks>
        [Browsable(true), Category(kCATEGORY_DISPLAYBOX),
        DefaultValue(kDEFAULT_DROPIMAGEPOSITION)]
        public DropImagePositionEnum DropImagePosition
        {
            get { return _dropImagePosition; }
            set { _dropImagePosition = value; }
        }

        #endregion

        #region DisplayText properties

        /// <summary>
        /// Gets or sets the maximum width in pixels of text in the display box
        /// </summary>
        /// <remarks>
        /// When the checked boxes make a string of text that exceeds the
        /// <code>DisplayTextWidth</code> amount in width, the display text
        /// is truncated to the maximum width, and the 
        /// <see cref="TruncateString">TruncateString</see> is appended 
        /// (by default, an ellipsis).  To allow the string to expand to its
        /// full width without truncating, set <code>DisplayTextWidth</code>
        /// to -1.
        /// </remarks>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTWIDTH)]
        public int DisplayTextWidth
        {
            get { return _displayTextWidth; }
            set { _displayTextWidth = value; }
        }


        /// <summary>
        /// Gets or sets the amount of pixels to pad between the display text and the
        /// left edge of the display box (or the drop-down image if present)
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTPADDINGLEFT)]
        public int DisplayTextPaddingLeft
        {
            get { return _displayTextPaddingLeft; }
            set { _displayTextPaddingLeft = value; }
        }


        /// <summary>
        /// Gets or sets the amount of pixels to pad between the display text and the
        /// top edge of the display box
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTPADDINGTOP)]
        public int DisplayTextPaddingTop
        {
            get { return _displayTextPaddingTop; }
            set { _displayTextPaddingTop = value; }
        }


        /// <summary>
        /// Gets or sets the amount of pixels to pad between the display text and the
        /// right edge of the display box (or the drop-down image if present)
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTPADDINGRIGHT)]
        public int DisplayTextPaddingRight
        {
            get { return _displayTextPaddingRight; }
            set { _displayTextPaddingRight = value; }
        }


        /// <summary>
        /// Gets or sets the amount of pixels to pad between the display text and the
        /// bottom edge of the display box
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTPADDINGBOTTOM)]
        public int DisplayTextPaddingBottom
        {
            get { return _displayTextPaddingBottom; }
            set { _displayTextPaddingBottom = value; }
        }


        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class used when rendering
        /// the display text		
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTCSSSTYLE)]
        public string DisplayTextCssStyle
        {
            get { return _displayTextCssStyle; }
            set { _displayTextCssStyle = value; }
        }


        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) style attribute used when rendering
        /// the display text		
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTCSSCLASS)]
        public string DisplayTextCssClass
        {
            get { return _displayTextCssClass; }
            set { _displayTextCssClass = value; }
        }


        /// <summary>
        /// Gets or sets the text to display when no items are checked		
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_TEXTWHENNONECHECKED)]
        public string TextWhenNoneChecked
        {
            get { return _textWhenNoneChecked; }
            set { _textWhenNoneChecked = value; }
        }


        /// <summary>
        /// Gets or sets the text to use as a separater when listing checked choices
        /// in the display text
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_SEPARATOR)]
        public string Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }


        /// <summary>
        /// Gets or sets the text to append to the truncated display text 
        /// when the checked choices produce a string to wide for the display box
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_TRUNCATESTRING)]
        public string TruncateString
        {
            get { return _truncateString; }
            set { _truncateString = value; }
        }



        /// <summary>
        /// Gets or sets the option of displaying either labels or values
        /// for checked boxes
        /// </summary>
        [Browsable(true), Category(kCATEGORY_DISPLAYTEXT),
        DefaultValue(kDEFAULT_DISPLAYTEXTLIST)]
        public DisplayTextListEnum DisplayTextList
        {
            get { return _displayTextList; }
            set { _displayTextList = value; }
        }


        #endregion

        #region CheckList Properties

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class used when rendering
        /// the checklist box		
        /// </summary>
        [Browsable(true), Category(kCATEGORY_CHECKLIST),
        DefaultValue(kDEFAULT_CHECKLISTCSSCLASS)]
        public string CheckListCssClass
        {
            get { return _checkListCssClass; }
            set { _checkListCssClass = value; }
        }


        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) style attribute used when rendering
        /// the checklist box		
        /// </summary>
        [Browsable(true), Category(kCATEGORY_CHECKLIST),
        DefaultValue(kDEFAULT_CHECKLISTCSSSTYLE)]
        public string CheckListCssStyle
        {
            get { return _checkListCssStyle; }
            set { _checkListCssStyle = value; }
        }



        // other inherited properties of a CheckBoxList

        /// <summary>
        /// Inherited from <see cref="CheckBoxList.CellPadding">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override int CellPadding
        {
            get { return base.CellPadding; }
            set { base.CellPadding = value; }
        }


        /// <summary>
        /// Inherited from <see cref="CheckBoxList.CellSpacing">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override int CellSpacing
        {
            get { return base.CellSpacing; }
            set { base.CellSpacing = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.Height">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override Unit Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.Width">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override Unit Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }


        /// <summary>
        /// Inherited from <see cref="CheckBoxList.RepeatColumns">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_CHECKLIST),
        ]
        public override int RepeatColumns
        {
            get { return base.RepeatColumns; }
            set { base.RepeatColumns = value; }
        }


        /// <summary>
        /// Inherited from <see cref="CheckBoxList.RepeatDirection">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_CHECKLIST),
        ]
        public override RepeatDirection RepeatDirection
        {
            get { return base.RepeatDirection; }
            set { base.RepeatDirection = value; }
        }


        /// <summary>
        /// Inherited from <see cref="CheckBoxList.RepeatLayout">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_CHECKLIST),
        ]
        public override RepeatLayout RepeatLayout
        {
            get { return base.RepeatLayout; }
            set { base.RepeatLayout = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.BackColor">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override System.Drawing.Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.BorderColor">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override System.Drawing.Color BorderColor
        {
            get { return base.BorderColor; }
            set { base.BorderColor = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.BorderStyle">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.BorderWidth">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override Unit BorderWidth
        {
            get { return base.BorderWidth; }
            set { base.BorderWidth = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.CssClass">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override string CssClass
        {
            get { return base.CssClass; }
            set { base.CssClass = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.Font">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override FontInfo Font
        {
            get { return base.Font; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.ForeColor">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override System.Drawing.Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }


        /// <summary>
        /// Inherited from <see cref="CheckBoxList.TextAlign">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override TextAlign TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.AccessKey">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override string AccessKey
        {
            get { return base.AccessKey; }
            set { base.AccessKey = value; }
        }


        /// <summary>
        /// Inherited from <see cref="ListControl.AutoPostBack">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override bool AutoPostBack
        {
            get { return base.AutoPostBack; }
            set { base.AutoPostBack = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.TabIndex">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override short TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = value; }
        }


        /// <summary>
        /// Inherited from <see cref="WebControl.ToolTip">CheckBoxList</see>
        /// </summary>
        [Browsable(true), Category(kCATEGORY_OTHER),
        ]
        public override string ToolTip
        {
            get { return base.ToolTip; }
            set { base.ToolTip = value; }
        }


        #endregion

        #region Configuration Properties

        /// <summary>
        /// Returns or sets the location of the client JavaScript code file
        /// </summary>
        /// <remarks>
        /// <para>The <b>ClientCodeLocation</b> path is the url written as the
        /// <b>src</b> attribute of a &lt;script&gt; tag as the control
        /// is rendered.  By default, this location is 
        /// <b>/aspnet_client/UNLV_IAP_WebControls/DropDownCheckList/DropDownCheckList.js</b>
        /// </para>
        /// <para>
        /// If the client javascript file is not in the location specified by 
        /// <b>ClientCodeLocation</b>, client-side javascript errors will occur
        /// and the control will not function properly.
        /// </para>
        /// </remarks>
        [Browsable(true), Category(kCATEGORY_CONFIGURATION)]
        public string ClientCodeLocation
        {
            get { return _clientCodeLocation; }
            set { _clientCodeLocation = value; }
        }


        #endregion

        #region Client-side Script Generation (OnPreRender)
        /// <summary>
        /// Generates a string containing a &lt;script&gt; tag identifying the
        /// client-side javascript code to include on the page; this string
        /// is registered with the page when a DropDownCheckList control is present.
        /// </summary>
        /// <returns>the &lt;script&gt; tag in a string</returns>
        protected string ClientJavascriptCodeScript()
        {
            string sSrc = string.Empty;
            if (string.IsNullOrEmpty(ClientCodeLocation))
            {
                sSrc = Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.UI.Resources.DropDownCheckList.js");
            }
            else
            {
                sSrc = ClientCodeLocation;
            }
            string sTemplate = "<script language='JavaScript1.2' src='{0}' ></script>\n";
            return string.Format(sTemplate, sSrc);
        }

        /// <summary>
        /// Returns a consistent template for a client-side 
        /// &lt;script&gt; tag
        /// </summary>
        /// <returns>the string</returns>
        protected string GetScriptTemplate()
        {
            return "<script language='JavaScript1.2'>{0}</script>\n";
        }

        /// <summary>
        /// Returns a string of javascript code to create
        /// and initialize a DropDownCheckList object on the client;
        /// this script is registered with the Page.
        /// </summary>
        /// <returns>the javascript code</returns>
        protected string ClientInitializeScript()
        {
            string sId = this.ClientID;

            // client code to construct the object, according to the javascript constructor:
            // function DDCL_DropDownCheckList(id, textWhenNone, separator, truncateString, dropDownMode, allowExpand, displayList)
            // e.g.
            //var ddcl_obj_ddcl1 = new DDCL_DropDownCheckList('DDCL1', 'Select Items...', '; ', '...', DDCL_DROPDOWNMODE_ONTOPWITHSHIM, false, DDCL_DISPLAYTEXTLIST_LABELS);               

            string sConstruct = string.Format(
                "var ddcl_obj_{0} = new DDCL_DropDownCheckList('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6});"
                , sId
                , _textWhenNoneChecked
                , _separator
                , _truncateString
                , Convert.ToInt32(_dropDownMode)
                , (_displayTextWidth > 0 ? "false" : "true")
                , Convert.ToInt32(_displayTextList)
                );

            return string.Format(GetScriptTemplate(), sConstruct);
        }

        protected override void OnInit(EventArgs e)
        {
            base.Width = _displayTextWidth;
            base.OnInit(e);
        }

        /// <summary>
        /// Registers client-side scripting code with the Page
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // the main Javascript object should be rendered only
            // once no matter how many controls need it.
            this.Page.RegisterClientScriptBlock("DDCL_MAINCODE"
                , ClientJavascriptCodeScript());

            // for this control, generate the necessary 
            // client-side initialization script and register 
            // with the page
            this.Page.RegisterStartupScript(
                this.ClientID + "_Initialize"
                , ClientInitializeScript());

        }
        #endregion

        #region Rendering

        /// <summary>
        /// Renders the drop-down image specified by 
        /// <see cref="DropImageSrc">DropImageSrc</see>
        /// </summary>
        /// <param name="output">the writer accepting the rendered output</param>
        protected void RenderDropImage(HtmlTextWriter output)
        {
            if (string.IsNullOrEmpty(_dropImageSrc))
            {
                _dropImageSrc = Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.UI.Resources.expand.gif");
            }
            output.Write("<td>");
            output.Write("<img");
            output.WriteAttribute("id", this.ClientID + "_img");
            output.WriteAttribute("align", "absmiddle");
            output.WriteAttribute("src", _dropImageSrc);
            output.WriteAttribute("border", "0");
            output.Write("/>");

            output.Write("</td>");
        }

        /// <summary>
        /// Modifies the HTML code as rendered through the inheritence from CheckBoxList
        /// to add additional custom attributes to each checkbox
        /// </summary>
        /// <param name="sHtml">the rendered HTML for the CheckBoxList</param>
        /// <returns>the modified HTML as a string</returns>
        protected string ModifyRenderedCheckboxes(string sHtml)
        {

            // use a regular expression to identify in the form:
            //<input id="DropDownCheckList1_0" type="checkbox" 
            string s = string.Format(
                "input id=\"{0}_(?<index>\\d+)\"\\s+type=\"checkbox\"\\s+"
                , this.ClientID);

            Regex r = new Regex(s, RegexOptions.IgnoreCase);

            MatchCollection matches = r.Matches(sHtml);
            foreach (Match m in matches)
            {
                try
                {
                    int index = Convert.ToInt32(m.Groups["index"].Value);
                    sHtml = Regex.Replace(sHtml, m.Value, m.Value + " value=\"" + this.Items[index].Value + "\" ");
                }
                catch
                {  /* do nothing */ }
            }

            return sHtml;
        }


        /// <summary>
        /// Renders the DropDownCheckList control
        /// </summary>
        /// <param name="output">the writer accepting the rendered output</param>
        protected override void Render(HtmlTextWriter output)
        {

            // render the display box as an html table;
            // we're using .Write() here rather than .RenderBeginTag()
            // to ensure divs are treated as divs in all cases, and
            // that all attributes are written.

            // here is a sample of the HTML to be rendered:
            /*
                    <table id="DDCL2_displaybox" cellpadding="0" cellspacing="0" border="0" style="padding: 1px; border: 1px solid #9999CC; cursor: pointer;">
                      <tr>
                        <td width="148" style="padding-left: 4px;">              
                          <div id="DDCL2_boundingbox" style="overflow: hidden;">
                              <span id="DDCL2_text" style="font-family: Tahoma; font-size: 10pt;">
                              </span>
                          </div>
                        </td>
                        <td>
                          <img id="DDCL2_img" align="absmiddle" src="expand.gif" border="0" />
                        </td>
                      </tr>
                    </table>      
            */

            output.Write("<table");
            output.WriteAttribute("id", this.ClientID + "_displaybox");
            output.WriteAttribute("cellpadding", "0");
            output.WriteAttribute("cellspacing", "0");
            output.WriteAttribute("border", "0");
            if (_displayBoxCssClass != "")
                output.WriteAttribute("class", _displayBoxCssClass);
            if (_displayBoxCssStyle != "")
                output.WriteAttribute("style", _displayBoxCssStyle);
            output.Write(">");
            output.Write("<tr>");

            // render the image at the left?
            if ((_dropImagePosition == DropImagePositionEnum.Left
                || _dropImagePosition == DropImagePositionEnum.Both))
                RenderDropImage(output);


            // render the text box cell and padding
            output.Write("<td");
            if (_displayTextWidth > 0)
                output.WriteAttribute("width", _displayTextWidth.ToString());

            CssStyleUtility pads = new CssStyleUtility("");
            if (_displayTextPaddingLeft > -1)
                pads.StyleTable["padding-left"] = _displayTextPaddingLeft.ToString() + "px";
            if (_displayTextPaddingTop > -1)
                pads.StyleTable["padding-top"] = _displayTextPaddingTop.ToString() + "px";
            if (_displayTextPaddingRight > -1)
                pads.StyleTable["padding-right"] = _displayTextPaddingRight.ToString() + "px";
            if (_displayTextPaddingBottom > -1)
                pads.StyleTable["padding-bottom"] = _displayTextPaddingBottom.ToString() + "px";

            if (pads.StyleTable.Count > 0)
                output.WriteAttribute("style", pads.ToString());

            output.Write(">");

            // render the text box cell contents; start with the boundingbox
            // force this to be a div
            output.Write("<div");
            output.WriteAttribute("id", this.ClientID + "_boundingbox");
            if (_displayTextWidth > 0)
                output.WriteAttribute("style", "overflow: hidden;");
            output.Write(">");

            // next render the span that will contain the display text
            output.Write("<span");
            output.WriteAttribute("id", this.ClientID + "_text");
            if (_displayTextCssClass != "")
                output.WriteAttribute("class", _displayTextCssClass);
            if (_displayTextCssStyle != "")
                output.WriteAttribute("style", _displayTextCssStyle);
            output.Write(">");

            // we'll put some content in the span so Mozilla can get 
            // the boundingbox width correctly; typically this is
            // &nbsp; if we're in design mode though we would
            // want different text to display
            if (this.Context == null)
            {
                // this is design mode; there is no current HttpContext;
                // output either the TextWhenNone value, or the ID
                if (this.TextWhenNoneChecked != string.Empty)
                    output.Write(this.TextWhenNoneChecked);
                else
                    output.Write("[" + this.ID + "]");
            }
            else
            {
                // this is runtime; we have an HttpContext; 
                // output a simple &nbsp; so Mozilla can get the 
                // width of the bounding box correctly
                output.Write("&nbsp;");
            }

            output.Write("</span>");
            output.Write("</div>"); // </div>
            output.Write("</td>"); // </td>

            // render the image at the right?
            if ((_dropImagePosition == DropImagePositionEnum.Right
                || _dropImagePosition == DropImagePositionEnum.Both))
                RenderDropImage(output);


            output.Write("</tr>");
            output.Write("</table>");


            // now render the checklist div if this is runtime; 
            // start with appropriate style attributes
            if (this.Context != null)
            {
                output.Write("<div");
                output.WriteAttribute("id", this.ClientID + "_checkboxes");
                if (_checkListCssClass != "")
                    output.WriteAttribute("class", _checkListCssClass);

                CssStyleUtility css = new CssStyleUtility(_checkListCssStyle);
                css.StyleTable["display"] = "none";
                if (_dropDownMode == DropDownModeEnum.Inline)
                    css.StyleTable["position"] = "relative";
                else
                {
                    css.StyleTable["position"] = "absolute";
                    css.StyleTable["z-index"] = "32767";
                }
                output.WriteAttribute("style",
                    css.ToString());

                output.Write(">");

                // next, render the contents of the checkboxlist;
                // do we need to render values?
                if (_displayTextList == DisplayTextListEnum.Values)
                {
                    // if so, we need to render to a string first
                    // so we can include our own values attribute
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter wr = new HtmlTextWriter(sw);
                    base.Render(wr);
                    string sHtml = sw.ToString();
                    wr.Close();
                    sw.Close();
                    // now modify the code to include custom attributes				
                    sHtml = ModifyRenderedCheckboxes(sHtml);
                    // and write to the output stream
                    output.Write(sHtml);
                }
                else
                {
                    // if we're not rendering custom value attributes,
                    // just output the checkboxes to the output stream
                    base.Render(output);
                }

                // close off by rendering the ending div tag
                output.Write("</div>");

                // if we want the shim technique, render that now as an iframe
                if (_dropDownMode == DropDownModeEnum.OnTopWithShim)
                {
                    // force the write so all the attributes we need are written
                    output.Write("<iframe");
                    output.WriteAttribute("id", this.ClientID + "_shim");
                    output.WriteAttribute("src", "javascript: false;");
                    output.WriteAttribute("scrolling", "no");
                    output.WriteAttribute("frameborder", "0");
                    output.WriteAttribute("style", "position:absolute; top:0px; left:0px; display:none;");
                    output.Write(">");
                    output.Write("</iframe>");
                }

            }

        }

        #endregion

        #region Methods to retrieve selected choices as a string

        /// <summary>
        /// Generates a string of text labels from the selected items, using the
        /// control's <see cref="Separator">Separator</see> property to separate
        /// listed items
        /// </summary>
        /// <returns>the string of selected text labels</returns>
        public string SelectedLabelsToString()
        {
            return RenderSelectedItemsToString(true, this.Separator, "");
        }

        /// <summary>
        /// Generates a string of text labels from the selected items
        /// </summary>
        /// <param name="separator">text used to separate listed items</param>
        /// <returns>the string of selected values</returns>
        public string SelectedLabelsToString(string separator)
        {
            return RenderSelectedItemsToString(true, separator, "");
        }

        /// <summary>
        /// Generates a string of delimited text labels from the selected items
        /// </summary>
        /// <param name="separator">text used to separate listed items</param>
        /// <param name="delimiter">text used to delimit each listed item</param>
        /// <returns>the string of selected values</returns>
        public string SelectedLabelsToString(string separator, string delimiter)
        {
            return RenderSelectedItemsToString(true, separator, delimiter);
        }


        /// <summary>
        /// Generates a string of values from the selected items, using the
        /// control's <see cref="Separator">Separator</see> property to separate
        /// listed items
        /// </summary>
        /// <returns>the string of selected values</returns>
        public string SelectedValuesToString()
        {
            return RenderSelectedItemsToString(false, this.Separator, "");
        }

        /// <summary>
        /// Generates a string of values from the selected items
        /// </summary>
        /// <param name="separator">text used to separate listed items</param>
        /// <returns>the string of selected values</returns>
        public string SelectedValuesToString(string separator)
        {
            return RenderSelectedItemsToString(false, separator, "");
        }

        /// <summary>
        /// Generates a string of delimited values from the selected items
        /// </summary>
        /// <param name="separator">text used to separate listed items</param>
        /// <param name="delimiter">text used to delimit each listed item</param>
        /// <returns>the string of selected values</returns>
        public string SelectedValuesToString(string separator, string delimiter)
        {
            return RenderSelectedItemsToString(false, separator, delimiter);
        }


        /// <summary>
        /// Utility method for generating a string from the selected items
        /// </summary>
        /// <param name="bRenderLabel">true to use labels in the resultant string, false to use values </param>
        /// <param name="sep">the separator to use when listing items</param>
        /// <param name="delim">text delimiter to use </param>
        /// <returns>a string of items delimited with <i>sep</i></returns>
        protected string RenderSelectedItemsToString(bool bRenderLabel, string sep, string delim)
        {
            string sReturn = string.Empty;

            foreach (ListItem item in Items)
            {
                if (item.Selected)
                {
                    if (sReturn.Length > 0) sReturn += sep;
                    if (bRenderLabel)
                        sReturn += delim + item.Text + delim;
                    else
                        sReturn += delim + item.Value + delim;
                }
            }

            return sReturn;
        }
        #region 十六进制
        /// <summary>
        /// 获取选中的值
        /// </summary>
        /// <returns></returns>
        public int SelectedValuesToInt32()
        {
            string[] values = SelectedValuesToString().Split(',');
            return OrOperateByHexString(values);
        }

        /// <summary>
        /// 设置选中的值
        /// </summary>
        public void SetSelectedValuesByInt32(int value)
        {
            foreach (ListItem item in Items)
            {
                int itemValue = Convert.ToInt32(item.Value, 16);
                item.Selected = (value & itemValue) > 0;
            }
        }

        /// <summary>
        /// 十六进制字符串的或操作
        /// </summary>
        /// <param name="value"></param>
        private int OrOperateByHexString(string[] value)
        {
            int orValue = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (!string.IsNullOrEmpty(value[i]))
                {
                    orValue = orValue | Convert.ToInt32(value[i].Trim(), 16);
                }
            }
            return orValue;
        }
        #endregion

        #region 二进制
        /// <summary>
        /// 获取选中的值(二进制)
        /// </summary>
        /// <returns></returns>
        public int SelectedValues()
        {
            int v = 0;
            string[] values = SelectedValuesToString().Split(',');
            foreach (string value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    v += Convert.ToInt32(value);
                }
            }
            return v;
        }

        /// <summary>
        /// 设置选中的值(二进制)
        /// </summary>
        public void SetSelectedValues(int value)
        {
            foreach (ListItem item in Items)
            {
                item.Selected = (value & Convert.ToInt32(item.Value)) > 0;
            }
        }
        #endregion

        #endregion

    }
}
