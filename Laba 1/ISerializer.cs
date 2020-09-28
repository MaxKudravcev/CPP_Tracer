using TracerLib;

namespace Laba_1
{
    /// <summary>
    /// An interface for the TraceResult serializers
    /// </summary>
    public interface ISerializer
    {
        byte[] Serialize(TraceResult tr);
    }
}
