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

        override public bool match(TransactionalStreamReader s)
        {
            s.startTransaction();
            foreach (Option o in optionsArray)
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
    }
}
