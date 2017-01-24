using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Registry
{
    class Request : Registry
    {
        private static Request instance = null;

        protected FTPClient.Request.Request request = new FTPClient.Request.Request();

        private Request() { }

        public void init()
        {
            
        }

        public static Request getInstance()
        {
            if (instance == null)
                instance = new Request();
            
            return instance;
        }

        public FTPClient.Request.Request get()
        {
            return request;
        }

        public void set(FTPClient.Request.Request request)
        {
            this.request = request;
            return;
        }
        
    }
}
