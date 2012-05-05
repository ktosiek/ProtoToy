using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    class VariableExpression : Expression
    {
        string variable;
        public VariableExpression(String expr)
        {
            variable = expr.Substring(1);
        }

        override public Option eval(Dictionary<String, Option> env)
        {
            return env[variable];
        }
    }
}
