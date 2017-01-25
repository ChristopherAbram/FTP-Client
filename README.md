<pre>FtpClient CLI is a program to work with FTP server. The program uses passive FTP mode. No SSL support.

Short help (list of commands):
 pwd                - prints the current working directory
 cd dirname         - changes working directory to dirname, which is relative path
 ls [-l] [filepath] - prints list of working directory, using option -l shows extended list
                    - filepath is a path to file (simple wild cards supported, e.g.: *.php)
 connect [-d] [hostname [user [port]]] - establishes connection with given host, password required.
                    - Option -d lunch program in debug mode - FTP commands visible
 help               - prints this short help
 mv source dest     - moves source file to the destination (must be full file name)
 rm [-f] [-d] path  - removes given path. Option -f tells that path is file, -d - directory
 set location localpath - sets local repository, localpath e.g. G:\Programs\Server\
 mkdir dirname      - makes directory 'dirname' in working directory
 exit               - quits server session and ends program
 upload filename    - uploads file from local repository to working directory. Allowed aliases: up, put
 download [-R] source [dest] - downloads source from server to local repository, to dest - full file name.
                    - Option -R enables downloading whole directory tree. Allowed aliases: down, get
                    
 (c) 2017 Krzysztof Abram. 
 All right reserved.
</pre>
