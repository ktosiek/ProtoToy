using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleIsSet : Rule
    {
        bool reverse;
        String variable;

        public RuleIsSet(XmlNode node, Protocol protocol)
        {
            if (node.Name == "isnset")
                reverse = true;
            else
                reverse = false;
            variable = node.Attributes["name"].Value;
        }

        public override Dictionary<string, Option> match(Dictionary<string, Option> variables, TransactionalStreamReader input, out List<Option> fields)
        {
            fields = null;
            if (reverse ^ variables.ContainsKey(variable))
                return variables;
            else
                return null;
        }

        public override Dictionary<string, Option> match(Dictionary<string, Option> variables, out List<byte[]> output, out List<Option> fields)
        {
            fields = null;
            output = null;
            if (reverse ^ variables.ContainsKey(variable))
                return variables;
            else
                return null;
        }
    }
}
