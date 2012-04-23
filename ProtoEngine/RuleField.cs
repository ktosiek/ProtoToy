using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleField : Rule
    {
        String name;
        Option opt;
        Option defaultOpt;

        public RuleField(XmlNode node) { }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input)
        {
            try
            {
                input.startTransaction();
                
            }
            catch (Exception)
            {
                input.cancelTransaction();
                throw;
            }
        }
    }
}
