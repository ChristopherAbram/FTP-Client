using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPClient.XMLReader
{
    public class SettingsReader : XMLReader
    {

        private string filename = "";

        public SettingsReader(string filename)
        {
            this.filename = filename;
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new StreamReader(filename)))
            {
                while (reader.ReadToFollowing("var"))
                {
                    // Get name:
                    reader.MoveToAttribute("name");
                    string name = reader.Value;
                    
                    // Get value:
                    reader.MoveToContent();
                    string value = reader.ReadElementContentAsString();

                    yield return new KeyValuePair<string, string>(name, value);
                }
            }
        }
    }
}
