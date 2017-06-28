using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Messages.XmlHandling
{
    public class XmlRootReader
    {
        public Type GetMessageType(string xml)
        {
            //Class responsible for extracting root information from the message.
            var doc = XDocument.Parse(xml);
            var name = doc.Root.Name.LocalName;
            var type = Type.GetType("Xsd2." + name + ", Messages");
            return type;
        }
    }
}
