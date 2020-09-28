using Newtonsoft.Json;
using System.Text;
using TracerLib;

namespace Laba_1
{
    /// <summary>
    /// JSON serializer for TraceResult class
    /// </summary>
    class JSONSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the given TraceResult
        /// </summary>
        /// <param name="tr">TraceResult</param>
        /// <returns>JSON serialization result as a byte array</returns>
        public byte[] Serialize(TraceResult tr)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tr, Formatting.Indented));
        }
    }
}
