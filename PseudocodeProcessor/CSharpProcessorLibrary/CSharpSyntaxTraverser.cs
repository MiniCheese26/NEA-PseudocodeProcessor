using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpSyntaxTraverser : ISyntaxTraverser
    {
        private readonly CompilationUnitSyntax _compilationUnitSyntax;

        public CSharpSyntaxTraverser(CompilationUnitSyntax compilationUnitSyntax)
        {
            _compilationUnitSyntax = compilationUnitSyntax;
        }
    }
}