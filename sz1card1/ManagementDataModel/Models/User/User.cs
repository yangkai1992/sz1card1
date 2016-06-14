using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDataModel.Models.User
{
    public class User
    {
        private Guid _guid = Guid.Empty;
        private string _account = string.Empty;
        private string _password = string.Empty;
        private string _trueName = string.Empty;
        private string _qq = string.Empty;
        private string _mobile = string.Empty;
        private DateTime _addTime = DateTime.Now;
        private bool _isLocked = false;
        private string _meno = string.Empty;
        private int _userWeight = 0;
        private int _userWeightUsed = 0;
        private string _tel = string.Empty;
        private Guid _userGroupGuid = Guid.Empty;
        private string _headImage = string.Empty;

        public User()
        {
            _guid = Guid.NewGuid();
        }

        /// <summary>
        /// 主键唯一标识
        /// </summary>
        public virtual Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account
        {
            get { return _account; }
            set { _account = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string TrueName
        {
            get { return _trueName; }
            set { _trueName = value; }
        }
        /// <summary>
        /// QQ
        /// </summary>
        public virtual string QQ
        {
            get { return _qq; }
            set { _qq = value; }
        }
        /// <summary>
        /// 手机号 
        /// </summary>
        public virtual string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime AddTime
        {
            get { return _addTime; }
            set { _addTime = value; }
        }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public virtual bool IsLocked
        {
            get { return _isLocked; }
            set { _isLocked = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Meno
        {
            get { return _meno; }
            set { _meno = value; }
        }

        /// <summary>
        /// 权重
        /// </summary>
        public virtual int UserWeight
        {
            get { return _userWeight; }
            set { _userWeight = value; }
        }

        /// <summary>
        /// 已用权重
        /// </summary>
        public virtual int UserWeightUsed
        {
            get { return _userWeightUsed; }
            set { _userWeightUsed = value; }
        }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Tel
        {
            get { return _tel; }
            set { _tel=value;}
        }

        /// <summary>
        /// 用户组标识
        /// </summary>
        public virtual Guid UserGroupGuid
        {
            get { return _userGroupGuid; }
            set { _userGroupGuid = value; }
        }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string HeadImage
        {
            get { return _headImage; }
            set { _headImage = value; }
        }

    }
}
