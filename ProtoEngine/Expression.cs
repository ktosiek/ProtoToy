using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    /// <summary>
    /// Odpowiada za parsowanie i ewaluację wyrażeń.
    /// </summary>
    public abstract class Expression
    {
        public static Expression fromString(String expr)
        {
            switch (expr[0])
            {
                case '(':
                    return new FunctionCallExpression(expr);
                case '$':
                    return new VariableExpression(expr);
                default:
                    return new ConstantExpression(expr);
            }
        }

        /// <summary>
        /// Redukuje wyrażenie do jednej wartości.
        /// </summary>
        /// <param name="env">środowisko, nie będzie zmieniane</param>
        /// <returns>wartość wyrażenia w danym środowisku</returns>
        abstract public Option eval(Dictionary<String, Option> env);
    }
}
