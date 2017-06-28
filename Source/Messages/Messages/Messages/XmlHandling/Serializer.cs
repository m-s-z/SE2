using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Messages.XmlHandling
{
    public class Serializer
    {
        private object _msg;
        private XmlSerializer _serializer;
        public Serializer(object msg)
        {
            _msg = msg;
            _serializer = new XmlSerializer(msg.GetType());
        }
        public string Serialize()
        {
            using (var writer = new Utf8StringWriter())
            {
                _serializer.Serialize(writer, _msg, XmlNamespaceProvider.GetNamespace());
                return writer.ToString();
            }
        }
        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
    }
}
