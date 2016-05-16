using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Validation
{
    /// <summary>
    /// 实体数据跟踪记录
    /// </summary>
    public class UpdateTracking
    {
        public string PropertyName { set; get; }
        public string Description { get; set; }
        public string BeforeValue { set; get; }
        public string AfterValue { get; set; }
    }

    /// <summary>
    /// 实体数据跟踪记录汇总
    /// </summary>
    public class UpdateTotalTracking
    {
        public UpdateTotalTracking()
        {
            PropertyList = new List<string>();
        }
        public string Ip { get; set; }
        public string BusinessAccount { get; set; }
        public string UserAccount { get; set; }
        public string ClassName { set; get; }
        public Guid Guid { set; get; }
        public List<string> PropertyList { set; get; }
        public string OperateTime { set; get; }
    }
}
