using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    abstract public class Command
    {
        public enum Status
        {
            DEFAULT, OK, ERROR, NEXT
        }

        protected Status _status = Status.DEFAULT;
        protected Response.Response _response = new Response.Response();
        protected Request.Request _request = new Request.Request();

        public virtual Status execute(Request.Request request)
        {
            _request = request;
            _status = _execute(request);

            _response.Status = _status;
            _request.setCommand(this);

            _response.Request = _request;
            return _status;
        }

        abstract protected Status _execute(Request.Request request);

        public Status status {
            get { return _status; }
            protected set { _status = value; }
        }

        public Response.Response response
        {
            get { return _response; }
            protected set { _response = value; }
        }

        protected void error(string message)
        {
            _response.Error(message);
        }

        protected void warning(string message)
        {
            _response.Warning(message);
        }

        protected void correct(string message)
        {
            _response.Correct(message);
        }

        protected void handle(System.Exception ex)
        {
            do
            {
                _response.Error(ex.Message);
            } while ((ex = ex.InnerException) != null);
        }

    }
}
