using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleSet : Rule
    {
        Expression value;
        String name;

        public RuleSet(XmlNode node, Protocol proto)
        {
            name = node.Attributes["name"].Value;
            value = Expression.fromString(node.Attributes["value"].Value);
        }

        private Dictionary<String, Option> match(Dictionary<String, Option> variables)
        {
            Dictionary<String, Option> newEnv = new Dictionary<string, Option>(variables);
            Option opt = value.eval(variables);
            newEnv.Add(name, opt);
            return newEnv;
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            return match(variables);
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output)
        {
            output = null;
            return match(variables);
        }
    }
}
