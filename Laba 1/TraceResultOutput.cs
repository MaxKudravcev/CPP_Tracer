using System.IO;

namespace Laba_1
{
    class TraceResultOutput
    {
        public static void WriteToStream(byte[] bytes, Stream outStream)
        {
            outStream.Write(bytes, 0, bytes.Length);
        }
    }
}
