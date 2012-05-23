using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ProtoEngine
{
    class RuleMatch : Rule
    {
        Expression myExpr;

        public RuleMatch(XmlNode node, Protocol proto)
        {
            if (node.Attributes["name"] != null)
            {
                // <match name=... value=... />
                Expression var = new VariableExpression("$" + node.Attributes["name"].Value);
                Expression value = Expression.fromString(node.Attributes["value"].Value);
                myExpr = new FunctionCallExpression("==", new List<Expression>(new Expression[] { var, value }));
            }
            else
            {
                // <match_expr>...</match_expr>
                myExpr = Expression.fromString(node.InnerText);
            }
        }

        private Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<Option> fields)
        {
            if (myExpr.eval(variables).Equals(new OptionBool("", true)))
            {
                fields = null;
                return variables;
            }
            else
            {
                fields = null;
                return null;
            }
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            TransactionalStreamReader input,
            out List<Option> fields)
        {
            return match(variables, out fields);
        }

        override public Dictionary<String, Option> match(Dictionary<String, Option> variables,
            out List<byte[]> output,
            out List<Option> fields)
        {
            output = null;
            return match(variables, out fields);
        }
    }
}
