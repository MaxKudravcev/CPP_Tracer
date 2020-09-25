using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TracerLib
{
    public class Tracer : ITracer
    {
        private TraceResult traceResult = new TraceResult();
        private ConcurrentDictionary<int, Stack<MethodTraceResult>> threadStacks = new ConcurrentDictionary<int, Stack<MethodTraceResult>>();

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
            MethodTraceResult method = new MethodTraceResult();

            ThreadTraceResult thread = new ThreadTraceResult();
            threadStacks.TryAdd(thread.ID, new Stack<MethodTraceResult>());
            threadStacks[thread.ID].Push(method);
                   
            method.StartStopwatch();
        }
        
        public void StopTrace()
        {
            var method = threadStacks[Thread.CurrentThread.ManagedThreadId].Pop();
            method.StopStopwatch();

            if (threadStacks[Thread.CurrentThread.ManagedThreadId].Count > 0)
                threadStacks[Thread.CurrentThread.ManagedThreadId].Peek().AddMethod(method);
            else
            {
                ThreadTraceResult thread = traceResult.Threads.Find(t => t.ID == Thread.CurrentThread.ManagedThreadId);
                if (thread == null)
                {
                    thread = new ThreadTraceResult();
                    traceResult.AddThread(thread);
                }
                thread.AddMethod(method); 
            }
        }
    }
}
