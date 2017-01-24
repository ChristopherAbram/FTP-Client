using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.View
{
    using Response;
    public abstract class View
    {
        public enum State
        {
            REPEAT, END
        }

        public State run(Response response)
        {
            State state = _run(response);
            Registry.Request.getInstance().set(response.Request);
            return state;
        }

        abstract protected State _run(Response response);

        protected void printError(Response response)
        {
            string msg = null;
            while ((msg = response.Error()) != null)
                System.Console.WriteLine("Error: " + msg);
        }

    }
}
