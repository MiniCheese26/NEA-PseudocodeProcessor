using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                
                string[] codeSplitOnNamespace = Regex.Split(codeJoined, @"^namespace\s\w+\s+{\s+",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);

                if (codeSplitOnNamespace.Length > 1)
                {
                    codeJoined = codeSplitOnNamespace[1];
                }

                if (!Regex.IsMatch(codeJoined,
                    @"(?:^\w+\s+(?:static\s+)?\w+\s+\w+(?:\(.*?\))?\s+{\s+)+",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline))
                {
                    codeJoined = "static void Main()\n{\n" + codeJoined;
                    codeJoined += "\n}"; // analyser doesn't care about incorrect numbers of curly brackets
                }

                _code = codeJoined;
            }
        }

        public CSharpProcessor(string code, LanguageVersion languageVersion, CancellationToken cancellationToken = default)
        {
            _languageVersion = languageVersion;
            _cancellationToken = cancellationToken;
            Code = code;
        }

        public IPseudoCode GetPseudoCode() // return type will most likely become more complex
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                return new CSharpPseudoCode("", "Code was null or empty");
            }
            
            MethodResult loadSyntaxTreeCSharpResult = LoadSyntaxTree();

            if (!loadSyntaxTreeCSharpResult.Success)
            {
                return new CSharpPseudoCode(loadSyntaxTreeCSharpResult.FailureMessage,
                    loadSyntaxTreeCSharpResult.Exception);
            }

            MethodResult loadCompilationUnitResult = LoadCompilationUnit();

            if (!loadCompilationUnitResult.Success)
            {
                return new CSharpPseudoCode(loadCompilationUnitResult.FailureMessage,
                    loadCompilationUnitResult.Exception);
            }

            var descTokens = _root.DescendantTokens();
            var descNodes = _root.DescendantNodes();
            var descTrivia = _root.DescendantTrivia();

            var e = _root.Members;
            var f = _root.Kind();
            var r = _root.ChildNodes();
            var p = _root.ChildTokens();
            var o = _root.GetText();
            var i = _root.GetDiagnostics();
            
            var traverser = new CSharpSyntaxIterator(_root);
            var b = traverser.TranslateCode();

            if (!b.Success)
            {
                return new CSharpPseudoCode("", b.FailureMessage);
            }

            return new CSharpPseudoCode("");
        }

        private MethodResult LoadSyntaxTree()
        {
            var parseOptions = new CSharpParseOptions(_languageVersion);

            try
            {
                _syntaxTree = CSharpSyntaxTree.ParseText(Code, parseOptions, cancellationToken: _cancellationToken);
            }
            catch (Exception ex)
            {
                return new MethodResult(false, ex, "Failed to parse CSharp code");
            }

            return new MethodResult(true);
        }

        private MethodResult LoadCompilationUnit()
        {
            if (_syntaxTree == null)
            {
                return new MethodResult(false, "Syntax tree was null");
            }

            try
            {
                _root = _syntaxTree.GetCompilationUnitRoot(_cancellationToken);
            }
            catch (Exception ex)
            {
                return new MethodResult(false, ex, "Failed to get CompilationUnitRoot");
            }

            return new MethodResult(true);
        }
    }
}