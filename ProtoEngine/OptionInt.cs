using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    public class OptionInt : Option
    {
        public delegate void OptionIntChangeHandler(OptionInt option);

        private int myValue;
        public int Value
        {
            get { return myValue; }
            set {
                if (value > maxV || value < minV)
                    throw new ArgumentOutOfRangeException();
                myValue = value;
                OptionIntChanged(this);
            }
        }
        private int maxV, minV;
        public int MaxValue { get {return maxV;}}
        public int MinValue {get {return minV;} }

        public OptionIntChangeHandler OptionIntChanged;

        public OptionInt(String name, XmlNode node)
            : base(name)
        {
            myValue = int.Parse(node.Attributes.GetNamedItem("value").Value);
            if (node.Attributes["max"] != null)
                maxV = int.Parse(node.Attributes["max"].Value);
            else maxV = int.MaxValue;
            if (node.Attributes["min"] != null)
                minV = int.Parse(node.Attributes["min"].Value);
            else minV = int.MinValue;
        }

        public OptionInt(String name, int value, int min, int max)
            : base(name)
        {
            this.Value = value;
            this.minV = min;
            this.maxV = max;
        }
    }
}
