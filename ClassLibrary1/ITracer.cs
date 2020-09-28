using System;

namespace TracerLib

{
    /// <summary>
    /// An interface for the tracer
    /// </summary>
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();

        TraceResult GetTraceResult();
    }
}
