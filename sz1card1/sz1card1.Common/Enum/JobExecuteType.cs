using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    public enum JobExecuteType
    {
        /// <summary>
        /// 发现重复任务执行 
        /// </summary>
        Execute=1,

        /// <summary>
        ///  发现重复任务询问是否执行
        /// </summary>
        Ask=0
    }
}
