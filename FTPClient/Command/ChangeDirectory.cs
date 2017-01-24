using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.Net;
    public class ChangeDirectory : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                string path = conn.WorkingDirectory;
                if (request.hasProperty("arg0"))
                    path = request.getProperty("arg0");

                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }

                ftp.ChangeWorkingDirectory(path);

                string wd = ftp.PrintWorkingDirectory();
                conn.WorkingDirectory = wd.Substring(wd.IndexOf('"') + 1, wd.LastIndexOf('"') - wd.IndexOf('"') - 1);
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
