using System;

namespace PseudocodeProcessor
{
    public interface IPseudoCode
    {
        string Code { get; }
        bool ErrorEncountered { get; }
        string ErrorMessage { get; }
        Exception Exception { get; }
    }
}