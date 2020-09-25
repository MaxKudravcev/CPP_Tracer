using System;

namespace TracerLib
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();

        TraceResult GetTraceResult();
    }
}
