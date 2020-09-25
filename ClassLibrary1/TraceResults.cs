using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TracerLib
{
    [DataContract(Name = "Method")]
    public class MethodTraceResult
    {
        private Stopwatch stopwatch = new Stopwatch();
        [DataMember(Order = 0)]
        public string Name { get; private set; }
        [DataMember(Order = 1)]
        public string Class { get; private set; }
        [DataMember(Order = 3)]
        public long Time { get; private set; }
        [DataMember(Order = 2)]
        public List<MethodTraceResult> Methods { get; private set; } = new List<MethodTraceResult>();

        internal void AddMethod(MethodTraceResult method) => Methods.Add(method);
        internal void StartStopwatch() => stopwatch.Start();
        internal void StopStopwatch()
        {
            stopwatch.Stop();
            Time = stopwatch.ElapsedMilliseconds;
        }

        public MethodTraceResult()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);

            Name = sf.GetMethod().Name;
            Class = sf.GetMethod().DeclaringType.Name;
        }

        public MethodTraceResult(string className, string name)
        {
            Class = className;
            Name = name;
        }
    }

    [DataContract(Name = "Thread")]
    public class ThreadTraceResult 
    {
        [DataMember(Order = 0)]
        public int ID { get; private set; }

        [DataMember(Order = 2)]
        public long Time {
            get
            {
                long x = 0;
                foreach(MethodTraceResult method in Methods)
                    x += method.Time;
                return x;
            }
            private set { }
        }

        [DataMember(Order = 1)]
        public List<MethodTraceResult> Methods { get; private set; } = new List<MethodTraceResult>();

        public void AddMethod(MethodTraceResult method) => Methods.Add(method);

        public ThreadTraceResult()
        {
            ID = Thread.CurrentThread.ManagedThreadId;
        }
    }

    [DataContract]
    public class TraceResult
    {
        [DataMember]
        public List<ThreadTraceResult> Threads { get; private set; } = new List<ThreadTraceResult>();

        internal void AddThread(ThreadTraceResult thread) => Threads.Add(thread);

    }
}
