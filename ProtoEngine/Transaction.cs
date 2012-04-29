using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    public class Transaction
    {
        public static Transaction fromXml(XmlNode node)
        {
            return new Transaction(node);
        }

        public Transaction(XmlNode node)
        {

        }
    }
}
