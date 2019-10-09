using System;

namespace PseudocodeProcessor
{
    public interface IPseudoCode
    {
        string PseudoCode { get; }
        bool ErrorEncountered { get; }
        string ErrorMessage { get; }
        Exception Exception { get; }
    }
}