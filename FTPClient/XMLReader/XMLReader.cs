using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.XMLReader
{
    public abstract class XMLReader : System.Collections.IEnumerable
    {
        abstract public System.Collections.IEnumerator GetEnumerator();
        
    }
}
