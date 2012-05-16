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

        public RuleBlock(XmlNode node, Protocol protocol)
        {
            if (node.Name == "conditional")
                failSilently = true;
            foreach (XmlNode child in node.ChildNodes)
            {
                if(child.Name[0] != '#')
                    rules.Add(Rule.fromXml(child, protocol));
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            input.startTransaction();
            Dictionary<String, Option> myEnv = variables;
            foreach (Rule rule in rules)
            {
                myEnv = rule.match(myEnv, input);
                if (myEnv == null)
                    break;
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
                input.commitTransaction();
                return myEnv;
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output)
        {
            Dictionary<String, Option> myEnv = new Dictionary<String, Option>(variables);
            output = new List<byte[]>();
            List<byte[]> list;
            foreach (Rule rule in rules)
            {
                myEnv = rule.match(myEnv, out list);
                if (myEnv == null)
                    return null;
                output.AddRange(list);
            }
            return myEnv;
        }
    }
}
