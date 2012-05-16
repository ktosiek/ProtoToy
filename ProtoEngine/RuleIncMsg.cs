using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleIncMsg : Rule
    {
        Protocol p;
        public RuleIncMsg(XmlNode node, Protocol proto)
        {
            p = proto;
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            throw new NotImplementedException();
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output)
        {
            throw new NotImplementedException();
        }
    }
}
