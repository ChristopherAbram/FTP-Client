using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTPClient.Response;

namespace FTPClient.View.Console
{
    using Response;
    public class Default : View
    {

        protected override State _run(Response response)
        {

            System.Console.WriteLine(" FtpClient CLI is a program to work with FTP server. The program uses passive FTP mode.\n" +
                " Short help (list of commands):\n\n" +
                " pwd                - prints the current working directory\n" +
                " cd dirname         - changes working directory to dirname, which is relative path\n" +
                " ls [-l] [filepath] - prints list of working directory, using option -l shows extended list\n" +
                "                    - filepath is a path to file (simple wild cards supported, e.g.: *.php)\n" + 
                " connect [-d] [hostname [user [port]]] - establishes connection with given host, password required.\n" +
                "                    - Option -d lunch program in debug mode - FTP commands visible\n" +
                " help               - prints this short help\n" +
                " mv source dest     - moves source file to the destination (must be full file name)\n" +
                " rm [-f] [-d] path  - removes given path. Option -f tells that path is file, -d - directory\n" +
                " set location localpath - sets local repository, localpath e.g. G:\\Programs\\Server\n" +
                " mkdir dirname      - makes directory 'dirname' in working directory\n" +
                " exit               - quits server session and ends program\n" +
                " upload filename    - uploads file from local repository to working directory. Allowed aliases: up, put\n" +
                " download [-R] source [dest] - downloads source from server to local repository, to dest - full file name.\n" +
                "                    - Option -R enables downloading whole directory tree. Allowed aliases: down, get\n\n" +
                " (c) 2017 Krzysztof Abram.\n All right reserved.\n");

            return State.END;
        }

    }
}
