using System;

namespace Server.Core.Exceptions
{
    [Serializable]
    public class ServiceUnavailableException : ApiException
    {
        public ServiceUnavailableException(string message)
            : base(message)
        {
        }

        public ServiceUnavailableException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
