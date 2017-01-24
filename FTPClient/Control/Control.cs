using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Control
{
    public class Control
    {
        protected Controller.Application.Map _map = new Controller.Application.Map();

        protected string _filepath = "";

        public Control(string filepath)
        {
            _filepath = filepath;
        }

        public Controller.Application.Map Map
        {
            get { return _map; }
            protected set { _map = value; }
        }

        public virtual void Parse()
        {
            if (File.Exists(_filepath))
            {
                try
                {
                    XMLReader.ControlReader reader = new XMLReader.ControlReader(_filepath);
                    foreach(FTPClient.XMLReader.Command command in reader)
                    {
                        _map.addClassRoot(command.name, command.classroot);

                        foreach(var view in command.views)
                            _map.addView(command.name, view.Key, view.Value);
                        
                        foreach (var forward in command.forwards)
                            _map.addForward(command.name, forward.Key, forward.Value);
                    }
                } catch(System.Exception ex)
                {
                    throw new Exception("Unable to load control settings", ex);
                }
            }
            else
            {
                throw new Exception("Control flow file does not exists");
            }
            return;
        }


    }
}
