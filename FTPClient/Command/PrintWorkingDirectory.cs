using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.IO;
    using System.Net;
    public class PrintWorkingDirectory : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }
                
                string wd = ftp.PrintWorkingDirectory();

                _response.Assign("working_directory", wd.Substring(wd.IndexOf('"') + 1, wd.LastIndexOf('"') - wd.IndexOf('"') - 1));
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
