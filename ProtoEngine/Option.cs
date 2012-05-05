using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    /// <summary>
    /// Klasa odpowiadająca opcji lub zmiennej dowolnego typu.
    /// Dziedziczące klasy muszą udostępniać konstruktory:
    /// (string name) - tworzy pustą instancję o nazwie name
    /// (string name, XmlNode node) - tworszy nową instancję na podstawie DOM node
    /// </summary>
    public abstract class Option
    {
        private String name;
        /// <summary>
        /// Nazwa opcji lub zmiennej.
        /// </summary>
        public String Name { get { return name; } }

        /// <summary>
        /// Dopasuj fragment danych wejściowych do tego typu opcji
        /// </summary>
        /// <param name="s">strumień który będzie dopasowywany</param>
        /// <returns></returns>
        abstract public bool match(TransactionalStreamReader s);

        /// <summary>
        /// Zwraca kopię (COW lub pełną) tej opcji.
        /// </summary>
        /// <returns>referencja do stworzonej kopii</returns>
        abstract public Option copy();

        public static Dictionary<String, Type> optionClasses = new Dictionary<String, Type>() {
            {"bool", typeof(OptionBool)},
            {"byte", typeof(OptionInt)},
            {"word", typeof(OptionInt)},
            {"dword", typeof(OptionInt)},
            {"int", typeof(OptionInt)},
            {"array", typeof(OptionArray)}
        };

        public static Option fromXml(XmlNode node)
        {
            String name = node.Attributes.GetNamedItem("name").Value;
            return (Option)optionClasses[
                    node.Attributes.GetNamedItem("type").Value.Split(null)[0]
                    ].GetConstructor(new Type[] { typeof(String), typeof(XmlNode) })
                    .Invoke(new object[] { name, node });
        }

        public Option(String name) {
            this.name = name;
        }
    }
}
