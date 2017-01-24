using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.FTP
{
    public class Ftp : IFtp
    {
        private string remoteHost, remotePath, remoteUser, remotePass, mes;
        private string reply;

        private int remotePort, bytes;
        private int retValue;

        private Boolean debug;
        private Boolean logged;
        private bool useStream;
        private bool isUpload;

        private Stream stream = null;
        //private Stream stream2 = null;

        private Socket clientSocket;
        private Socket dataSocket;

        // Block size - 1KB:
        private static int BLOCK_SIZE = 1024;

        Byte[] buffer = new Byte[BLOCK_SIZE];
        Encoding UTF8 = Encoding.UTF8;

        public Ftp()
        {
            remoteHost = string.Empty;
            remotePath = "/";
            remoteUser = string.Empty;
            remotePass = string.Empty;
            remotePort = 21;
            debug = false;
            logged = false;
        }

        /// <summary>
        /// Sets or gets the name of the FTP server to connect to.
        /// </summary>
        public string RemoteHost 
        {
            get { return remoteHost; }
            set { remoteHost = value; }
        }

        /// <summary>
        /// Sets or gets the flag enabling using stream
        /// </summary>
        public bool UseStream
        {
            get { return useStream; }
            set { useStream = value; }
        }

        /// <summary>
        /// Sets or gets the port number to use for FTP.
        /// </summary>
        public int RemotePort
        {
            get { return remotePort; }
            set { remotePort = value; }
        }

        /// <summary>
        /// Sets or gets the remote directory path.
        /// </summary>
        public string RemotePath
        {
            get { return remotePath; }
            set { remotePath = value; }
        }

        /// <summary>
        /// Sets or gets the user name to use for logging into the remote server.
        /// </summary>
        public string RemoteUser
        {
            get { return remoteUser; }
            set { remoteUser = value; }
        }

        /// <summary>
        /// Sets or gets the password to user for logging into the remote server.
        /// </summary>
        public string RemotePass
        {
            get { return remotePass; }
            set { remotePass = value; }
        }

        /// <summary>
        /// Set debug mode.
        /// </summary>
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        /// <summary>
        /// Gets a value that indicates whether Socket is connected to remote host
        /// </summary>
        public bool Connected
        {
            get { return (clientSocket != null && clientSocket.Connected); }
        }

        /// <summary>
        /// Gets a value that indicates whether user has been authenticated
        /// </summary>
        public bool Authenticated
        {
            get { return logged; }
        }

        /// <summary>
        /// Set connection to the remote server.
        /// </summary>
        public void Connect()
        {
            IPEndPoint ep = null;
            try {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            } catch(Exception ex)
            {
                throw new IOException("Couldn't create socket", ex);
            }
            try {
                ep = new IPEndPoint(Dns.Resolve(remoteHost).AddressList[0], remotePort);
            } catch(System.Exception ex)
            {
                throw new System.Exception("Unable to resolve dns name", ex);
            }

            try
            {
                clientSocket.Connect(ep);
            }
            catch (Exception ex)
            {
                throw new IOException("Couldn't connect to remote server", ex);
            }

            readReply();
            if (retValue != 220)
            {
                Close();
                throw new IOException(reply.Substring(4));
            }

            //dataSocket = createDataSocket();

        }

        /// <summary>
        /// Set connection to the remote server which name is hostname.
        /// </summary>
        /// <param name="hostname">The name of FTP host</param>
        public void Connect(string hostname)
        {
            remoteHost = hostname;
            Connect();
        }

        /// <summary>
        /// Perform authentication with FTP server
        /// </summary>
        public void Authenticate()
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                    Connect();
            }
            catch (Exception)
            {
                throw new IOException("Couldn't connect to remote server");
            }

            if (debug)
                Console.WriteLine("USER " + remoteUser);

            sendCommand("USER " + remoteUser);

            if (!(retValue == 331 || retValue == 230))
            {
                cleanup();
                throw new IOException(reply.Substring(4));
            }

            if (retValue != 230)
            {
                if (debug)
                    Console.WriteLine("PASS xxx");

                sendCommand("PASS " + remotePass);
                if (!(retValue == 230 || retValue == 202))
                {
                    cleanup();
                    throw new IOException(reply.Substring(4));
                }
            }

            logged = true;

            if (debug)
                Console.WriteLine("Connected to " + remoteHost);

            ChangeWorkingDirectory(remotePath);
        }

        /// <summary>
        /// Perform authentication with FTP server
        /// </summary>
        /// <param name="user">A username to FTP account</param>
        /// <param name="password">A password to FTP account</param>
        public void Authenticate(string user, string password)
        {
            remoteUser = user;
            remotePass = password;
            Authenticate();
        }

        /// <summary>
        /// Executes QUIT command and closes connection
        /// </summary>
        public void Close()
        {
            if (clientSocket != null)
                sendCommand("QUIT");
            cleanup();

            if (debug)
                Console.WriteLine("Connection closed");
        }


        /// <summary>
        /// Return a string array containing the remote directory's file list.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>file list as string array</returns>
        public string[] GetFileList(string mask)
        {
            if (!logged)
                Authenticate();

            Socket cSocket = createDataSocket();

            sendCommand("NLST " + mask);

            if (!(retValue == 150 || retValue == 125))
                throw new IOException(reply.Substring(4));
            
            mes = "";

            while (true)
            {
                int bytes = cSocket.Receive(buffer, buffer.Length, 0);
                mes += UTF8.GetString(buffer, 0, bytes);

                if (bytes < buffer.Length)
                    break;
            }

            string[] seperator = { "\r\n" };
            string[] mess = mes.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            cSocket.Close();

            readReply();

            if (retValue != 226)
                throw new IOException(reply.Substring(4));

            return mess;
        }
        
        /// <summary>
        /// Returns a string array containing the remote directory's extended file list.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>Extended file list</returns>
        public string[] GetExtendedFileList(string mask)
        {
            if (!logged)
                Authenticate();

            Socket cSocket = createDataSocket();

            sendCommand("LIST " + mask);

            if (!(retValue == 150 || retValue == 125))
                throw new IOException(reply.Substring(4));

            mes = "";

            while (true)
            {
                int bytes = cSocket.Receive(buffer, buffer.Length, 0);
                mes += UTF8.GetString(buffer, 0, bytes);

                if (bytes < buffer.Length)
                    break;
            }

            string[] seperator = { "\r\n" };
            string[] mess = mes.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            cSocket.Close();

            readReply();

            if (retValue != 226)
                throw new IOException(reply.Substring(4));

            return mess;
        }


        /// <summary>
        /// Return the size of a file
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <returns>File size</returns>
        public long GetFileSize(string filename)
        {
            if (!logged)
                Authenticate();
            
            sendCommand("SIZE " + filename);
            long size = 0;

            if (retValue == 213)
                size = Int64.Parse(reply.Substring(4));
            else
                throw new IOException(reply.Substring(4));
            
            return size;
        }


        /// <summary>
        /// If the value of mode is true, set binary mode for downloads,
        /// otherwise, set ASCII mode.
        /// </summary>
        /// <param name="mode">A binary mode</param>
        public void SetBinaryMode(Boolean mode)
        {
            if (mode)
                sendCommand("TYPE I");
            else
                sendCommand("TYPE A");
            
            if (retValue != 200)
                throw new IOException(reply.Substring(4));
        }

        /// <summary>
        /// Download a remote file to the Assembly's local directory,
        /// keeping the same file name, and set the resume flag.
        /// </summary>
        /// <param name="remFileName">A remote file name</param>
        /// <param name="resume">Resume flag</param>
        public void Download(string remFileName, Boolean resume)
        {
            Download(remFileName, "", resume);
        }
        
        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path, and set the resume flag. The local file name will be
        /// created or overwritten, but the path must exist.
        /// </summary>
        /// <param name="remFileName">A remote file name</param>
        /// <param name="locFileName">A local file name</param>
        /// <param name="resume">Resume flag</param>
        public void Download(string remFileName, string locFileName = "", Boolean resume = false)
        {
            if (!logged)
                Authenticate();
            
            SetBinaryMode(true);

            if(debug)
                Console.WriteLine("Downloading file " + remFileName + " from " + remoteHost + "/" + remotePath);

            if (locFileName.Equals(""))
                locFileName = remFileName;
            
            if (!File.Exists(locFileName))
            {
                Stream st = File.Create(locFileName);
                st.Close();
            }

            FileStream output = new FileStream(locFileName, FileMode.Open);
            Socket cSocket = createDataSocket();
            long offset = 0;

            if (resume)
            {
                offset = output.Length;
                if (offset > 0)
                {
                    sendCommand("REST " + offset);
                    if (retValue != 350)
                    {
                        //throw new IOException(reply.Substring(4));
                        //Some servers may not support resuming.
                        offset = 0;
                    }
                }

                if (offset > 0)
                {
                    if (debug)
                        Console.WriteLine("seeking to " + offset);
                    
                    long npos = output.Seek(offset, SeekOrigin.Begin);
                    if(debug)
                        Console.WriteLine("new pos=" + npos);
                }
            }

            sendCommand("RETR " + remFileName);

            if (!(retValue == 150 || retValue == 125))
                throw new IOException(reply.Substring(4));
            
            while (true)
            {
                bytes = cSocket.Receive(buffer, buffer.Length, 0);
                output.Write(buffer, 0, bytes);

                if (bytes <= 0)
                    break;
            }

            output.Close();
            if (cSocket.Connected)
                cSocket.Close();
            
            readReply();
            if (!(retValue == 226 || retValue == 250))
                throw new IOException(reply.Substring(4));

            SetBinaryMode(false);
        }
        
        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path, and set the resume flag. The local file name will be
        /// created or overwritten, but the path must exist.
        /// </summary>
        /// <param name="remFileName">A remote file name</param>
        /// <param name="locFileName">A local file name</param>
        /// <param name="resume">Resume flag</param>
        public void DownloadDirectory(string remote, string local = "", Boolean resume = false)
        {
            if (!logged)
                Authenticate();

            //SetBinaryMode(true);

            if (debug)
                Console.WriteLine("Downloading directory " + remote + " from " + remoteHost + "/" + remotePath);
            
            try {
                // Change working directory to given as parameter:
                ChangeWorkingDirectory(remote);

                // Create local directory:
                Directory.CreateDirectory(Path.Combine(local, remote));

                // Get directory files and directories:
                string[] paths = GetFileList("");
                int i = 0;
                foreach (string path in paths)
                {
                    paths[i] = path.Split('/').Last();
                    ++i;
                }

                // Perform downloading job:
                i = 0;
                foreach (string path in paths)
                {
                    if (path == "." || path == "..") continue;
                    //Console.WriteLine(path);

                    try
                    {
                        // Change working directory to new directory:
                        //ChangeWorkingDirectory(path);
                        // Recursive call, get all file:
                        DownloadDirectory(path, Path.Combine(local, remote), resume);
                        // Change working directory to prevoius:
                        ChangeWorkingDirectory("..");

                    }
                    catch (Exception ex)
                    {
                        // If changing directory failed, try to download it as file:
                        Download(path, Path.Combine(local, remote, path), true);
                    }
                }
            } catch(Exception ex)
            {
                throw new IOException(ex.Message);
            }
        }

        /// <summary>
        /// Upload a file to remote path and set the resume flag.
        /// </summary>
        /// <param name="fileName">A local file name</param>
        /// <param name="resume">Resume flag</param>
        public void Upload(string fileName, Boolean resume = false)
        {
            if (!logged)
                Authenticate();
            
            Socket cSocket = createDataSocket();
            long offset = 0;
            isUpload = true;
            if (resume)
            {
                try
                {
                    SetBinaryMode(true);
                    offset = GetFileSize(fileName);
                }
                catch (Exception)
                {
                    offset = 0;
                }
            }

            if (offset > 0)
            {
                sendCommand("REST " + offset);
                if (retValue != 350)
                {
                    //throw new IOException(reply.Substring(4));
                    //Remote server may not support resuming.
                    offset = 0;
                }
            }

            sendCommand("STOR " + Path.GetFileName(fileName));

            if (!(retValue == 125 || retValue == 150))
                throw new IOException(reply.Substring(4));
            
            // open input stream to read source file
            FileStream input = new FileStream(fileName, FileMode.Open);

            if (offset != 0)
            {
                if (debug)
                    Console.WriteLine("seeking to " + offset);
                input.Seek(offset, SeekOrigin.Begin);
            }

            if(debug)
                Console.WriteLine("Uploading file " + fileName + " to " + remotePath);

            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
                cSocket.Send(buffer, bytes, 0);
            
            input.Close();
            if (cSocket.Connected)
                cSocket.Close();
            
            readReply();
            if (!(retValue == 226 || retValue == 250))
                throw new IOException(reply.Substring(4));
        }

        /// <summary>
        /// Delete a file from the remote FTP server.
        /// </summary>
        /// <param name="filename">A name of a file</param>
        public void DeleteFile(string filename)
        {
            if (!logged)
                Authenticate();
            
            sendCommand("DELE " + filename);

            if (retValue != 250)
                throw new IOException(reply.Substring(4));
        }

        /// <summary>
        /// Rename a file on the remote FTP server.
        /// </summary>
        /// <param name="oldFileName">An old name of a file</param>
        /// <param name="newFileName">A new name of a file</param>
        public void RenameFile(string oldFileName, string newFileName)
        {
            if (!logged)
                Authenticate();
            
            sendCommand("RNFR " + oldFileName);

            if (retValue != 350)
                throw new IOException(reply.Substring(4));
            
            // known problem
            // rnto will not take care of existing file.
            // i.e. It will overwrite if newFileName exist
            sendCommand("RNTO " + newFileName);
            if (retValue != 250)
                throw new IOException(reply.Substring(4));
        }

        /// <summary>
        /// Create a directory on the remote FTP server in remote path.
        /// </summary>
        /// <param name="dirName">A directory name</param>
        public void MakeDirectory(string dirName)
        {
            if (!logged)
                Authenticate();
            
            sendCommand("MKD " + dirName);

            if (retValue != 257)
                throw new IOException(reply.Substring(4));
        }

        /// <summary>
        /// Delete a directory on the remote FTP server in remote path.
        /// </summary>
        /// <param name="dirName">A directory name</param>
        public void DeleteDirectory(string dirName)
        {
            if (!logged)
                Authenticate();
            
            sendCommand("RMD " + dirName);

            if (retValue != 250)
                throw new IOException(reply.Substring(4));
        }

        /// <summary>
        /// Change the current working directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName">A directory name</param>
        public void ChangeWorkingDirectory(string dirName)
        {
            if (dirName.Equals("."))
                return;
            
            if (!logged)
                Authenticate();
            
            sendCommand("CWD " + dirName);

            if (retValue != 250)
                throw new IOException(reply.Substring(4));
            
            this.remotePath = dirName;
            if(debug)
                Console.WriteLine("Current directory is " + remotePath);
        }

        /// <summary>
        /// Returns information about current working directory on remote FTP server.
        /// </summary>
        /// <returns>Working directory information</returns>
        public string PrintWorkingDirectory()
        {
            string wd = null;
            if (!logged)
                Authenticate();

            sendCommand("PWD");
            
            if (retValue >= 300)
                throw new IOException(reply.Substring(4));

            wd = reply.Substring(4);
            return wd;
        }

        /// <summary>
        /// Sends command to the FTP server
        /// </summary>
        /// <param name="command">A command string</param>
        public void sendCommand(String command)
        {
            Byte[] cmdBytes = Encoding.UTF8.GetBytes((command + "\r\n").ToCharArray());

            if (useStream)
                WriteMsg(command + "\r\n");
            else
                clientSocket.Send(cmdBytes, cmdBytes.Length, 0);

            readReply();
        }

        /// <summary>
        /// Creates new TCP Socket for data transmitting.
        /// </summary>
        /// <returns>A newly created data socket</returns>
        public Socket createDataSocket()
        {
            sendCommand("PASV");

            if (retValue != 227)
                throw new IOException(reply.Substring(4));

            int index1 = reply.IndexOf('(');
            int index2 = reply.IndexOf(')');
            string ipData = reply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];

            int len = ipData.Length;
            int partCount = 0;
            string buf = "";

            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                    buf += ch;
                else if (ch != ',')
                    throw new IOException("Malformed PASV reply: " + reply);

                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV reply: " + reply);
                    }
                }
            }

            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (parts[4] << 8) + parts[5];

            //Console.WriteLine("Data Socket: " + ipAddress + " port: " + port + " " + parts[4] + " " + parts[5]);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);

            try
            {
                s.Connect(ep);
                if (debug && s.Connected)
                    Console.WriteLine("Connected on port: " + ((IPEndPoint)s.LocalEndPoint).Port.ToString());
                
            }
            catch (Exception)
            {
                throw new IOException("Can't connect to remote server");
            }

            return s;
        }

        public void MakeDataConnection()
        {
            dataSocket = createDataSocket();
        }

        private void readReply()
        {
            if (useStream)
                reply = ResponseMsg();
            else
            {
                mes = "";
                reply = readLine();
                retValue = Int32.Parse(reply.Substring(0, 3));
            }
        }

        private void cleanup()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            logged = false;
        }

        private string readLine()
        {
            while (true)
            {
                if (useStream)
                    bytes = stream.Read(buffer, buffer.Length, 0);
                else
                    bytes = clientSocket.Receive(buffer, buffer.Length, 0);
                
                mes += UTF8.GetString(buffer, 0, bytes);
                if (bytes < buffer.Length)
                    break;
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            if (mes.Length > 2)
                mes = mess[mess.Length - 2];
            else
                mes = mess[0];
            
            if (!mes.Substring(3, 1).Equals(" "))
                return readLine();
            
            if (debug)
                for (int k = 0; k < mess.Length - 1; k++)
                    Console.WriteLine(mess[k]);
            
            return mes;
        }

        private void WriteMsg(string message)
        {
            System.Text.UTF8Encoding en = new System.Text.UTF8Encoding();

            byte[] WriteBuffer = new byte[BLOCK_SIZE];
            WriteBuffer = en.GetBytes(message);

            stream.Write(WriteBuffer, 0, WriteBuffer.Length);

            if(debug)
                Console.WriteLine(" WRITE:" + message);
        }

        private string ResponseMsg()
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] serverbuff = new Byte[BLOCK_SIZE];
            int count = 0;

            while (true)
            {
                byte[] buff = new Byte[2];
                int bytes = stream.Read(buff, 0, 1);
                if (bytes == 1)
                {
                    serverbuff[count] = buff[0];
                    count++;

                    if (buff[0] == '\n')
                        break;
                }
                else break;
            }

            string retval = enc.GetString(serverbuff, 0, count);
            if(debug)
                Console.WriteLine(" READ:" + retval);

            retValue = Int32.Parse(retval.Substring(0, 3));
            return retval;
        }
    }
}
