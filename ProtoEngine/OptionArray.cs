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

        public OptionArray(String name, XmlNode node)
            : base(name)
        {
            string[] split = node.Attributes["type"].Value.Split();
            construct(split[1], int.Parse(split[2]));
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
                    s.cancelTransaction(); // TODO: A co z zmienionymi już wartościami opcji?
                    return false;
                }
            
            s.commitTransaction();
            return true;
        }
    }
}
