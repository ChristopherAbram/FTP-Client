using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class ChangeDirectory : View
    {

        protected override State _run(Response response)
        {
            printError(response);
            return State.END;
        }

    }
}
