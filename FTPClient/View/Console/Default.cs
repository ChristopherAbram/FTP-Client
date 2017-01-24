using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Default : View
    {

        protected override State _run(Response response)
        {

            System.Console.WriteLine("Ftp Client to program do...\n(c) 2017 Krzysztof Abram.\nAll right reserved.\n\n");

            return State.END;
        }

    }
}
