using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Upload : View
    {

        protected override State _run(Response response)
        {
            if (response.Assignments.Has("up") && (bool)response.Assignments["up"] && response.Assignments.Has("file"))
                System.Console.WriteLine("upload: uploaded file '" + response.Assignments["file"] + "'");
            
            printError(response);
            return State.END;
        }

    }
}
