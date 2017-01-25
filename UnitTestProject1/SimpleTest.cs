using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    using FTPClient;

    [TestClass]
    public class SimpleTest
    {

        private string Host = "";
        private string User = "";
        private string Pass = "";

        [TestMethod]
        public void RegistrySingleton()
        {
            Assert.AreSame(FTPClient.Registry.Application.getInstance(), FTPClient.Registry.Application.getInstance());
        }

        [TestMethod]
        public void FtpConnection()
        {
            string host = Host;
            try {
                FTPClient.FTP.Ftp ftp = new FTPClient.FTP.Ftp();

                // Set debug mode:
                ftp.Debug = false;

                // Connect and authenticate:
                ftp.Connect(host);

                Assert.IsTrue(ftp.Connected);

            } catch(Exception ex)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void FtpUserAuthentication()
        {
            string host = Host;
            string user = User;
            string pass = Pass;
            try
            {
                FTPClient.FTP.Ftp ftp = new FTPClient.FTP.Ftp();

                // Set debug mode:
                ftp.Debug = false;

                // Connect and authenticate:
                ftp.Connect(host);
                ftp.Authenticate(user, pass);

                Assert.IsTrue(ftp.Connected);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ChangeDirectory()
        {
            string host = Host;
            string user = User;
            string pass = Pass;
            try
            {
                FTPClient.FTP.Ftp ftp = new FTPClient.FTP.Ftp();

                // Set debug mode:
                ftp.Debug = false;

                // Connect and authenticate:
                ftp.Connect(host);
                ftp.Authenticate(user, pass);

                // Call cd:
                ftp.ChangeWorkingDirectory("public_html");

                Assert.IsTrue(ftp.Connected);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }
    }
}
