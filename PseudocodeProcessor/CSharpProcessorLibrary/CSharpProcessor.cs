using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    public class CSharpProcessor : IPseudoCodeProcessor
    {
        private readonly LanguageVersion _languageVersion;
        private readonly CancellationToken _cancellationToken; // if i can use this correctly it'll be useful for cancelling a previous job so a new one can start immediately
        private SyntaxTree _syntaxTree;
        private CompilationUnitSyntax _root;

        private string _code;
        public string Code
        {
            get => _code;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                
                IEnumerable<string> linesTrimmed = value
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim());

                string codeJoined = string.Join(Environment.NewLine, linesTrimmed);

                _code = codeJoined;
            }
        }

        public CSharpProcessor(string code, LanguageVersion languageVersion = LanguageVersion.Latest, CancellationToken cancellationToken = default)
        {
            _languageVersion = languageVersion;
            _cancellationToken = cancellationToken;
            Code = code;
        }

        public IPseudoCode GetPseudoCode() // return type will most likely become more complex
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                return new CSharpPseudoCode("Code was null or empty");
            }
            
            CSharpResult loadSyntaxTreeCSharpResult = LoadSyntaxTree();

            if (!loadSyntaxTreeCSharpResult.Success)
            {
                return new CSharpPseudoCode("Failed to initialise SyntaxTree", loadSyntaxTreeCSharpResult.Exception);
            }

            CSharpResult loadCompilationUnitResult = LoadCompilationUnit();

            if (!loadCompilationUnitResult.Success)
            {
                return new CSharpPseudoCode("Failed to initialise CompilationUnit",
                    loadCompilationUnitResult.Exception);
            }
            
            ISyntaxTraverser syntaxTraverser = new CSharpSyntaxTraverser(_root);

            return new CSharpPseudoCode("", "");
        }

        private CSharpResult LoadSyntaxTree()
        {
            var parseOptions = new CSharpParseOptions(_languageVersion);

            try
            {
                _syntaxTree = CSharpSyntaxTree.ParseText(Code, parseOptions, cancellationToken: _cancellationToken);
            }
            catch (Exception ex)
            {
                return new CSharpResult(false, ex);
            }

            return new CSharpResult(true);
        }

        private CSharpResult LoadCompilationUnit()
        {
            if (_syntaxTree == null)
            {
                return new CSharpResult(false);
            }

            try
            {
                _root = _syntaxTree.GetCompilationUnitRoot(_cancellationToken);
            }
            catch (Exception ex)
            {
                return new CSharpResult(false, ex);
            }

            return new CSharpResult(true);
        }
    }
}