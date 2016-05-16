///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2012-08-01 Created by pq
///功能说明：对网站中某些特定的Url(可设置特定的http参数)进行访问次数、平均处理时间、报错次数的监控,
///并将监控到的值映射为性能计数器，以供windows性能监视器或其他监控软件采集
///注意事项：需在web.config中配置对象名，和需要监控的节点
///修改记录：
///2012-08-04 [by cxh] 增加对wcf特定方法的调用监控
///2012-08-05 [by cxh] 增加对特定http请求参数的支持
///2012-08-15 [by pq]  监测点分离成类，增加对线程安全的支持
///2012-08-15 [by pq]  svc用xmlreader来读取，提高访问速度
///</summary>

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Text;

namespace sz1card1.Common.Diagnostics
{
    /// <summary>
    /// 监测点
    /// </summary>
    internal class MonitorPoint
    {
        /// <summary>
        /// 监测点实例名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 监测点相对url
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Url后缀(如aspx,ashx,svc,asmx等)
        /// </summary>
        public string Suffix
        {
            get;
            set;
        }

        /// <summary>
        /// Url过滤参数
        /// </summary>
        public Dictionary<string, string> Parameters
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
        /// 报错次数计数器
        /// </summary>
        public PerformanceCounter ErrorsPerSampleCounter
        {
            get;
            set;
        }
    }

    public class UrlPerformancor : IHttpModule
    {
        private static Dictionary<string, MonitorPoint> monitorPoints;

        static UrlPerformancor()
        {
            string prefix = "";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["urlPerformancorPrefix"]))
            {
                prefix = ConfigurationManager.AppSettings["urlPerformancorPrefix"];
            }
            monitorPoints = new Dictionary<string, MonitorPoint>();
            NameValueCollection collection = ConfigurationManager.GetSection("urlPerformancor") as NameValueCollection;
            foreach (string key in collection.AllKeys)
            {
                //初始化监测点对象
                string instanceName = string.IsNullOrEmpty(prefix) ? key : string.Format("{0}_{1}", prefix, key);
                MonitorPoint monitorPoint = new MonitorPoint()
                {
                    Name = instanceName,
                    RequestsPerSampleCounter = new PerformanceCounter("WebSite", "Requests Per Sample", instanceName, false),
                    AverageBaseCounter = new PerformanceCounter("WebSite", "AverageBase", instanceName, false),
                    AvgProcessTimeCounter = new PerformanceCounter("WebSite", "Avg. Process Time", instanceName, false),
                    ErrorsPerSampleCounter = new PerformanceCounter("WebSite", "Exceptions Per Sample", instanceName, false)
                };
                monitorPoint.RequestsPerSampleCounter.RawValue = 0;
                monitorPoint.AverageBaseCounter.RawValue = 0;
                monitorPoint.AvgProcessTimeCounter.RawValue = 0;
                monitorPoint.ErrorsPerSampleCounter.RawValue = 0;
                monitorPoint.Url = collection[key].Split('?')[0];
                //截取请求文件名后缀
                monitorPoint.Suffix = monitorPoint.Url.LastIndexOf('.') > 0 ? monitorPoint.Url.Substring(monitorPoint.Url.LastIndexOf('.') + 1).ToLower() : "";
                //处理http参数
                if (collection[key].Split('?').Length == 2)
                {
                    string param = collection[key].Split('?')[1];
                    string[] array = param.Split('&');
                    Dictionary<string, string> dictParams = new Dictionary<string, string>();
                    foreach (string keyValue in array)
                    {
                        if (keyValue.Contains('='))
                        {
                            dictParams.Add(keyValue.Split('=')[0], keyValue.Split('=')[1]);
                        }
                    }
                    monitorPoint.Parameters = dictParams;
                }
                monitorPoints.Add(instanceName, monitorPoint);
            }
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
            context.Error += new EventHandler(context_Error);
        }


        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            foreach (KeyValuePair<string, MonitorPoint> keyValue in monitorPoints)
            {
                //如果链接匹配
                MonitorPoint point = keyValue.Value;
                if (application.Context.Request.Url.LocalPath == point.Url)
                {
                    switch (point.Suffix)
                    {
                        case "svc"://wcf
                            if (point.Parameters != null && point.Parameters.Count > 0 && application.Context.Request.InputStream != null)
                            {
                                using (XmlReader reader = XmlReader.Create(application.Context.Request.InputStream))
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.Name == "a:Action")
                                        {
                                            string method = reader.ReadString();
                                            if (method.Contains(point.Parameters["method"]))
                                            {
                                                application.Context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                                                StartPerformance(point, application);
                                                return;
                                            }
                                            break;
                                        }
                                    }
                                }
                                application.Context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                            }
                            break;
                        default://aspx,ashx
                            bool isMatch = true;
                            if (point.Parameters != null)
                            {
                                foreach (KeyValuePair<string, string> keyValueParams in point.Parameters)
                                {
                                    if (keyValueParams.Value == "null")
                                    {
                                        isMatch = (application.Context.Request[keyValueParams.Key] == null);
                                        break;
                                    }
                                    else if (keyValueParams.Value != application.Context.Request[keyValueParams.Key])
                                    {
                                        isMatch = false;
                                        break;
                                    }
                                }
                            }
                            if (isMatch)
                            {
                                StartPerformance(point, application);
                                return;
                            }
                            break;
                    }
                }
            }
        }

        private void StartPerformance(MonitorPoint point, HttpApplication application)
        {
            point.AverageBaseCounter.Increment();
            point.RequestsPerSampleCounter.Increment();
            long start = Stopwatch.GetTimestamp();
            application.Context.Items.Add("UrlPerformancorKey", point.Name);
            application.Context.Items.Add("StartTimestamp", start);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application.Context.Items.Contains("UrlPerformancorKey"))
            {
                long start = (long)application.Context.Items["StartTimestamp"];
                long end = Stopwatch.GetTimestamp();
                monitorPoints[application.Context.Items["UrlPerformancorKey"].ToString()].AvgProcessTimeCounter.IncrementBy(end - start);
            }
        }

        void context_Error(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            if (application.Context.Items.Contains("UrlPerformancorKey"))
            {
                monitorPoints[application.Context.Items["UrlPerformancorKey"].ToString()].ErrorsPerSampleCounter.Increment();
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 安装计数器对象及计数器
        /// </summary>
        /// <param name="objName">对象名</param>
        public static void Install(string objName)
        {
            if (!PerformanceCounterCategory.Exists(objName))
            {
                CounterCreationDataCollection collection = new CounterCreationDataCollection();
                collection.Add(new CounterCreationData("Avg. Process Time", "", PerformanceCounterType.AverageTimer32));
                collection.Add(new CounterCreationData("AverageBase", "", PerformanceCounterType.AverageBase));
                collection.Add(new CounterCreationData("Requests Per Sample", "", PerformanceCounterType.CounterDelta32));
                collection.Add(new CounterCreationData("Exceptions Per Sample", "", PerformanceCounterType.CounterDelta32));
                PerformanceCounterCategory.Create(objName, "", PerformanceCounterCategoryType.MultiInstance, collection);
            }
        }

        /// <summary>
        /// 卸载计数器对象
        /// </summary>
        /// <param name="objName">对象名</param>
        public static void UnInstall(string objName)
        {
            if (PerformanceCounterCategory.Exists(objName))
            {
                PerformanceCounterCategory.Delete(objName);
            }
        }
    }
}