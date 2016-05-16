using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// Dummy class which is used by the <see cref="Toolbar"/>
    /// class to maintain spacers.
    /// </summary>
    [ToolboxData("<{0}:ToolbarSeparator runat=server></{0}:ToolbarSeparator>")]
    public class ToolbarSeparator : ToolbarItem
    {


        public ToolbarSeparator()
        {
        }


        protected void InitItemControls()
        { /*noting to do here */ }

    }
}
