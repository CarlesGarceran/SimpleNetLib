using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Store.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerFunctionAttribute : Attribute
    {
        public string functionName;

        public ServerFunctionAttribute()
        {
            this.functionName = Guid.NewGuid().ToString();
        }

        public ServerFunctionAttribute(string functionName)
        {
            this.functionName = functionName;
        }
    }
}
