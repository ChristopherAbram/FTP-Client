using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Registry
{
    public abstract class Registry
    {
        protected Dictionary<object, object> array = new Dictionary<object, object>();

        protected virtual void _set(object index, object value)
        {
            try
            {
                if (index != null && array.ContainsKey(index))
                {
                    array[index] = value;
                }
                else
                {
                    array.Add(index, value);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to set '" + index + "' registry index", ex);
            }
            return;
        }

        protected virtual object _get(object index)
        {
            try
            {
                if (array.ContainsKey(index))
                {
                    return array[index];
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("Unable to get '" + index + "' registry value", ex);
            }
            return null;
        }


    }
}
