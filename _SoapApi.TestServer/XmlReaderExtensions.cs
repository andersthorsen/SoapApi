using System;
using System.Xml;

namespace SoapApi.TestServer
{
    public static class XmlReaderExtensions
    {
        public static bool ReadUntilElement(this XmlReader reader)
        {
            if (!reader.Read())
                return false;

            while (reader.NodeType != XmlNodeType.Element)
            {
                if (!reader.Read())
                    return false;
            }

            return reader.NodeType == XmlNodeType.Element;
        }

        public static bool ReadUntilElement(this XmlReader reader, string ns, string localName)
        {

            var namespaceUri = new Uri(ns);

            if (!reader.ReadUntilElement())
                return false;

            while (reader.LocalName != localName || new Uri(reader.NamespaceURI) != namespaceUri)
            {
                if (!reader.ReadUntilElement())
                    return false;
            }

            return (reader.LocalName == localName && new Uri(reader.NamespaceURI) == namespaceUri);
        }
    }
}