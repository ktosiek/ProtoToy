using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    class ExpressionException : Exception
    {
        public ExpressionException()
        {
        }

        public ExpressionException(string message): base(message)
        {
        }
    }
}
