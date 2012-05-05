using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProtoEngine
{
    public enum MessageType { Request, Response, Bidirectional };
    /// <summary>
    /// Klasa odpowiadająca opisowi wiadomości (nie wiadomości jako takiej!)
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Buduje obiekt Message na podstawie opisu w XML
        /// </summary>
        /// <param name="node">opis wiadomości jako element XML DOM</param>
        /// <returns>zbudowana wiadomość</returns>
        public static Message fromXml(XmlNode node)
        {
            MessageType type;
            switch (node.Attributes["type"].Value)
            {
                case "request":
                    type = MessageType.Request;
                    break;
                case "response":
                    type = MessageType.Response;
                    break;
                case "bidir":
                    type = MessageType.Bidirectional;
                    break;
                default:
                    throw new ArgumentException("Nieznany rodzaj wiadomości: " + node.Attributes["type"].Value);
            }

            String name = null;
            if(node.Attributes["name"] != null)
                name = node.Attributes["name"].Value;
            return new Message(name, type, node);
        }
        

        private MessageType type;
        public MessageType Type { get { return this.type; } }
        private string name;
        public string Name { get { return name; } }
        private Rule rule;

        public Message()
        {
            rule = Rule.empty();
        }

        public Message(String name, MessageType type, XmlNode node)
        {
            this.type = type;
            this.name = name;
            rule = Rule.fromXml(node);
        }

        public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            return rule.match(variables, input);
        }
    }
}
