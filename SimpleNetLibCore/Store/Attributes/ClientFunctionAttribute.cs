using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Store.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ClientFunctionAttribute : Attribute
    {
        public string functionName;

        public ClientFunctionAttribute()
        {
            this.functionName = Guid.NewGuid().ToString();
        }

        public ClientFunctionAttribute(string functionName)
        {
            this.functionName = functionName;
        }
    }
}
