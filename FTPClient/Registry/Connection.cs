using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Registry
{
    using FTP;

    class Connection : Registry
    {
        private static Connection instance = null;

        protected Ftp FtpConnection = null;

        public string FtpServerName = "";
        public string FtpUserName = "";
        public string WorkingDirectory = "/";

        private Connection() { }
        

        public static Connection getInstance()
        {
            if (instance == null)
                instance = new Connection();
            
            return instance;
        }

        public Ftp GetFtpConnection()
        {
            return FtpConnection;
        }

        public void SetFtpConnection(Ftp ftpConnection)
        {
            FtpConnection = ftpConnection;
        }
        
    }
}
