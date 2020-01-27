using System;

namespace Server.Core.Exceptions
{
    [Serializable]
    public class MethodNotAllowedException : ApiException
    {
        public MethodNotAllowedException(string message)
            : base(message)
        {
        }

        public MethodNotAllowedException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
