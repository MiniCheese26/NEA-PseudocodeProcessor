using System;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    public class CSharpPseudoCode : IPseudoCode
    {
        public string PseudoCode { get; }
        public bool ErrorEncountered => string.IsNullOrWhiteSpace(PseudoCode) || !string.IsNullOrWhiteSpace(ErrorMessage) || Exception != null;
        public string ErrorMessage { get; }
        public Exception Exception { get; }

        public CSharpPseudoCode(string code, string errorMessage = "")
        {
            PseudoCode = code;
            ErrorMessage = errorMessage;
        }

        public CSharpPseudoCode(string errorMessage, Exception exception)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }
    }
}