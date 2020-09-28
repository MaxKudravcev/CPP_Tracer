using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TracerLib
{
    /// <summary>
    /// Tracer for measuring methods execution speed and building call-trees
    /// </summary>
    public class Tracer : ITracer
    {
        #region Private fields

        private TraceResult traceResult = new TraceResult();
        private ConcurrentDictionary<int, Stack<MethodTraceResult>> threadStacks = new ConcurrentDictionary<int, Stack<MethodTraceResult>>();

        #endregion

        #region Public methods

        /// <summary>
        /// Get tracer results
        /// </summary>
        /// <returns>An instance of "TraceResult"</returns>
        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        /// <summary>
        /// Starts tracing of the method from which this method is called
        /// </summary>
        public void StartTrace()
        {
            MethodTraceResult method = new MethodTraceResult();

            ThreadTraceResult thread = new ThreadTraceResult();
            threadStacks.TryAdd(thread.ID, new Stack<MethodTraceResult>());
            threadStacks[thread.ID].Push(method);
                   
            method.StartStopwatch();
        }
        
        /// <summary>
        /// Stops tracing of current method, adds the result to the traceResult field
        /// </summary>
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

        #endregion
    }
}
