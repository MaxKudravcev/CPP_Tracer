using Newtonsoft.Json;
using System.Text;
using TracerLib;

namespace Laba_1
{
    class JSONSerializer : ISerializer
    {
        public byte[] Serialize(TraceResult tr)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tr, Formatting.Indented));
        }
    }
}
