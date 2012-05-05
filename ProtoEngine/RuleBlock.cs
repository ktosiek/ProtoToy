using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleBlock : Rule
    {
        List<Rule> rules;
        bool failSilently = false;

        public RuleBlock(XmlNode node)
        {
            if (node.Name == "conditional")
                failSilently = true;
            foreach(XmlNode child in node.ChildNodes)
                rules.Add(Rule.fromXml(child));
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
    }
}
