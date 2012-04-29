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
        Option value;

        public RuleField(XmlNode node) {
            name = node.Attributes["name"].Value;
            type = Option.optionClasses[ node.Attributes["type"].Value ];
            try
            {
                value = Option.fromXml(node);
            } catch(Exception ex) { throw; }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            return variables;
        }
    }
}
