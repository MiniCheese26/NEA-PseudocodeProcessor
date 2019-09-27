using System;

namespace PseudocodeProcessor
{
    internal interface IResult
    {
        bool Success { get; }
        Exception Exception { get; }
    }
}