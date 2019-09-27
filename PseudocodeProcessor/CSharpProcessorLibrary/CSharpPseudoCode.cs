using System;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    public class CSharpPseudoCode : IPseudoCode
    {
        public string Code { get; }
        public bool ErrorEncountered => string.IsNullOrWhiteSpace(Code) || !string.IsNullOrWhiteSpace(ErrorMessage) || Exception != null;
        public string ErrorMessage { get; }
        public Exception Exception { get; }

        public CSharpPseudoCode(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public CSharpPseudoCode(string code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public CSharpPseudoCode(string errorMessage, Exception exception)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }
    }
}