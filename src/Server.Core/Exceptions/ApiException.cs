﻿using System;

namespace Server.Core.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiException(string message, object apiProblemDetails)
            : base(message) =>
            Data.Add(nameof(apiProblemDetails), apiProblemDetails);
    }
}
