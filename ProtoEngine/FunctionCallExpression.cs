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

        public static Option sub(List<Option> args)
        {
            int sum = 0;
            int min = int.MaxValue, max = int.MinValue, size = 0;

            foreach (Option opt in args)
            {
                if (opt == args[0])
                    sum = ((OptionInt)opt).Value;
                else
                    sum -= ((OptionInt)opt).Value;
                size = ((OptionInt)opt).Size > size ? ((OptionInt)opt).Size : size;
                min = ((OptionInt)opt).MinValue < min ? ((OptionInt)opt).MinValue : min;
                max = ((OptionInt)opt).MaxValue > max ? ((OptionInt)opt).MaxValue : max;
            }
            return new OptionInt("", sum, min, max, size);
        }

        public static Option mod(List<Option> args)
        {
            OptionInt a = (OptionInt)args[0];
            OptionInt b = (OptionInt)args[1];
            return new OptionInt("",
                a.Value % b.Value,
                a.MinValue < b.MinValue ? a.MinValue : b.MinValue,
                a.MaxValue > b.MaxValue ? a.MaxValue : b.MaxValue,
                a.Size > b.Size ? a.Size : b.Size);
        }

        public static Option div(List<Option> args)
        {
            OptionInt a = (OptionInt)args[0];
            OptionInt b = (OptionInt)args[1];
            return new OptionInt("",
                a.Value / b.Value,
                a.MinValue < b.MinValue ? a.MinValue : b.MinValue,
                a.MaxValue > b.MaxValue ? a.MaxValue : b.MaxValue,
                a.Size > b.Size ? a.Size : b.Size);
        }

        public static Option gt(List<Option> args)
        {
            OptionInt a = (OptionInt)args[0];
            OptionInt b = (OptionInt)args[1];
            return new OptionBool("", a.Value > b.Value);
        }

        public static Option eq(List<Option> args)
        {
            Option fst = args[0];
            foreach(Option o in args.GetRange(1, args.Count - 1))
                if(!fst.Equals(o))
                    return new OptionBool("", false);
            return new OptionBool("", true);
        }

        public static Option mul(List<Option> args)
        {
            throw new NotImplementedException("mul");
        }

        public static Option crc(List<Option> args)
        {
            // TODO: FIXME: crc :-)
            return new OptionInt("", 0xbeef, 0, 0xffff, 2);
        }

        // substr arr start end
        public static Option substr(List<Option> args)
        {
            OptionArray arr = (OptionArray)args[0];
            int start = ((OptionInt)args[1]).Value;
            int end = ((OptionInt)args[2]).Value;
            OptionArray ret = new OptionArray("", "array " + arr.TypeName.Split()[1] + " " + (end - start));
            for (int i = start; i < end; i++)
                ret.setOption(i - start, arr.getOption(i));
            return ret;
        }

        public static Option pack_bool(List<Option> args)
        {
            OptionArray arr = (OptionArray)args[0];
            List<Byte> data = new List<Byte>();
            byte b = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                if (((OptionBool)arr.getOption(i)).Value)
                    b |= (byte)(1 << (i % 8));
                if (i != 0 && i % 8 == 0) // Full byte done, next please
                {
                    data.Add(b);
                    b = 0;
                }
            }

            OptionArray ret = new OptionArray("", "array bool " + data.Count);
            for(int i = 0; i < data.Count; i ++)
                ret.setOption(i, new OptionInt("", data[i], 0, 255, 1));

            return ret;
        }

        Dictionary<String, function> functions = new Dictionary<string, function>() {
            {"+", add},
            {"%", mod},
            {"/", div},
            {"*", mul},
            {"-", sub},
            {"pack_bool", pack_bool},
            {"substr", substr},
            {"crc", crc},
            {"gt", gt},
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
                        else
                            next += c;
                        break;
                    case '(':
                        brackets++;
                        next += c;
                        break;
                    case ')':
                        brackets--;
                        next += c;
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
