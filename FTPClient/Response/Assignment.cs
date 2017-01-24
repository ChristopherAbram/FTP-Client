using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Response
{
    public class Assignment : Dictionary<string, object>
    {
        public Assignment() : base() { }

        public void Assign(string index, object value)
        {
            try
            {
                if (index != null && index != "" && this.ContainsKey(index))
                    this[index] = value;
                else
                    this.Add(index, value);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to set '" + index + "' assignment index", ex);
            }
            return;
        }

        public void AssignAll(Assignment assignments)
        {
            foreach (var assignment in assignments)
                Assign(assignment.Key, assignment.Value);
            return;
        }

        public bool Has(string index)
        {
            try
            {
                return this.ContainsKey(index);
            } catch(System.Exception ex)
            {
                throw new Exception("Unable to check assignment", ex);
            }
            return false;
        }

        public void Merge(Assignment assignments)
        {
            foreach (var assignment in assignments)
                Assign(assignment.Key, assignment.Value);
        }

    }
}
