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
    public class Upload : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                string source = null, dest = null;
                if (request.hasProperty("arg0"))
                    source = request.getProperty("arg0");

                //if (request.hasProperty("arg1"))
                //    dest = request.getProperty("arg1");

                if(source == null)
                {
                    error("Missing source - filename argument");
                    return Status.ERROR;
                }
                
                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }

                ftp.Upload(Path.Combine(Registry.Application.getLocalDirectoryPath(), source), true);
                _response.Assign("up", true);
                _response.Assign("file", source);
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
