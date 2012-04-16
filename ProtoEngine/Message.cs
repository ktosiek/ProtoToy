using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProtoEngine
{
    public enum MessageType { Request, Response, Bidirectional };
    public class Message
    {
        public static Message fromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }
        

        private MessageType type;
        public MessageType Type { get { return this.type; } }
    }
}
