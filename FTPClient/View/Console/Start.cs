using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Start : View
    {

        protected override State _run(Response response)
        {
            if (!response.Request.hasProperty("user") || !response.Request.hasProperty("password"))
            {
                System.Console.Write("user: ");
                response.Request.setProperty("user", System.Console.ReadLine());

                System.Console.Write("password: ");
                response.Request.setProperty("password", System.Console.ReadLine());

                return State.REPEAT;
            }
            else
            {
                System.Console.WriteLine(":)");
                System.Console.WriteLine(response.Request);
            }

            return State.END;
        }

    }
}
