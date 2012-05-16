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
            if (s == "true")
                value = new OptionBool("", true);
            else if (s == "false")
                value = new OptionBool("", false);
            else
                try
                {
                    value = new OptionInt("", int.Parse(s), int.MinValue, int.MaxValue, sizeof(int));
                }
                catch (FormatException)
                {
                    throw new ArgumentException();
                }
        }

        override public Option eval(Dictionary<String, Option> env)
        {
            return value;
        }
    }
}
