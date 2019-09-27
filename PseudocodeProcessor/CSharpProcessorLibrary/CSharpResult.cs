using System;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpResult : IResult
    {
        public bool Success { get; }
        public Exception Exception { get; }

        public CSharpResult(bool success)
        {
            Success = success;
        }

        public CSharpResult(bool success, Exception exception)
        {
            Success = success;
            Exception = exception;
        }
    }
}