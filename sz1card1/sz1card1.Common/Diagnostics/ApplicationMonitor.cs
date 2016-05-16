using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sz1card1.Common.Diagnostics
{
    /// <summary>
    /// 应用程序监控类
    /// </summary>
    public class ApplicationMonitor
    {
        private long requestTimeWarningValue = 3000;
        private List<MonitorContext> contextCollection = null;
        private Dictionary<string, ApplicationMonitorPoint> monitorPoints = null;

        /// <summary>
        /// 请求处理时间预警值(单位:毫秒)
        /// </summary>
        public long RequestTimeWarningValue
        {
            get
            {
                return requestTimeWarningValue;
            }
            set
            {
                requestTimeWarningValue = value;
            }
        }

        public ApplicationMonitor()
        {
            contextCollection = new List<MonitorContext>();
            monitorPoints = new Dictionary<string, ApplicationMonitorPoint>();
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        public void Start()
        {
            NameValueCollection collection = ConfigurationManager.GetSection("monitorPoints") as NameValueCollection;
            InitMonitorPoints(collection);
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        /// <param name="collection"></param>
        public void Start(NameValueCollection collection)
        {
            InitMonitorPoints(collection);
        }

        /// <summary>
        /// 初始化监控指标
        /// </summary>
        /// <param name="collection"></param>
        private void InitMonitorPoints(NameValueCollection collection)
        {
            if (collection != null)
            {
                string applicationName = Application.ProductName;
                foreach (string name in collection.AllKeys)
                {
                    ApplicationMonitorPoint monitorPoint = new ApplicationMonitorPoint()
                    {
                        Name = name,
                        Key = collection[name],
                        RequestsPerSampleCounter = new PerformanceCounter(applicationName, "Requests Per Sample", name, false),
                        AverageBaseCounter = new PerformanceCounter(applicationName, "AverageBase", name, false),
                        AvgProcessTimeCounter = new PerformanceCounter(applicationName, "Avg. Process Time", name, false),
                        ErrorsPerSampleCounter = new PerformanceCounter(applicationName, "Exceptions Per Sample", name, false),
                        WarningsPerSampleCounter = new PerformanceCounter(applicationName, "Warnings Per Sample", name, false)
                    };
                    monitorPoint.RequestsPerSampleCounter.RawValue = 0;
                    monitorPoint.AverageBaseCounter.RawValue = 0;
                    monitorPoint.AvgProcessTimeCounter.RawValue = 0;
                    monitorPoint.ErrorsPerSampleCounter.RawValue = 0;
                    monitorPoint.WarningsPerSampleCounter.RawValue = 0;
                    monitorPoints.Add(collection[name], monitorPoint);
                }
            }
        }

        public void StartPerformance(string identity, string action, object[] parameters)
        {
            long start = Stopwatch.GetTimestamp();
            if (monitorPoints.Keys.Count<string>(s => s == action) > 0)
            {
                monitorPoints[action].AverageBaseCounter.Increment();
                monitorPoints[action].RequestsPerSampleCounter.Increment();
                MonitorContext context = new MonitorContext(identity, action);
                context.RequestParameters.Add(identity, parameters);
                context.RequestParameters.Add("StartTimestamp", start);
                AddMonitorContext(context);
            }
            if (monitorPoints.Keys.Count<string>(s => s == "*") > 0)
            {
                monitorPoints["*"].AverageBaseCounter.Increment();
                monitorPoints["*"].RequestsPerSampleCounter.Increment();
                MonitorContext context = new MonitorContext(identity, "*");
                context.RequestParameters.Add(identity, parameters);
                context.RequestParameters.Add("StartTimestamp", start);
                AddMonitorContext(context);
            }

        }

        public void EndPerformance(string identity)
        {
            List<MonitorContext> contexts = null;
            lock (contextCollection)
            {
                contexts = contextCollection.Where<MonitorContext>(m => m.RequestIdentity == identity).ToList();
            }
            for (int i = 0; i < contexts.Count(); i++)
            {
                long start = (long)contexts[i].RequestParameters["StartTimestamp"];
                long end = Stopwatch.GetTimestamp();
                long requestValue = (end - start) / Stopwatch.Frequency * 1000;
                monitorPoints[contexts[i].RequestKeyName].AvgProcessTimeCounter.IncrementBy(end - start);
                if (requestValue > requestTimeWarningValue)
                {
                    WarningPerformance(contexts[i]);
                }
                DeleteMonitorContext(contexts[i]);
            }
        }

        public void ErrorPerformance(string identity)
        {
            List<MonitorContext> contexts = null;
            lock (contextCollection)
            {
                contexts = contextCollection.Where<MonitorContext>(m => m.RequestIdentity == identity).ToList();
            }
            for (int i = 0; i < contexts.Count(); i++)
            {
                monitorPoints[contexts[i].RequestKeyName].ErrorsPerSampleCounter.Increment();
                DeleteMonitorContext(contexts[i]);
            }
        }

        private void WarningPerformance(MonitorContext context)
        {
            monitorPoints[context.RequestKeyName].WarningsPerSampleCounter.Increment();
            object[] parameters = context.RequestParameters[context.RequestIdentity.ToString()] as object[];
            if (parameters != null)
            {
                string parameterInfo = string.Empty;
                foreach (object obj in parameters)
                {
                    if (obj != null)
                    {
                        parameterInfo += obj.ToString() + ",";
                    }
                }
                if (parameterInfo.Length > 0)
                {
                    parameterInfo = parameterInfo.Remove(parameterInfo.Length - 1);
                }
                string warningInfo = string.Format("{0}处理时间超过预警值({1}毫秒)\n参数:{2}", monitorPoints[context.RequestKeyName].Name, requestTimeWarningValue, parameterInfo);
                sz1card1.Common.Log.LoggingService.Info(warningInfo);
            }
        }

        private void AddMonitorContext(MonitorContext context)
        {
            lock (contextCollection)
            {
                contextCollection.Add(context);
            }
        }

        private void DeleteMonitorContext(MonitorContext context)
        {
            lock (contextCollection)
            {
                contextCollection.Remove(context);
            }
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void Stop()
        {
            lock (contextCollection)
            {
                contextCollection.Clear();
            }
            lock (monitorPoints)
            {
                monitorPoints.Clear();
            }
        }

        /// <summary>
        /// 安装计数器对象及计数器
        /// </summary>
        public static void Install()
        {
            string objName = Application.ProductName;
            if (!PerformanceCounterCategory.Exists(objName))
            {
                CounterCreationDataCollection collection = new CounterCreationDataCollection();
                collection.Add(new CounterCreationData("Avg. Process Time", "", PerformanceCounterType.AverageTimer32));
                collection.Add(new CounterCreationData("AverageBase", "", PerformanceCounterType.AverageBase));
                collection.Add(new CounterCreationData("Requests Per Sample", "", PerformanceCounterType.CounterDelta32));
                collection.Add(new CounterCreationData("Exceptions Per Sample", "", PerformanceCounterType.CounterDelta32));
                collection.Add(new CounterCreationData("Warnings Per Sample", "", PerformanceCounterType.CounterDelta32));
                PerformanceCounterCategory.Create(objName, "", PerformanceCounterCategoryType.MultiInstance, collection);
            }
        }

        /// <summary>
        /// 卸载计数器对象
        /// </summary>
        public static void UnInstall()
        {
            string objName = Application.ProductName;
            if (PerformanceCounterCategory.Exists(objName))
            {
                PerformanceCounterCategory.Delete(objName);
            }
        }
    }
}
