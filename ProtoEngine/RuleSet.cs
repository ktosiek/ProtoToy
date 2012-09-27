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

        private Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<Option> fields)
        {
            Dictionary<String, Option> newEnv = new Dictionary<string, Option>(variables);
            Option opt = value.eval(variables);
            opt.Name = name;
            newEnv[name] = opt;
            fields = null;
            return newEnv;
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input,
            out List<Option> fields)
        {
            return match(variables, out fields);
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output,
            out List<Option> fields)
        {
            output = null;
            return match(variables, out fields);
        }
    }
}
