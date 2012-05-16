using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class FunctionCallExpression : Expression
    {
        public delegate Option function(List<Option> args);
        function func;
        List<Expression> arguments;

        public static Option add(List<Option> args)
        {
            int sum = 0;
            int min = int.MaxValue, max = int.MinValue, size = 0;

            foreach(Option opt in args) {
                sum += ((OptionInt)opt).Value;
                size = ((OptionInt)opt).Size > size ? ((OptionInt)opt).Size : size;
                min = ((OptionInt)opt).MinValue < min ? ((OptionInt)opt).MinValue : min;
                max = ((OptionInt)opt).MaxValue > max ? ((OptionInt)opt).MaxValue : max;
            }
            return new OptionInt("", sum, min, max, size);
        }

        public static Option eq(List<Option> args)
        {
            Option fst = args[0];
            foreach(Option o in args.GetRange(1, args.Count - 1))
                if(!fst.Equals(o))
                    return new OptionBool("", false);
            return new OptionBool("", true);
        }

        public static Option unimplemented_function(List<Option> args)
        {
            throw new NotImplementedException();
        }

        Dictionary<String, function> functions = new Dictionary<string, function>() {
            {"+", add},
            {"crc", unimplemented_function},
            {"gt", unimplemented_function},
            {"==", eq}
        };

        public FunctionCallExpression(String expr)
        {
            expr = expr.Substring(1, expr.Length - 2);
            string[] split = splitArgs(expr);
            if (!functions.ContainsKey(split[0]))
                throw new ArgumentException("Unknown function " + split[0]);
            func = functions[split[0]];
            arguments = new List<Expression>();
            for (int i = 1; i < split.Length; i++)
                arguments.Add(Expression.fromString(split[i]));
        }

        public FunctionCallExpression(String functionName, List<Expression> args)
        {
            if (!functions.ContainsKey(functionName))
                throw new ArgumentException("Unknown function " + functionName);
            func = functions[functionName];
            arguments = args;
        }

        /// <summary>
        /// Rozdziela argumenty biorąc pod uwagę ()
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string[] splitArgs(string expr)
        {
            List<string> args = new List<string>();
            string next = "";
            int brackets = 0;
            foreach (char c in expr)
            {
                switch (c)
                {
                    case ' ':
                        if (brackets == 0)
                        {
                            args.Add(next);
                            next = "";
                        }
                        break;
                    case '(':
                        brackets++;
                        break;
                    case ')':
                        brackets--;
                        break;
                    default:
                        next += c;
                        break;
                }
            }
            if (next != "")
                args.Add(next);
            return args.ToArray();
        }

        override public Option eval(Dictionary<String, Option> env)
        {
            List<Option> args = new List<Option>();
            foreach (Expression e in arguments)
                args.Add(e.eval(env));
            return func(args);
        }
    }
}
