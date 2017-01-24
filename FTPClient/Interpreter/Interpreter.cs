using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Interpreter
{
    using FTPClient.Request;

    public class Interpreter
    {
        protected string command_prompt = "ftp> ";
        protected Request _request = new Request();

        public void Parse()
        {

            Registry.Connection conn = Registry.Connection.getInstance();
            if (conn.FtpServerName != "" && conn.FtpUserName != "") command_prompt =  conn.FtpUserName + "@" + conn.FtpServerName + ":" + conn.WorkingDirectory + ">";

            // Read input:
            string output = null;
            do
            {
                Console.Write(command_prompt);
                output = Console.ReadLine().Trim();
            }
            while (output == "");

            // Split command string by whitespace:
            string []exploded = output.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Set command name:
            _request.setProperty(Request.CMD, exploded[0]);

            // Extract arguments and options:
            int i = 0, k = 0;
            for(int j = 1; j < exploded.Count(); ++j)
            {
                string arg = exploded[j];
                // Option begins from character: '-'
                if (arg.ElementAt(0) == '-')
                {
                    _request.setProperty(Request.OPT + i, arg);
                    ++i;
                }
                // Otherwise this is argument:
                else
                {
                    _request.setProperty(Request.ARG + k, arg);
                    ++k;
                }
            }

            return;
        }

        public Request GetRequest()
        {
            return _request;
        }

        /* Set command prompt */
        public string CP
        {
            get { return command_prompt; }
            set { command_prompt = value; }
        }


    }
}
