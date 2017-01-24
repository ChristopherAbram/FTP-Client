using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Registry
{
    public class Application : Registry
    {

        private static Application instance = null;

        protected static Controller.Application.Map _map = null;

        protected string local_path = "";

        private Application() { }

        public void init()
        {
            try {
                XMLReader.SettingsReader reader = new XMLReader.SettingsReader("..\\..\\Private\\registry.settings.xml");
                foreach (KeyValuePair<string, string> keyval in reader)
                {
                    _set(keyval.Key, keyval.Value);
                }
            } catch(System.Exception ex)
            {
                throw new Exception("Unable to load registry settings", ex);
            }
        }

        public static Application getInstance()
        {
            if(instance == null)
            {
                instance = new Application();
            }
            return instance;
        }

        public string get(string index)
        {
            return (_get(index) as string);
        }

        public static string getControlFlowFilename()
        {
            Application a = Application.getInstance();
            return a.get("control_flow_file");
        }

        public static string getSettingsDirectoryName()
        {
            Application a = Application.getInstance();
            return a.get("settings_directory");
        }

        public static string getSettingsDirectoryAbsolutePath()
        {
            Application a = Application.getInstance();
            string settings_directory = a.get("settings_directory");
            string basePath = Path.Combine(Path.GetFullPath("."), "..", "..", settings_directory);
            return basePath;
        }

        public static string getControlFlowFilepath()
        {
            string filename = Application.getControlFlowFilename();
            string filepath = Path.Combine(Application.getSettingsDirectoryAbsolutePath(), filename);
            return filepath;
        }

        public static void setApplicationControllerMap(Controller.Application.Map map)
        {
            _map = map;
        }

        public static Controller.Application.Map getApplicationControllerMap()
        {
            return _map;
        }

        public static string getLocalDirectoryPath()
        {
            return getInstance().local_path;
        }

        public static void setLocalDirectoryPath(string path)
        {
            getInstance().local_path = path;
        }

    }
}
