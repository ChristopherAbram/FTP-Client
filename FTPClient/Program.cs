using FTPClient.Registry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Run application:
                Controller.Front.Console.run();

            } catch(System.Exception ex)
            {
                do
                {
                    Console.WriteLine(ex.Message);
                } while ((ex = ex.InnerException) != null);
            }
        }
    }
}
