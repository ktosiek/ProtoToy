using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    public abstract class Option
    {
        private static Dictionary<String, Type> optionClasses = new Dictionary<String, Type>() {
            {"bool", typeof(OptionBool)}
        };

        public static Option fromXml(XmlNode node)
        {
            String name = node.Attributes.GetNamedItem("name").Value;
            return (Option)optionClasses[
                node.Attributes.GetNamedItem("type").Value
                ].GetConstructor(new Type[] { typeof(String), typeof(XmlNode) })
                .Invoke(new object[] { name, node });
        }

        public String Name { get; set; }

        public void setFrom(Option opt)
        {
            if (this.GetType() != opt.GetType())
            {
                throw new NotSupportedException(); // TODO
            }
        }

        public Option(String name) {
            Name = name;
        }
    }
}
