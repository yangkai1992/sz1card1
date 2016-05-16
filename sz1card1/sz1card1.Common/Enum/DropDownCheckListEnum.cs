using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// Supplies options for determining if labels or values will be listed in 
    /// the display box for checked boxes.
    /// </summary>
    public enum DisplayTextListEnum
    {
        /// <summary>
        /// Indicates that labels should be listed in the display text for checked boxes.
        /// </summary>
        Labels = 0,

        /// <summary>
        /// Indicates that values should be listed in the display text for checked boxes.
        /// </summary>
        Values = 1,

    }

    /// <summary>
    /// Supplies options for determining if the checklist will drop down inline with other
    /// HTML items on the page, or on top of other HTML content.
    /// </summary>
    public enum DropDownModeEnum
    {
        /// <summary>
        /// Renders the checklist using a CSS "position: relative" style attribute, placing
        /// the checklist inline with other HTML content
        /// </summary>
        Inline = 0,

        /// <summary>
        /// Renders the checklist using a CSS "position: absolute" style attribute, placing
        /// the checklist on top of other HTML content
        /// </summary>
        OnTop = 1,

        /// <summary>
        /// Renders the checklist using a CSS "position: absolute" style attribute, with
        /// an additional &lt;iframe&gt; tag which acts as a shim; this allows for Internet
        /// Explorer versions 5.5 and greater to properly render the checklist above 
        /// other windowed controls.
        /// </summary>
        OnTopWithShim = 2
    }

    /// <summary>
    /// Supplies options for specifying the position for rendering the drop-down image
    /// in a DropDownCheckList control, relative to the control's display box.
    /// </summary>
    public enum DropImagePositionEnum
    {
        /// <summary>
        /// Positions the drop-down image to the left of the display box
        /// </summary>
        Left = 0,

        /// <summary>
        /// Positions the drop-down image to the right of the display box
        /// </summary>
        Right = 1,

        /// <summary>
        /// Renders the drop-down image both left and right of the display box
        /// </summary>
        Both = 2,

        /// <summary>
        /// Specifies a drop-down image should not be rendered
        /// </summary>
        NoImage = 3
    }
}
