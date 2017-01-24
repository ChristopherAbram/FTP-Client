using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Set : View
    {

        protected override State _run(Response response)
        {
            if (response.Assignments.Has("chdir") && (bool)response.Assignments["chdir"] && response.Assignments.Has("path"))
                System.Console.WriteLine("Changed local working directory to \"" + response.Assignments["path"] + "\"");

            printError(response);
            return State.END;
        }

    }
}
