using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace ProtoEngine
{
    public abstract class Rule
    {

        private static Dictionary<String, Type> ruleClasses = new Dictionary<String, Type>() {
            {"msg", typeof(RuleBlock)},
            {"msg_start_mark", typeof(RuleBlock)},
            {"block", typeof(RuleBlock)},
            {"conditional", typeof(RuleBlock)},
            {"responds_when", typeof(RuleBlock)},
            {"field", typeof(RuleField)},
           // {"inc_msg", typeof(RuleIncMsg)},
            {"match", typeof(RuleMatch)},
            {"match_expr", typeof(RuleMatch)},
            {"set", typeof(RuleSet)},
            {"isset", typeof(RuleIsSet)},
            {"isnset", typeof(RuleIsSet)},
            {"assert", typeof(RuleMatch)} //,
            //{"warning", typeof(RuleWarning)}
        };

        public static Rule fromXml(XmlNode node, Protocol protocol)
        {
            if (!ruleClasses.ContainsKey(node.Name))
                throw new ArgumentException("Unknown rule " + node.Name);
            return (Rule)ruleClasses[
                     node.Name
                     ].GetConstructor(new Type[] { typeof(XmlNode), typeof(Protocol) })
                     .Invoke(new object[] { node, protocol });
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
        /// <param name="fields">dopasowane pola wiadomości</param>
        /// <returns>stan środowiska po dopasowaniu lub null jeśli się ono nie udało</returns>
        abstract public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input, out List<Option> fields);

        /// <summary>
        /// Spróbuj skonstruować odpowiedź na podstawie środowiska.
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        abstract public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output, out List<Option> fields);
    }
}
