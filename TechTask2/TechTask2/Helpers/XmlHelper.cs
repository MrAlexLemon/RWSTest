using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using TechTask2.Models;

namespace TechTask2.Helpers
{
    public static class XmlHelper
    {
        public static List<Node> ParseXML(string xmlContent)
        {
            using (var reader = XDocument.Load(xmlContent).CreateReader())
            {
                var rootNodes = new List<Node>();
                var nodeStack = new Stack<Node>();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            var node = new Node { Name = reader.Name };
                            if (reader.MoveToFirstAttribute())
                            {
                                // Read the attributes
                                do
                                {
                                    node.Attributes.Add(reader.Name, reader.Value);
                                }
                                while (reader.MoveToNextAttribute());
                                // Move back to element
                                reader.MoveToElement();
                            }
                            var nodes = nodeStack.Count > 0 ? nodeStack.Peek().Children : rootNodes;
                            nodes.Add(node);
                            if (!reader.IsEmptyElement)
                                nodeStack.Push(node);
                            break;

                        case XmlNodeType.Text:
                            nodeStack.Peek().Text = reader.Value;
                            break;

                        case XmlNodeType.EndElement:
                            nodeStack.Pop();
                            break;
                    }
                }
                return rootNodes;
            }
        }
        public static void SerializeToXML(List<Node> test, string xmlFilePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                NewLineHandling = NewLineHandling.None,
                Indent = true,
                NewLineChars = "\n"
            };

            using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
            {
                Action<Node> traverse = null;

                traverse = (n) =>
                {
                    writer.WriteStartElement(n.Name);

                    foreach (var attribute in n.Attributes)
                        writer.WriteAttributeString(attribute.Key, attribute.Value);

                    if (!string.IsNullOrEmpty(n.Text))
                        writer.WriteValue(n.Text);

                    n.Children.ForEach(traverse);
                    writer.WriteEndElement();
                };


                writer.WriteStartDocument();

                foreach (var item in test)
                {
                    traverse(item);
                }
            }
        }
    }
}
