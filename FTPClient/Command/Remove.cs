using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.Net;
    public class Remove : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                string type = null;
                if (request.hasProperty("opt0"))
                    type = request.getProperty("opt0");

                string file = null;
                if (request.hasProperty("arg0"))
                    file = request.getProperty("arg0");
                else
                {
                    error("Missing argument - file or directory name");
                    return Status.ERROR;
                }
                
                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }

                if(type == null || type == "-f")
                {
                    ftp.DeleteFile(file);
                    _response.Assign("rm_f", true);
                    _response.Assign("path", file);
                }
                else if (type == "-d")
                {
                    ftp.DeleteDirectory(file);
                    _response.Assign("rm_d", true);
                    _response.Assign("path", file);
                }
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
