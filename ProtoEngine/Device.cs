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
        private List<Transaction> transactions;

        public Device(Device dev)
        {
            name = dev.name;
            transactions = dev.transactions;
        }

        public Device(String name, XmlNode deviceNode)
        {
            foreach (XmlNode node in deviceNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "options":
                        foreach (XmlNode optNode in node.ChildNodes)
                        {
                            Option opt = Option.fromXml(node);
                            options.Add(opt);
                        }
                        break;
                    case "responds_when":
                        responds_when = Rule.fromXml(node);
                        break;
                    case "transactions":
                        foreach (XmlNode tNode in node.ChildNodes)
                        {
                            Transaction t = Transaction.fromXml(node);
                            transactions.Add(t);
                        }
                        break;
                    default:
                        throw new NotSupportedException("Unknown node: " + node.Name);
                }
            }
        }

        public static Device fromXml(XmlNode deviceNode)
        {
            return new Device(deviceNode.Attributes["name"].Value, deviceNode);
        }

    }
}
