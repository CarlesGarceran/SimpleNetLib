using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Exceptions
{
    public class NonImplementedMethodException : Exception
    {
        public NonImplementedMethodException(string reason = "Not Needed On This Side") : base($"This method has not been implemented due {reason}.")
        {
            
        }
    }
}
