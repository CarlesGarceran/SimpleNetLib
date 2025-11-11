using System;

namespace SimpleNetLibCore.Exceptions
{
    public class NonImplementedMethodException : Exception
    {
        public NonImplementedMethodException(string reason = "Not Needed On This Side") : base($"This method has not been implemented due {reason}.")
        {
            
        }
    }
}
