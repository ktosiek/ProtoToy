using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace ProtoEngine
{
    abstract class Rule
    {
        public static Rule fromXml(XmlNode node)
        {
            throw new NotImplementedException();//return new Rule(node);
        }

        public static Rule empty()
        {
            throw new NotImplementedException();//return new Rule(node);
        }

        abstract public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input);
    }
}
