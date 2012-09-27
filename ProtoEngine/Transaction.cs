using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace ProtoEngine
{
    public class Transaction
    {
        private Rule rule;
        public static Transaction fromXml(XmlNode node, Protocol protocol)
        {
            return new Transaction(node, protocol);
        }

        public Transaction(XmlNode node, Protocol protocol)
        {
            rule = new RuleBlock(node, protocol);
        }

        public Dictionary<String, Option> match(Dictionary<String, Option> variables)
        {
            // Transactions don't eat data
            TransactionalStreamReader mock = new TransactionalStreamReader(new MemoryStream());
            List<Option> fields;
            return rule.match(variables, mock, out fields);
        }
    }
}
