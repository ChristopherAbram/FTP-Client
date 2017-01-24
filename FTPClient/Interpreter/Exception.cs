using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Interpreter
{
    public class Exception : System.Exception
    {
        public Exception() : base() { }
        public Exception(string message) : base(message) { }
        public Exception(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
