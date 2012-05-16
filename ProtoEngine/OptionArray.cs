using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    /// <summary>
    /// Odpowiada tablicy opcji o jednym typie.
    /// </summary>
    class OptionArray : Option
    {
        private Type type;
        private Option[] optionsArray;
        private Option[] OptionsArray { get {
            if (optionsArray == null) { return optionsArray = (Option[])parent.OptionsArray.Clone(); }
            else { return optionsArray; }
        }
        }

        private OptionArray parent;

        public int Count { get { return OptionsArray.Length; } }

        public Option getOption(int n)
        {
            if (OptionsArray[n] == null)
            {
                return optionsArray[n] = parent.getOption(n).copy();
            }
            else
            {
                return optionsArray[n];
            }
        }

        public void setOption(int n, Option opt)
        {
            OptionsArray[n] = opt;
        }

        public OptionArray(String name, XmlNode node)
            : base(name)
        {
            string[] split = node.Attributes["type"].Value.Split();
            construct(split[1], int.Parse(split[2]));
        }

        private OptionArray(String name, OptionArray parent)
            : base(name)
        {
            this.parent = parent;
        }

        private void construct(String type, int size)
        {
            this.type = Option.optionClasses[type];
            optionsArray = new Option[size];
            for (int n = 0; n < size; n++)
            {
                optionsArray[n] = (Option)Activator.CreateInstance(
                   this.type,
                    new object[] { "____" + this.Name + "[" + n + "]" }
                );
            }
        }

        override public byte[] toBytes()
        {
            List<byte[]> bytesList = new List<byte[]>();
            int len = 0;
            foreach (Option o in OptionsArray)
            {
                byte[] bytes = o.toBytes();
                if (bytes == null)
                    return null;
                bytesList.Add(bytes);
                len += bytes.Length;
            }
            byte[] ret = new byte[len];
            int i = 0;
            foreach (byte[] b in bytesList)
            {
                b.CopyTo(ret, i);
                i += b.Length;
            }
            return ret;
        }

        override public bool match(TransactionalStreamReader s)
        {
            // FIXME: wczytywanie vs. dopasowanie tablic
            s.startTransaction();
            foreach (Option o in OptionsArray)
                if (!o.match(s))
                {
                    s.cancelTransaction();
                    return false;
                }
            
            s.commitTransaction();
            return true;
        }

        public override Option copy()
        {
            return new OptionArray(Name, this);
        }

        public override bool Equals(object obj)
        {
            if (this.GetType().IsInstanceOfType(obj))
            {
                OptionArray other = (OptionArray)obj;
                for (int i = 0; i < Count; i++)
                {
                    if (!this.getOption(i).Equals(other.getOption(i)))
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
