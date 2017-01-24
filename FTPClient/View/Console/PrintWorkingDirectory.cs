using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class PrintWorkingDirectory : View
    {

        protected override State _run(Response response)
        {
            System.Console.WriteLine(response.Assignments.Has("working_directory") ? "Working directory is \"" + response.Assignments["working_directory"] + "\"" : "Unable to display working directory.");
            printError(response);
            return State.END;
        }

    }
}
