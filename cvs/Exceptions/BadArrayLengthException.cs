using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvs.Exceptions
{
    internal class BadArrayLengthException : Exception
    {
        public BadArrayLengthException() : base()
        {
        }

        public BadArrayLengthException(string message) : base(message)
        {
        }
    }
}
