using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTP;
    using System.Net;
    public class Connect : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                string server = string.Empty, username = string.Empty, port = string.Empty, password = string.Empty;
                bool p = false, r = false, s = false, q = false;

                // Getting server name:
                if (request.hasProperty("arg0"))
                {
                    server = request.getProperty("arg0");
                    request.setProperty("server", server);
                    p = true;
                }
                else if (request.hasProperty("server"))
                {
                    server = request.getProperty("server");
                    p = true;
                }
                // Getting username:
                if (request.hasProperty("arg1"))
                {
                    username = request.getProperty("arg1");
                    request.setProperty("username", username);
                    r = true;
                }
                else if (request.hasProperty("username"))
                {
                    username = request.getProperty("username");
                    r = true;
                }
                // Getting port number:
                if (request.hasProperty("arg2"))
                {
                    port = request.getProperty("arg2");
                    request.setProperty("port", port);
                    s = true;
                }
                else if (request.hasProperty("port"))
                {
                    port = request.getProperty("port");
                    s = true;
                }
                // Getting password:
                if (request.hasProperty("password"))
                {
                    password = request.getProperty("password");
                    q = true;
                }

                // All data:
                if(p && r && s && q)
                {
                    Ftp ftp = new Ftp();

                    // Set debug mode:
                    ftp.Debug = false;
                    if (request.hasProperty("opt0"))
                    {
                        ftp.Debug = request.getProperty("opt0") == "-d";
                    }
                    ftp.RemotePort = port == "" ? 21 : int.Parse(port);

                    IPAddress ip = Dns.Resolve(server).AddressList[0];
                    if (ip != null)
                        _response.Assign("ip_resolve", true);

                    _response.Assign("ip", ip.ToString());

                    // Connect and authenticate:
                    ftp.Connect(ip.ToString());
                    if (ftp.Connected) _response.Assign("connected", true);

                    ftp.Authenticate(username, password);
                    if (ftp.Authenticated) _response.Assign("logged", true);

                    string pwd = "/";
                    ftp.ChangeWorkingDirectory(pwd);

                    // Set registry connection:
                    Registry.Connection conn = Registry.Connection.getInstance();
                    conn.SetFtpConnection(ftp);
                    conn.FtpServerName = server;
                    conn.FtpUserName = username;
                    conn.WorkingDirectory = pwd;
                }
                
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
