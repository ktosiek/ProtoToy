using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProtoEngine
{
    public class Device
    {
        private String name;
        public String Name { get { return name; } }
        private List<Option> options = new List<Option>();
        public List<Option> Options { get { return options; } }
        private Rule responds_when = Rule.empty();
        public Rule RespondsWhen { get { return responds_when; } }
        private List<Transaction> transactions = new List<Transaction>();
        public List<Transaction> Transactions { get { return transactions; } }

        public Device(Device dev)
        {
            name = dev.name;
            transactions = dev.transactions;
            options = new List<Option>();
            foreach (Option o in dev.options)
                options.Add(o.copy());
            responds_when = dev.responds_when;
        }

        public Device(String name, XmlNode deviceNode, Protocol protocol)
        {
            foreach (XmlNode node in deviceNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "options":
                        foreach (XmlNode optNode in node.ChildNodes)
                        {
                            Option opt = Option.fromXml(optNode);
                            options.Add(opt);
                        }
                        break;
                    case "responds_when":
                        responds_when = Rule.fromXml(node, protocol);
                        break;
                    case "transactions":
                        foreach (XmlNode tNode in node.ChildNodes)
                        {
                            Transaction t = Transaction.fromXml(tNode);
                            transactions.Add(t);
                        }
                        break;
                    default:
                        throw new NotSupportedException("Unknown node: " + node.Name);
                }
            }
        }

        public static Device fromXml(XmlNode deviceNode, Protocol protocol)
        {
            return new Device(deviceNode.Attributes["name"].Value, deviceNode, protocol);
        }

    }
}
