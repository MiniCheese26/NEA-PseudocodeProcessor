using System;

namespace PseudocodeProcessor
{
    internal class MethodResult
    {
        public bool Success { get; }
        public Exception Exception { get; }
        public string FailureMessage { get; }

        public MethodResult(bool success, string failureMessage = "")
        {
            Success = success;
            FailureMessage = failureMessage;
        }

        public MethodResult(bool success, Exception exception, string failureMessage)
        {
            Success = success;
            Exception = exception;
            FailureMessage = failureMessage;
        }
    }
}