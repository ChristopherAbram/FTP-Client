using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.Net;
    public class Move : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                string source = null, dest = null;

                if (request.hasProperty("arg0"))
                    source = request.getProperty("arg0");
                else
                {
                    error("Missing source path argument");
                    return Status.ERROR;
                }

                if (request.hasProperty("arg1"))
                    dest = request.getProperty("arg1");
                else
                {
                    error("Missing destination path argument");
                    return Status.ERROR;
                }

                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }

                ftp.RenameFile(source, dest);
                _response.Assign("moved", true);
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
