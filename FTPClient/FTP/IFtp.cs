using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.FTP
{
    interface IFtp
    {
        void Connect();
        void Connect(string hostname);
        void Authenticate();
        void Authenticate(string user, string password);
        void Close();
        string[] GetFileList(string mask);
        long GetFileSize(string filename);
        void SetBinaryMode(Boolean mode);
        void Download(string remFileName, string locFileName = "", Boolean resume = false);
        void Download(string remFileName, Boolean resume);
        void Upload(string fileName, Boolean resume = false);
        void DeleteFile(string filename);
        void RenameFile(string oldFileName, string newFileName);
        void MakeDirectory(string dirName);
        void ChangeWorkingDirectory(string dirName);
        void sendCommand(String command);
        Socket createDataSocket();
    }
}
