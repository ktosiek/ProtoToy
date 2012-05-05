using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class ConstantExpression : Expression
    {
        Option value;

        public ConstantExpression(String s)
        {
            value = new OptionInt("", int.Parse(s), int.MinValue, int.MaxValue, sizeof(int));
            // TODO: inne typy
        }

        override public Option eval(Dictionary<String, Option> env)
        {
            return value;
        }
    }
}
