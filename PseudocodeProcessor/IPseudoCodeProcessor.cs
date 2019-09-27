namespace PseudocodeProcessor
{
    public interface IPseudoCodeProcessor
    {
        string Code { get; }
        IPseudoCode GetPseudoCode();
    }
}