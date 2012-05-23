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
            TransactionalStreamReader input,
            out List<Option> fields)
        {
            List<Message> exclude = new List<Message>(p.excludedSoFar);
            exclude.Add(p.currentMessage);
            return p.matchIncoming(variables, input, exclude, out fields);
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output,
            out List<Option> fields)
        {
            List<Message> exclude = new List<Message>(p.excludedSoFar);
            exclude.Add(p.currentMessage);
            return p.matchOutgoing(variables, exclude, out output, out fields);
        }
    }
}
