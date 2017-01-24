using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Move : View
    {

        protected override State _run(Response response)
        {

            if (response.Assignments.Has("moved") && (bool)response.Assignments["moved"] == true)
                System.Console.WriteLine("Properly moved source");

            printError(response);
            return State.END;
        }

    }
}
