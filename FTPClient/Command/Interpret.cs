using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTPClient.Request;
    using FTPClient.Interpreter;

    public class Interpret : Command
    {

        protected override Status _execute(Request request)
        {
            Interpreter interpreter = new Interpreter();
            interpreter.Parse();
            request = interpreter.GetRequest();
            // Set registry request:
            Registry.Request.getInstance().set(request);

            return Status.OK;
        }

    }
}
