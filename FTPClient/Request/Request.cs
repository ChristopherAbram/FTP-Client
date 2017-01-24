using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Request
{
    public class Request
    {
        protected Dictionary<string, string> _parameters = new Dictionary<string, string>();

        protected Command.Command _command = null;

        public const string CMD = "command";
        public const string SUB = "subcommand";
        public const string ARG = "arg";
        public const string OPT = "opt";

        public Request(){ }

        public Request(Dictionary<string, string> parameters)
        {
            foreach(var param in parameters)
            {
                _set(param.Key, param.Value);
            }
        }

        public Request(string[] indexes, string[] values)
        {
            int i = 0;
            int indexes_count = indexes.Count();
            int values_count = values.Count();
            foreach(string index in indexes)
            {
                if (i < values_count)
                    _set(index, values[i]);
                else break;
                ++i;
            }
        }


        public string getProperty(string index)
        {
            return _get(index);
        }

        public void setProperty(string index, string value)
        {
            _set(index, value);
            return;
        }

        public bool hasProperty(string index)
        {
            return _parameters.ContainsKey(index);
        }

        public bool unsetProperty(string index)
        {
            return _parameters.Remove(index);
        }

        public void setCommand(Command.Command command)
        {
            _command = command;
            return;
        }

        public Command.Command getLastCommand()
        {
            return _command;
        }

        public override string ToString()
        {
            string str = "";
            foreach (var r in _parameters)
                str += r.Key + " => " + r.Value + "\n";
            return str;
        }

        protected void _set(string index, string value)
        {
            try
            {
                if (index != null && index != "" && _parameters.ContainsKey(index))
                    _parameters[index] = value;
                else
                    _parameters.Add(index, value);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to set '" + index + "' request index", ex);
            }
            return;
        }

        protected string _get(string index)
        {
            try
            {
                if (_parameters.ContainsKey(index))
                    return _parameters[index];
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to get '" + index + "' request value", ex);
            }
            return null;
        }



    }
}
