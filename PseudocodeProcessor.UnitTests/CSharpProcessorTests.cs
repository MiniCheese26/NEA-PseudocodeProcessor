using Microsoft.CodeAnalysis.CSharp;
using PseudocodeProcessor.CSharpProcessorLibrary;
using Xunit;

namespace PseudocodeProcessor.UnitTests
{
    public class CSharpProcessorTests
    {
        private IPseudoCodeProcessor _pseudoCodeProcessor;
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetPseudoCode_CodeIsEmptyOrNull_ErrorEncounteredIsTrue(string code)
        {
            _pseudoCodeProcessor = new CSharpProcessor(code, LanguageVersion.CSharp7_3);
            IPseudoCode result = _pseudoCodeProcessor.GetPseudoCode();
            
            Assert.True(result.ErrorEncountered);
        }
    }
}