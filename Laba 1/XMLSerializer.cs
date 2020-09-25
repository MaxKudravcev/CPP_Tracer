using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using TracerLib;

namespace Laba_1
{
    class TraceResultXMLSerializer : ISerializer
    {
        public byte[] Serialize(TraceResult tr)
        {
            
            DataContractSerializer ser = new DataContractSerializer(typeof(TraceResult));
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineOnAttributes = true };
            using (var xmlWriter = XmlWriter.Create(ms, writerSettings))
                ser.WriteObject(xmlWriter, tr);
            
            return ms.ToArray();
        }
    }
}
