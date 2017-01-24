using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Controller.Front
{
    abstract public class Controller
    {



        public static void run()
        {
            return;
        }

        abstract protected void _init();

        abstract protected void _handle();

    }
}
