using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Exit : View
    {

        protected override State _run(Response response)
        {
            printError(response);
            System.Console.WriteLine("Goodbye!!!");

            System.Environment.Exit(0);
            return State.END;
        }

    }
}
