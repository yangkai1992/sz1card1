using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Drawing.Design;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// 选项卡
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Designer(typeof(SubContainerControlDesigner))]
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class TabOptionItem : Control
    {
        #region "Private Variables"
        private string _Tab_Name;
        private string _Focus_Name;
        private string _OnClientClick = "";
        private bool _Tab_Visible = true;
        #endregion

        #region "Public Variables"
        /// <summary>
        /// 选项卡名称
        /// </summary>
        public string Tab_Name
        {
            get
            {
                return _Tab_Name;
            }
            set
            {
                _Tab_Name = value;
            }
        }

        /// <summary>
        /// 焦点控件ID
        /// </summary>
        public string Focus_Name
        {
            get
            {
                return _Focus_Name;
            }
            set
            {
                _Focus_Name = value;
            }
        }

        /// <summary>
        /// 点击脚本
        /// </summary>
        public string OnClientClick
        {
            get
            {
                return _OnClientClick;
            }
            set
            {
                _OnClientClick = value;
            }
        }

        /// <summary>
        /// 选项卡是否显示
        /// </summary>
        public bool Tab_Visible
        {
            get
            {
                return _Tab_Visible;
            }
            set
            {
                _Tab_Visible = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TabOptionItem()
            : this(String.Empty, true)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_Tab_Name"></param>
        /// <param name="_Tab_Visible"></param>
        public TabOptionItem(string _Tab_Name, bool _Tab_Visible)
        {
            this._Tab_Name = _Tab_Name;
            this._Tab_Visible = _Tab_Visible;
        }
        #endregion
    }
}
