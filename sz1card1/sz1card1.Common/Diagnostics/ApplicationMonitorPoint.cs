using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Diagnostics
{
    /// <summary>
    /// 应用程序监测点
    /// </summary>
    internal class ApplicationMonitorPoint
    {
        /// <summary>
        /// 监测点实例值
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// 监测点实例名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 监测点实例相关参数
        /// </summary>
        public List<string> Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// 请求次数计数器
        /// </summary>
        public PerformanceCounter RequestsPerSampleCounter
        {
            get;
            set;
        }

        /// <summary>
        /// 平均计数器分母
        /// </summary>
        public PerformanceCounter AverageBaseCounter
        {
            get;
            set;
        }

        /// <summary>
        /// 平均处理时间计数器
        /// </summary>
        public PerformanceCounter AvgProcessTimeCounter
        {
            get;
            set;
        }

        /// <summary>
        /// 预警次数计数器
        /// </summary>
        public PerformanceCounter WarningsPerSampleCounter
        {
            get;
            set;
        }

        /// <summary>
        /// 报错次数计数器
        /// </summary>
        public PerformanceCounter ErrorsPerSampleCounter
        {
            get;
            set;
        }
    }
}
