using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// 移动运营商下拉多选框
    /// </summary>
    [ToolboxData("<{0}:MobileOperatorSeletor runat=server></{0}:MobileOperatorSeletor>"), DefaultProperty("TextWhenNoneChecked")]
    public class MobileOperatorSeletor : DropDownCheckList
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DataSource = DataUtil.GetMobileOperators();
            DataTextField = "text";
            DataValueField = "value";
            DataBind();
            foreach (ListItem item in Items)
            {
                item.Selected = true;
            }
        }
    }
}
