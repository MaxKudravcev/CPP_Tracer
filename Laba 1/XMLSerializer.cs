using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using TracerLib;

namespace Laba_1
{
    /// <summary>
    /// XML serializer for TraceResult class
    /// </summary>
    class TraceResultXMLSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the given TraceResult
        /// </summary>
        /// <param name="tr">TraceResult</param>
        /// <returns>XML serialization result as a byte array</returns>
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
