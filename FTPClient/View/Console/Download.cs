using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Download : View
    {

        protected override State _run(Response response)
        {
            if (response.Assignments.Has("down") && (bool)response.Assignments["down"] && response.Assignments.Has("remote") && response.Assignments.Has("local"))
                System.Console.WriteLine("download: downloaded file '" + (response.Assignments["local"] == null ? response.Assignments["remote"] : response.Assignments["local"]) + "' from remote host '" + response.Assignments["remote"] + "'");
            
            printError(response);
            return State.END;
        }

    }
}
