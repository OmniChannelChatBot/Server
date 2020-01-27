using System;

namespace Server.Core.Exceptions
{
    [Serializable]
    public class PreconditionFailedException : ApiException
    {
        public PreconditionFailedException(string message)
            : base(message)
        {
        }

        public PreconditionFailedException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
