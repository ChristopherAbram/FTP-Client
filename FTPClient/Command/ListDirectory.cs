using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.Net;
    using System.Net.Sockets;
    public class ListDirectory : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                Registry.Connection conn = Registry.Connection.getInstance();
                Ftp ftp = conn.GetFtpConnection();

                /*string ipAddress = "";
                if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0)
                {
                    ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
                    //System.Console.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork));
                }*/

                bool extended = false;
                if (request.hasProperty("opt0"))
                    extended = request.getProperty("opt0") == "-l";

                string arg = conn.WorkingDirectory;
                if (request.hasProperty("arg0"))
                    arg = request.getProperty("arg0");

                if (ftp == null)
                {
                    error("Not connected with FTP server, use 'connect' command");
                    return Status.ERROR;
                }

                if (!extended)
                {
                    string[] filelist = ftp.GetFileList(arg);
                    _response.Assign("list", filelist);
                }
                else
                {
                    string[] filelist = ftp.GetExtendedFileList(arg);
                    _response.Assign("list", filelist);
                }
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
