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
            throw new NotImplementedException();
        }
        

        private MessageType type;
        public MessageType Type { get { return this.type; } }
    }
}
