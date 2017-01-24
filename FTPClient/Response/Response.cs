using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Response
{
    public class Response
    {
        // Assignments:
        protected Assignment _assignments = new Assignment();

        // Messages:
        protected Stack<string> _error = new Stack<string>();
        protected Stack<string> _warning = new Stack<string>();
        protected Stack<string> _correct = new Stack<string>();

        // Status code:
        protected Command.Command.Status _status = Command.Command.Status.DEFAULT;

        // Request object:
        protected Request.Request _request = null;

        // Name of view class:
        protected string _view = null;

        public Response()
        {
            _request = Registry.Request.getInstance().get();
        }

        public Response(Request.Request request)
        {
            _request = request;
        }

        public void Assign(string index, object value)
        {
            _assignments.Assign(index, value);
        }

        public void AssignAll(Assignment assignments)
        {
            _assignments.AssignAll(assignments);
        }

        public Assignment Assignments
        {
            get { return _assignments; }
            set { _assignments = value; }
        }

        public Command.Command.Status Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Request.Request Request
        {
            get { return _request; }
            set { _request = value; }
        }

        public string View
        {
            get { return _view; }
            set { _view = value; }
        }

        public string Error()
        {
            try
            {
                return _error.Pop();
            }
            catch (System.Exception ex)
            {
                //throw new Exception("Unable to get error message", ex);
            }
            return null;
        }

        public void Error(string message)
        {
            try
            {
                _error.Push(message);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to add new error message", ex);
            }
        }

        public string Warning()
        {
            try
            {
                return _warning.Pop();
            }
            catch (System.Exception ex)
            {
                //throw new Exception("Unable to get warning message", ex);
            }
            return null;
        }

        public void Warning(string message)
        {
            try
            {
                _warning.Push(message);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to add new warning message", ex);
            }
        }

        public string Correct()
        {
            try
            {
                return _correct.Pop();
            }
            catch (System.Exception ex)
            {
                //throw new Exception("Unable to get info message", ex);
            }
            return null;
        }

        public void Correct(string message)
        {
            try
            {
                _correct.Push(message);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to add new info message", ex);
            }
        }

        public void Join(Response response)
        {
            _assignments.Merge(response._assignments);
            foreach (var error in response._error) _error.Push(error);
            foreach (var warning in response._warning) _error.Push(warning);
            foreach (var correct in response._correct) _error.Push(correct);
            return;
        }
    }
}
