using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTPClient.Request;

    public class Start : Command
    {

        protected override Status _execute(Request request)
        {
            if (request.hasProperty("user")) Console.WriteLine(request.getProperty("user"));
            if (request.hasProperty("password")) Console.WriteLine(request.getProperty("password"));

            //Console.WriteLine(request);

            return Status.OK;
        }

    }
}
