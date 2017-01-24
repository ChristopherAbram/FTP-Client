using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class ListDirectory : View
    {

        protected override State _run(Response response)
        {

            if (response.Assignments.Has("list"))
            {
                string[] list = response.Assignments["list"] as string[];
                if (list != null)
                {
                    foreach (string file in list)
                    {
                        System.Console.WriteLine(" " + file);
                    }
                }
            }
            printError(response);
            return State.END;
        }

    }
}
