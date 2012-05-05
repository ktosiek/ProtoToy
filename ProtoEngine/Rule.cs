using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace ProtoEngine
{
    abstract class Rule
    {

        private static Dictionary<String, Type> ruleClasses = new Dictionary<String, Type>() {
            {"msg", typeof(RuleBlock)},
            {"block", typeof(RuleBlock)},
            {"conditional", typeof(RuleBlock)},
            {"field", typeof(RuleField)}
        };

        public static Rule fromXml(XmlNode node)
        {
            return (Rule)ruleClasses[
                    node.Name
                    ].GetConstructor(new Type[] { typeof(XmlNode) })
                    .Invoke(new object[] { node });
        }

        public static Rule empty()
        {
            return new EmptyRule();
        }

        /// <summary>
        /// Spróbuj dopasować te regułę do danych wejściowych.
        /// </summary>
        /// <param name="variables">stan środowiska przed dopasowywaniem</param>
        /// <param name="input">strumień wejściowy</param>
        /// <returns>stan środowiska po dopasowaniu lub null jeśli się ono nie udało</returns>
        abstract public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input);
    }
}
