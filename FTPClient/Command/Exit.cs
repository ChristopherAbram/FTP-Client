using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Command
{
    using FTPClient.Request;

    public class Exit : Command
    {

        protected override Status _execute(Request request)
        {
            try {
                Registry.Connection conn = Registry.Connection.getInstance();
                if (conn.GetFtpConnection() != null)
                {
                    FTP.Ftp ftpClient = conn.GetFtpConnection();
                    if (ftpClient.Connected)
                    {
                        ftpClient.Close();
                    }
                }
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            return Status.OK;
        }

    }
}
