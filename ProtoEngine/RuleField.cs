using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    /// <summary>
    /// Odpowiada &lt;field ...>
    /// </summary>
    class RuleField : Rule
    {
        String name;
        Type type;
        Expression value;

        public RuleField(XmlNode node, Protocol p) {
            if (node.Attributes["name"] != null)
                name = node.Attributes["name"].Value;
            else
                name = "";
            type = Option.optionClasses[ node.Attributes["type"].Value.Split()[0] ];
            if (node.Attributes["value"] != null)
                value = Expression.fromString(node.Attributes["value"].Value);
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            Option opt;
            if (value == null) // No value to match
            { // Tworzę pustą opcję
                opt = (Option)Activator.CreateInstance(type, new object[] { name });
            }
            else
            {
                opt = value.eval(variables);
            }

            if (opt != null && opt.match(input))
            {
                Dictionary<String, Option> newEnv = new Dictionary<string, Option>(variables);
                newEnv.Add(opt.Name, opt);
                return newEnv;
            }
            else
            {
                return null;
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output)
        {
            if (value == null)
            {
                output = null;
                return null;
            }
            Option opt = value.eval(variables);
            byte[] bytes;
            output = new List<byte[]>();
            output.Add(bytes = opt.toBytes());
            if (bytes == null)
                return null;
            return variables;
        }
    }
}
