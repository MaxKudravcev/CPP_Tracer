using System.IO;

namespace Laba_1
{
    /// <summary>
    /// Helper class for outputting serialization result to the given stream
    /// </summary>
    class TraceResultOutput
    {
        /// <summary>
        /// Writes serialization result to the given stream
        /// </summary>
        /// <param name="bytes">The result of serializing</param>
        /// <param name="outStream">Stream to which the result will be outputted to</param>
        public static void WriteToStream(byte[] bytes, Stream outStream)
        {
            outStream.Write(bytes, 0, bytes.Length);
        }
    }
}
