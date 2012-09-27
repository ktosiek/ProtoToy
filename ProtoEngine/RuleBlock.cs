using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleBlock : Rule
    {
        List<Rule> rules = new List<Rule>();
        bool failSilently = false;
        String name;

        public RuleBlock(XmlNode node, Protocol protocol)
        {
            if (node.Name == "conditional")
                failSilently = true;
            foreach (XmlNode child in node.ChildNodes)
            {
                if(child.Name[0] != '#')
                    rules.Add(Rule.fromXml(child, protocol));
            }
            if (node.Attributes["name"] != null)
                name = node.Attributes["name"].Value;
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input,
            out List<Option> fields)
        {
            input.startTransaction();
            Dictionary<String, Option> myEnv = variables;
            fields = new List<Option>();
            foreach (Rule rule in rules)
            {
                List<Option> newFields;
                myEnv = rule.match(myEnv, input, out newFields);
                if (myEnv == null)
                    break;
                if (newFields != null)
                    fields.AddRange(newFields);
            }
            if (myEnv == null)
            {
                input.cancelTransaction();
                if (failSilently)
                    return variables;
                else
                    return null;
            }
            else
            {
                if (this.name != null)
                    myEnv[name] = new OptionArray(name, input.getTransactionData());
                input.commitTransaction();
                return myEnv;
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output,
            out List<Option> fields)
        {
            Dictionary<String, Option> myEnv = new Dictionary<String, Option>(variables);
            output = new List<byte[]>();
            List<byte[]> newOutput;
            fields = new List<Option>();
            foreach (Rule rule in rules)
            {
                List<Option> newFields;
                myEnv = rule.match(myEnv, out newOutput, out newFields);
                if (myEnv == null)
                    return null;
                if (newOutput != null)
                    output.AddRange(newOutput);
                if (newFields != null)
                    fields.AddRange(newFields);
            }

            if (this.name != null)
            {
                List<char> data = new List<char>();
                foreach (byte[] bs in output)
                    foreach (byte b in bs)
                        data.Add((char)b);
                myEnv[name] = new OptionArray(name, data);
            }
            return myEnv;
        }
    }
}
