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
        String type;
        Option option;
        Expression value;

        public RuleField(XmlNode node, Protocol p) {
            if (node.Attributes["name"] != null)
                name = node.Attributes["name"].Value;
            else
                name = "";
            
            type = node.Attributes["type"].Value;

            if (node.Attributes["value"] != null)
            {
                value = Expression.fromString(node.Attributes["value"].Value);
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input,
            out List<Option> fields)
        {
            Option opt;
            if (value == null) // No value to match
            { // Tworzę pustą opcję
                opt = (Option)Activator.CreateInstance(
                    Option.optionClasses[type.Split()[0]],
                    new object[] { name, type });
            }
            else
            {
                opt = value.eval(variables);
            }

            if (opt != null && opt.match(input))
            {
                opt.Name = this.name;
                Dictionary<String, Option> newEnv = new Dictionary<string, Option>(variables);
                newEnv.Add(opt.Name, opt);
                fields = new List<Option>();
                fields.Add(opt);
                return newEnv;
            }
            else
            {
                fields = null;
                return null;
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output,
            out List<Option> fields)
        {
            fields = null;
            if (value == null)
            {
                output = null;
                if (variables.ContainsKey(name))
                {
                    fields = new List<Option>();
                    fields.Add(variables[name]);
                    output = new List<byte[]>();
                    output.Add(variables[name].toBytes());
                    return variables;
                }
                else
                    return null;
            }
            Option opt = value.eval(variables);
            byte[] bytes;
            output = new List<byte[]>();
            output.Add(bytes = opt.toBytes());
            if (bytes == null)
                return null;
            fields = new List<Option>();
            opt.Name = this.name;
            fields.Add(opt);
            return variables;
        }
    }
}
