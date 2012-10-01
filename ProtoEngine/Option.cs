using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
        /// <summary>
        /// Nazwa opcji lub zmiennej.
        /// </summary>
        public String Name { get; set; }

        abstract public String TypeName { get; }

        /// <summary>
        /// Zwraca wartość tej opcji jako tablicę bajtów, lub null jeśli nie ma wartości.
        /// </summary>
        /// <returns></returns>
        abstract public byte[] toBytes();

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

        /// <summary>
        /// Ustawia wartość opcji na podstawie ciągu znaków.
        /// </summary>
        /// <param name="valueString"></param>
        abstract public void setValueFromString(string valueString);

        public abstract override bool Equals(object obj);

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
            XmlNode nameNode = node.Attributes.GetNamedItem("name");
            String name;
            if (nameNode == null)
                name = "";
            else
                name = nameNode.Value;

            String typeName = node.Attributes["type"].Value.Split(null)[0];
            Type type = optionClasses[typeName];
            ConstructorInfo constructor = type.GetConstructor(new Type[] {
                typeof(String), // name
                typeof(XmlNode) });
            
            return (Option)constructor.Invoke(new object[] { name, node });
        }

        public Option(String name) {
            this.Name = name;
        }
    }
}
