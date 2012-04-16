using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class OptionBool : Option
    {
        public bool Value { get; set; }

        OptionBool(String name, XmlNode node)
            : base(name)
        {
            try
            {
                String strValue = node.Attributes.GetNamedItem("value").Value;
                switch (strValue)
                {
                    case "true":
                        Value = true;
                        break;
                    case "false":
                        Value = false;
                        break;
                    default:
                        throw new NotSupportedException("Bad value for type bool: " + strValue);
                }
            }
            catch (Exception ex)
            {
                if (ex is System.ArgumentException ||
                    ex is System.InvalidOperationException)
                {
                    Value = false;
                }
                else
                {
                    throw;
                }
            }
        }

        OptionBool(String name, bool value)
            : base(name)
        {
            this.Value = value;
        }
    }
}
