using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    public class Default : Command
    {

        protected override Status _execute(Request.Request request)
        {

            

            return Status.OK;
        }

    }
}
