using System;

namespace Server.Core.Exceptions
{
    [Serializable]
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
