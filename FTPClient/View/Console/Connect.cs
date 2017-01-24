using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Connect : View
    {

        protected override State _run(Response response)
        {
            State state = State.END;
            // Getting server name:
            if (!response.Request.hasProperty("server"))
            {
                System.Console.Write("host: ");
                response.Request.setProperty("server", System.Console.ReadLine());
                state = State.REPEAT;
            }
            // Getting username:
            if (!response.Request.hasProperty("username"))
            {
                System.Console.Write("user: ");
                response.Request.setProperty("username", System.Console.ReadLine());
                state = State.REPEAT;
            }
            // Getting port:
            if (!response.Request.hasProperty("port"))
            {
                System.Console.Write("port: ");
                response.Request.setProperty("port", System.Console.ReadLine());
                state = State.REPEAT;
            }
            // Getting password:
            if (!response.Request.hasProperty("password"))
            {
                System.Console.Write("password: ");
                response.Request.setProperty("password", System.Console.ReadLine());
                state = State.REPEAT;
            }

            // 
            if(state != State.REPEAT)
            {
                if (response.Assignments.Has("ip_resolve")) System.Console.WriteLine("Status: IP resolving for " + response.Request.getProperty("server"));
                if (response.Assignments.Has("connected") && response.Assignments.Has("ip")) System.Console.WriteLine("Status: Connected with " + response.Assignments["ip"]);
                if (response.Assignments.Has("logged") && response.Assignments.Has("ip")) System.Console.WriteLine("Status: Authenticated user: " + response.Request.getProperty("username"));
            }

            printError(response);

            return state;
        }

    }
}
