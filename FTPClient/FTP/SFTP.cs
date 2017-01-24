using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.FTP
{
    class SFtp : Ftp, IFtp
    {
        /*private static bool OnCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {

            Console.WriteLine("Server Certificate Issued To: {0}", certificate.GetName());

            Console.WriteLine("Server Certificate Issued By: {0}", certificate.GetIssuerName());


            // Return true if there are no policy errors

            // The certificate can also be manually verified to 

            //make sure it meets your specific // policies by 

            //   interrogating the x509Certificate object.

            if (errors != SslPolicyErrors.None)
            {

                Console.WriteLine("Server Certificate Validation Error");

                Console.WriteLine(errors.ToString());

                return false;

            }

            else
            {

                Console.WriteLine("No Certificate Validation Errors");

                return true;

            }

        }

        private void showCertificateInfo(X509Certificate remoteCertificate, bool verbose)
        {
            Console.WriteLine("Certficate Information for:\n{0}\n", remoteCertificate.GetName());
            Console.WriteLine("Valid From: \n{0}", remoteCertificate.GetEffectiveDateString());
            Console.WriteLine("Valid To: \n{0}", remoteCertificate.GetExpirationDateString());
            Console.WriteLine("Certificate Format: \n{0}\n", remoteCertificate.GetFormat());

            Console.WriteLine("Issuer Name: \n{0}", remoteCertificate.GetIssuerName());

            if (verbose)
            {
                Console.WriteLine("Serial Number: \n{0}", remoteCertificate.GetSerialNumberString());
                Console.WriteLine("Hash: \n{0}", remoteCertificate.GetCertHashString());
                Console.WriteLine("Key Algorithm: \n{0}", remoteCertificate.GetKeyAlgorithm());
                Console.WriteLine("Key Algorithm Parameters: \n{0}", remoteCertificate.GetKeyAlgorithmParametersString());
                Console.WriteLine("Public Key: \n{0}", remoteCertificate.GetPublicKeyString());
            }
        }
        private void showSslInfo(string serverName, SslStream sslStream, bool verbose)
        {
            showCertificateInfo(sslStream.RemoteCertificate, verbose);

            Console.WriteLine("\n\nSSL Connect Report for : {0}\n", serverName);
            Console.WriteLine("Is Authenticated: {0}", sslStream.IsAuthenticated);
            Console.WriteLine("Is Encrypted: {0}", sslStream.IsEncrypted);
            Console.WriteLine("Is Signed: {0}", sslStream.IsSigned);
            Console.WriteLine("Is Mutually Authenticated: {0}\n", sslStream.IsMutuallyAuthenticated);

            Console.WriteLine("Hash Algorithm: {0}", sslStream.HashAlgorithm);
            Console.WriteLine("Hash Strength: {0}", sslStream.HashStrength);
            Console.WriteLine("Cipher Algorithm: {0}", sslStream.CipherAlgorithm);
            Console.WriteLine("Cipher Strength: {0}\n", sslStream.CipherStrength);

            Console.WriteLine("Key Exchange Algorithm: {0}", sslStream.KeyExchangeAlgorithm);
            Console.WriteLine("Key Exchange Strength: {0}\n", sslStream.KeyExchangeStrength);
            Console.WriteLine("SSL Protocol: {0}", sslStream.SslProtocol);
        }

        public void getSslStream()
        {
            this.getSslStream(clientSocket);
        }

        public void getSslStream(Socket Csocket)
        {
            RemoteCertificateValidationCallback callback = new RemoteCertificateValidationCallback(OnCertificateValidation);
            SslStream _sslStream = new SslStream(new NetworkStream(Csocket));//,new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            try
            {
                _sslStream.AuthenticateAsClient(
                  remoteHost,
                  null,
                  System.Security.Authentication.SslProtocols.Ssl3 | System.Security.Authentication.SslProtocols.Tls,
                  true
                );

                if (_sslStream.IsAuthenticated)
                    if (isUpload)
                        stream2 = _sslStream;
                    else
                        stream = _sslStream;
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message);
            }

            if (debug)
                showSslInfo(remoteHost, _sslStream, true);

            // readReply();
        }
        
         ///
        /// Secure Upload a file and set the resume flag.
        ///
        ///
        ///
        public void uploadSecure(string fileName, Boolean resume)
        {

            sendCommand("PASV");

            if (retValue != 227)
            {
                throw new IOException(reply.Substring(4));
            }

            if (!logined)
            {
                Authenticate();
            }

            Socket cSocket = createDataSocket();
            isUpload = true;

            //this.getSslStream(cSocket);

            long offset = 0;

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
            {
                throw new IOException(reply.Substring(4));
            }


            FileStream input = File.OpenRead(fileName);
            byte[] bufferFile = new byte[input.Length];

            input.Read(bufferFile, 0, bufferFile.Length);
            input.Close();

            if (offset != 0)
            {

                if (debug)
                {
                    Console.WriteLine("seeking to " + offset);
                }
                input.Seek(offset, SeekOrigin.Begin);
            }

            Console.WriteLine("Uploading file " + fileName + " to " + remotePath);

            if (cSocket.Connected)
            {

                this.stream2.Write(bufferFile, 0, bufferFile.Length);
                Console.WriteLine("File Upload");

            }


            this.stream2.Close();
            if (cSocket.Connected)
            {
                cSocket.Close();
            }

            readReply();
            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }


        }
        
        
        */
    }
}
