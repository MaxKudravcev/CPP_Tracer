using TracerLib;

namespace Laba_1
{
    public interface ISerializer
    {
        byte[] Serialize(TraceResult tr);
    }
}
