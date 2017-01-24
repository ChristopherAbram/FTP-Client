using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Controller.Application
{
    using FTPClient.Request;
    using FTPClient.Command;

    public class Controller
    {
        protected Map _map              = null;
        protected Request _request      = null;
        protected string _cmd           = "";

        protected Type _base            = null;
        protected Command _default      = null;

        protected bool _invoked         = false;

        public Controller(Map map, Request request, string cmd = Request.CMD)
        {
            _map = map;
            _request = request;
            _cmd = cmd;

            _default = new Default();
            _base = _default.GetType().BaseType;
        }

        public string GetView()
        {
            string view = null;
            string cmd = _request.getProperty(_cmd);
            Command previous = _request.getLastCommand();
            Command.Status status = previous.status;
            
            if (cmd != null)
            {
                view = _map.getView(cmd, status);
                if (view == null)
                    view = _map.getView(cmd, Command.Status.DEFAULT);
                if (view == null)
                    view = _map.getView(Map.DEFAULT, status);
                if (view == null)
                    view = _map.getView(Map.DEFAULT, Command.Status.DEFAULT);
            }
            return view;
        }

        public string GetForward()
        {
            string forward = null;
            string cmd = _request.getProperty(_cmd);
            Command previous = _request.getLastCommand();
            Command.Status status = previous.status;

            if (cmd != null)
            {
                forward = _map.getForward(cmd, status);
                if (forward == null)
                    forward = _map.getForward(cmd, Command.Status.DEFAULT);
                if (forward == null)
                    forward = _map.getForward(Map.DEFAULT, status);
                if (forward == null)
                    forward = _map.getForward(Map.DEFAULT, Command.Status.DEFAULT);
            }

            if (forward != null)
                _request.setProperty(Request.CMD, forward);
            return forward;
        }

        public Command GetCommand()
        {
            string cmd = null;
            Command previous = _request.getLastCommand();
            if(previous == null)
            {
                cmd = _request.getProperty(_cmd);
                if(cmd == null)
                {
                    _request.setProperty(_cmd, Map.DEFAULT);
                    return _default;
                }
            }
            else
            {
                cmd = GetForward();
                if(cmd == null)
                    return null;
            }

            Command command = null;
            try
            {
                string classroot = _map.getClassRoot(cmd);
                Type classname = _resolveClassName(classroot);
                if (classname != null)
                    command = Activator.CreateInstance(classname) as Command;
            }
            catch (System.Exception ex) { }

            if (command == null)
                throw new Exception("Command: '" + cmd + "' does not exist...");

            return command;
        }

        protected Type _resolveClassName(string classroot)
        {
            Type classname = null;
            try {
                if (Type.GetType(_base.Namespace + "." + classroot) != null)
                {
                    Type command = Type.GetType(_base.Namespace + "." + classroot);
                    if (command.IsClass && command.IsSubclassOf(_base))
                    {
                        classname = command;
                    }
                }
            } catch(System.Exception ex)
            {
                return null;
            }
            return classname;
        }


    }
}
