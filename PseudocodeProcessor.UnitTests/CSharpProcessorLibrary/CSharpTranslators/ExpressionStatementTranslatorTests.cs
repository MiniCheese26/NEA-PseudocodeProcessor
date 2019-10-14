using Microsoft.CodeAnalysis.CSharp;
using PseudocodeProcessor.CSharpProcessorLibrary;
using Xunit;

namespace PseudocodeProcessor.UnitTests.CSharpProcessorLibrary.CSharpTranslators
{
    public class ExpressionStatementTranslatorTests
    {
        private IPseudoCodeProcessor _pseudoCodeProcessor;
        
        [Theory]
        [InlineData("Console.WriteLine(\"Test\");")]
        [InlineData("Console.Write(\"Test\");")]
        [InlineData("Console.Write(\"Test\"   );")]
        public void GetPseudoCode_InvocationExpressionPassed_ReturnsCorrectPseudocode(string code)
        {
            _pseudoCodeProcessor = new CSharpProcessor(code, LanguageVersion.CSharp7_3);
            IPseudoCode result = _pseudoCodeProcessor.GetPseudoCode();
            
            Assert.False(result.ErrorEncountered);
            Assert.Equal("Procedure Main()\n    Output(\"Test\")\n", result.PseudoCode);
        }
    }
}