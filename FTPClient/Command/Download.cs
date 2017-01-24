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
    public class Download : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                string source = null, dest = null;
                if (request.hasProperty("arg0"))
                    source = request.getProperty("arg0");

                if (request.hasProperty("arg1"))
                    dest = request.getProperty("arg1");

                string rec = null;
                if (request.hasProperty("opt0"))
                    rec = request.getProperty("opt0");

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
                
                if(rec == null)
                    ftp.Download(source, Path.Combine(Registry.Application.getLocalDirectoryPath(), dest == null ? source : dest), true);
                else if(rec == "-R")
                    ftp.DownloadDirectory(source, Registry.Application.getLocalDirectoryPath(), true);
                

                _response.Assign("down", true);
                _response.Assign("remote", source);
                _response.Assign("local", dest);
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            return Status.OK;
        }

    }
}
