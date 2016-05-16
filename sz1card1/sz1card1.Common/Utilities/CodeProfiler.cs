using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;

namespace sz1card1.Common
{
    public class CodeProfiler
    {
        private StringBuilder result;
        private Stopwatch watch;

        public CodeProfiler()
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;
            result = new StringBuilder();
            //Profiling(string.Empty, () => { });
            watch = new Stopwatch();
            watch.Start();
        }

        public T Profiling<T>(string name, Func<T> fun)
        {
            var value = default(T);
            //1.
            //if (string.IsNullOrEmpty(name)) return value;

            // 2.
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            //int[] gcCounts = new int[GC.MaxGeneration + 1];
            //for (int i = 0; i <= GC.MaxGeneration; i++)
            //{
            //    gcCounts[i] = GC.CollectionCount(i);
            //}

            // 3.
            //ulong cycleCount = GetCycleCount();
            long startElapsed = watch.ElapsedMilliseconds;
            value = fun();
            long endElapsed = watch.ElapsedMilliseconds;
            //ulong cpuCycles = GetCycleCount() - cycleCount;

            // 4.
            result.AppendLine(string.Format("{0}:", name));
            result.AppendLine(string.Format("Time Elapsed:{0}ms", (endElapsed-startElapsed).ToString("N0")));
            //result.AppendLine(string.Format("CPU Cycles:{0}", cpuCycles.ToString("N0")));

            // 5.
            //for (int i = 0; i <= GC.MaxGeneration; i++)
            //{
            //    int count = GC.CollectionCount(i) - gcCounts[i];
            //    result.AppendLine(string.Format("Gen{0}:{1}", i, count));
            //}
            result.AppendLine();
            return value;
        }

        public void Profiling(string name, Action action)
        {
            Profiling(name, () =>
            {
                action();
                return string.Empty;
            });
        }

        public string GetResult()
        {
            return result.ToString();
        }

        public long GetCurrentElapsed()
        {
            return watch.ElapsedMilliseconds;
        }

        public static CodeProfiler Current
        {
            get
            {
                if (CallContext.GetData("CodeProfiler") == null)
                {
                    CallContext.SetData("CodeProfiler", new CodeProfiler());
                }
                return (CodeProfiler)CallContext.GetData("CodeProfiler");
            }
        }

        //private static ulong GetCycleCount()
        //{
        //    ulong cycleCount = 0;
        //    QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
        //    return cycleCount;
        //}

        //[DllImport("kernel32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        //[DllImport("kernel32.dll")]
        //static extern IntPtr GetCurrentThread();
    }
}
