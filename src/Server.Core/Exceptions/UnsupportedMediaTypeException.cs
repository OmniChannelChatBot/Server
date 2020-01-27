using System;

namespace Server.Core.Exceptions
{
    [Serializable]
    public class UnsupportedMediaTypeException : ApiException
    {
        public UnsupportedMediaTypeException(string message)
            : base(message)
        {
        }

        public UnsupportedMediaTypeException(string message, object apiProblemDetails)
            : base(message, apiProblemDetails)
        {
        }
    }
}
