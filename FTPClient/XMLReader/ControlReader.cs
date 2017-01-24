using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace FTPClient.XMLReader
{

    public struct Command
    {
        public string name;
        public string classroot;
        public Dictionary<FTPClient.Command.Command.Status, string> views;
        public Dictionary<FTPClient.Command.Command.Status, string> forwards;
    }

    class ControlReader : XMLReader
    {
        private string filename = "";

        public ControlReader(string filename)
        {
            this.filename = filename;
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            StreamReader stream = new StreamReader(filename);
            string content = stream.ReadToEnd();
            XDocument doc = XDocument.Parse(content);

            var commands = doc.Descendants("command");
            XAttribute attr = null;

            foreach (var command in commands)
            {
                Command c = new Command();

                // Getting command name:
                if ((attr = command.Attribute("name")) != null)
                    c.name = attr.Value;

                // Class name:
                var classroot = command.Element("classroot");
                if (classroot != null && (attr = classroot.Attribute("name")) != null)
                    c.classroot = attr.Value;

                // Getting views:
                var views = command.Descendants("view");
                c.views = new Dictionary<FTPClient.Command.Command.Status, string>();
                if (views != null)
                {
                    foreach(var view in views)
                    {
                        FTPClient.Command.Command.Status status = FTPClient.Command.Command.Status.DEFAULT;
                        if ((attr = view.Attribute("status")) != null)
                            status = (FTPClient.Command.Command.Status)Enum.Parse(typeof(FTPClient.Command.Command.Status), attr.Value);

                        c.views.Add(status, view.Value);
                    }
                }

                // Getting forwards:
                var forwards = command.Descendants("forward");
                c.forwards = new Dictionary<FTPClient.Command.Command.Status, string>();
                if(forwards != null)
                {
                     foreach(var forward in forwards)
                     {
                         FTPClient.Command.Command.Status status = FTPClient.Command.Command.Status.DEFAULT;
                         if ((attr = forward.Attribute("status")) != null)
                             status = (FTPClient.Command.Command.Status)Enum.Parse(typeof(FTPClient.Command.Command.Status), attr.Value);

                         c.forwards.Add(status, forward.Value);
                     }
                }

                yield return c;
            }
        }
    }
}
