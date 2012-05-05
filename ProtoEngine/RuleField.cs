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

        public RuleField(XmlNode node) {
            name = node.Attributes["name"].Value;
            type = Option.optionClasses[ node.Attributes["type"].Value ];
            if (node.Attributes["value"] != null)
                value = Expression.fromString(node.Attributes["value"].Value);
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            Option opt;
            if (value == null)
            {
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
    }
}
