using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class VariableExpression : Expression
    {
        string variable;
        public VariableExpression(String expr)
        {
            variable = expr.Substring(1);
        }

        override public Option eval(Dictionary<String, Option> env)
        {
            if(env.ContainsKey(variable)) {
                return env[variable];
            } else {
                throw new ExpressionException("No variable named " + variable);
            }
        }
    }
}
