using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.Net;
    public class MakeDirectory : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                string path = null;
                if (request.hasProperty("arg0"))
                    path = request.getProperty("arg0");

                if(path == null)
                {
                    error("Missing directory name");
                    return Status.ERROR;
                }

                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }

                ftp.MakeDirectory(path);
                _response.Assign("dirname", path);
                _response.Assign("made", true);
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
