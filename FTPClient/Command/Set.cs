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
    public class Set : Command
    {
        
        protected override Status _execute(Request.Request request)
        {
            try {
                
                // Set local directory path (local repository):
                if(request.hasProperty("arg0") && request.getProperty("arg0") == "location")
                {
                    if (!request.hasProperty("arg1"))
                    {
                        error("Missing path argument");
                        return Status.ERROR;
                    }

                    string path = request.getProperty("arg1");
                    if (Directory.Exists(path))
                    {
                        Registry.Application.setLocalDirectoryPath(path);
                        _response.Assign("chdir", true);
                        _response.Assign("path", path);
                    }
                    else
                    {
                        error("Given directory does not exists");
                        return Status.ERROR;
                    }
                }
                // Missing any set arguments:
                else
                {
                    error("Missing set argument");
                    return Status.ERROR;
                }
            } catch(System.Exception ex)
            {
                handle(ex);
            }
            
            return Status.OK;
        }

    }
}
