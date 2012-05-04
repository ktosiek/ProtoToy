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
        private int mySize;
        public int MaxValue { get {return maxV;}}
        public int MinValue { get {return minV;} }
        public int Size { get { return mySize; } }

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
            if (node.Attributes["size"] != null)
                mySize = int.Parse(node.Attributes["size"].Value);
            else mySize = (int)Math.Ceiling(Math.Log(MaxValue - MinValue, 2));
        }

        public OptionInt(String name, int value, int min, int max, int size)
            : base(name)
        {
            this.Value = value;
            this.minV = min;
            this.maxV = max;
        }

        override public bool match(TransactionalStreamReader s)
        {
            int newValue = 0;
            s.startTransaction();
            for (int i = 0; i < Size; i++)
            {
                newValue <<= 8;
                newValue |= s.ReadByte();
            }

            if (MinValue > newValue || newValue > MaxValue)
            {
                s.cancelTransaction();
                return false;
            }

            Value = newValue;
            s.commitTransaction();
            return true;
        }

        public override Option copy()
        {
            return new OptionInt(Name, Value, MinValue, MaxValue, Size);
        }
    }
}
