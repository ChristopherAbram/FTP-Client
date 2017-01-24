using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Remove : View
    {

        protected override State _run(Response response)
        {
            if (response.Assignments.Has("rm_f") && (bool)response.Assignments["rm_f"] && response.Assignments.Has("path"))
                System.Console.WriteLine("rm: removed file '" + response.Assignments["path"] + "'");

            if (response.Assignments.Has("rm_d") && (bool)response.Assignments["rm_d"] && response.Assignments.Has("path"))
                System.Console.WriteLine("rm: removed file '" + response.Assignments["path"] + "'");

            printError(response);
            return State.END;
        }

    }
}
