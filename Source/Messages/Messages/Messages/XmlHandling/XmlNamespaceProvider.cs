using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Messages.XmlHandling
{
    public class XmlNamespaceProvider
    {
        public static XmlSerializerNamespaces GetNamespace()
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "https://se2.mini.pw.edu.pl/17-results/");
            return namespaces;
        }
    }
}
