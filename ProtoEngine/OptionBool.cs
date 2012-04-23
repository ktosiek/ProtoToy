using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    public class OptionBool : Option
    {
        public delegate void OptionBoolChangeHandler(OptionBool option);

        private bool myValue;
        public bool Value
        {
            get { return myValue; }
            set { myValue = value; OptionBoolChanged(this); }
        }
        public OptionBoolChangeHandler OptionBoolChanged;

        public OptionBool(String name, XmlNode node)
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

        public OptionBool(String name, bool value)
            : base(name)
        {
            this.Value = value;
        }
    }
}
