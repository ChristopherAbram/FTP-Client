using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class MakeDirectory : View
    {

        protected override State _run(Response response)
        {

            if (response.Assignments.Has("made") && (bool)response.Assignments["made"] == true && response.Assignments.Has("dirname"))
                System.Console.WriteLine("mkdir: created directory '" + response.Assignments["dirname"] + "'");

            printError(response);
            return State.END;
        }

    }
}
