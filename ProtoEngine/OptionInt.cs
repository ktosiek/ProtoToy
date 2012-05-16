using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    public class OptionInt : Option
    {
        public delegate void OptionIntChangeHandler(OptionInt option);

        public OptionIntChangeHandler optionIntChanged;
        private int? myValue;
        public int Value
        {
            get { return myValue == null ? Int32.MinValue : (int)myValue; }
            set {
                if (value > maxV || value < minV)
                    throw new ArgumentOutOfRangeException();
                myValue = value;
                if(optionIntChanged != null) optionIntChanged(this);
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
            if(node.Attributes["value"] != null)
                myValue = int.Parse(node.Attributes["value"].Value);

            if (node.Attributes["type"] != null) {
                mySize = (new Dictionary<String, int>() {
                    {"byte", 1},
                    {"word", 2},
                    {"dword", 4},
                    {"int", 4}
                })[node.Attributes["type"].Value];
            }

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
            this.minV = min;
            this.maxV = max;
            this.mySize = size;
            this.Value = value;
        }

        override public byte[] toBytes()
        {
            if (myValue == null)
                return null;
            byte[] bytes = new byte[Size];
            int tmp = Value;
            for (int i = 0; i < Size; i++)
            {
                bytes[Size - 1 - i] = (byte)tmp;
                tmp /= 256;
            }
            return bytes;
        }

        override public bool match(TransactionalStreamReader s)
        {
            // First, read whatever is in the stream
            int newValue = 0;
            s.startTransaction();
            for (int i = 0; i < Size; i++)
            {
                newValue |= s.ReadByte() << (8*i);
            }

            if (MinValue > newValue || newValue > MaxValue)
            {
                s.cancelTransaction();
                return false;
            }

            if (myValue == null)
            {
                Value = newValue;
                s.commitTransaction();
                return true;
            }
            else // myValue != null
                if (myValue == newValue)
                {
                    s.commitTransaction();
                    return true;
                }
                else
                {
                    s.cancelTransaction();
                    return false;
                }
        }

        public override Option copy()
        {
            return new OptionInt(Name, Value, MinValue, MaxValue, Size);
        }

        public override bool Equals(object obj)
        {
            if (this.GetType().IsInstanceOfType(obj))
                return ((OptionInt)obj).Value == this.Value;
            return false;
        }
    }
}
