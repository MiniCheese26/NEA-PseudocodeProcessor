using System;

namespace PseudocodeProcessor
{
    internal class MethodResult
    {
        public bool Success { get; }
        public Exception Exception { get; }

        public MethodResult(bool success)
        {
            Success = success;
        }

        public MethodResult(bool success, Exception exception)
        {
            Success = success;
            Exception = exception;
        }
    }
}