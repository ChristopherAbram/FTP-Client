using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Controller.Application
{
    public class Map
    {

        public const string DEFAULT = "default";

        // View map:
        protected Dictionary<string, Dictionary<Command.Command.Status, string>> _view = new Dictionary<string, Dictionary<Command.Command.Status, string>>();

        // Forward map:
        protected Dictionary<string, Dictionary<Command.Command.Status, string>> _forward = new Dictionary<string, Dictionary<Command.Command.Status, string>>();

        // Classname map:
        protected Dictionary<string, string> _classroot = new Dictionary<string, string>();

        public void addClassRoot(string command, string classroot)
        {
            try
            {
                if (_classroot.ContainsKey(command))
                    _classroot[command] = classroot;
                else
                    _classroot.Add(command, classroot);
            } catch(System.Exception ex)
            {
                throw new Exception("Unable to add new class name to controller map", ex);
            }
        }

        public string getClassRoot(string command)
        {
            try
            {
                if (_classroot.ContainsKey(command))
                    return _classroot[command];
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to get class name for command '" + command + "'", ex);
            }
            return null;
        }

        public void addForward(string command, Command.Command.Status status, string forward)
        {
            try
            {
                if (_forward.ContainsKey(command))
                {
                    if (_forward[command].ContainsKey(status))
                        _forward[command][status] = forward;
                    else
                        _forward[command].Add(status, forward);
                }
                else {
                    Dictionary<Command.Command.Status, string> dict = new Dictionary<Command.Command.Status, string>();
                    dict.Add(status, forward);
                    _forward.Add(command, dict);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to add new forward command to controller map", ex);
            }
        }

        public string getForward(string command = DEFAULT, Command.Command.Status status = Command.Command.Status.DEFAULT)
        {
            try
            {
                if (_forward.ContainsKey(command))
                    if (_forward[command].ContainsKey(status))
                        return _forward[command][status];
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to get forward command for {" + command + ", " + status + "}", ex);
            }
            return null;
        }

        public void addView(string command, Command.Command.Status status, string view)
        {
            try
            {
                if (_view.ContainsKey(command))
                {
                    if (_view[command].ContainsKey(status))
                        _view[command][status] = view;
                    else
                        _view[command].Add(status, view);
                }
                else {
                    Dictionary<Command.Command.Status, string> dict = new Dictionary<Command.Command.Status, string>();
                    dict.Add(status, view);
                    _view.Add(command, dict);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to add new view name to controller map", ex);
            }
        }

        public string getView(string command = DEFAULT, Command.Command.Status status = Command.Command.Status.DEFAULT)
        {
            try
            {
                if (_view.ContainsKey(command))
                    if (_view[command].ContainsKey(status))
                        return _view[command][status];
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to get view name for {" + command + ", " + status + "}", ex);
            }
            return null;
        }

    }
}
