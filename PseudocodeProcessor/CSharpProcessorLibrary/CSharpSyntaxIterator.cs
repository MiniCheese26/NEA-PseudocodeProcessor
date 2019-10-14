using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PseudocodeProcessor.CSharpProcessorLibrary.Translator;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpSyntaxIterator // might inherit from CSharpSyntaxWalker instead of an interface, then override methods instead of a singular organise method
    {
        public string TranslatedCode { get; private set; }
        private readonly CompilationUnitSyntax _compilationUnitSyntax;
        private readonly CSharpTranslator _cSharpTranslator;

        public CSharpSyntaxIterator(CompilationUnitSyntax compilationUnitSyntax)
        {
            _compilationUnitSyntax = compilationUnitSyntax;
            _cSharpTranslator = new CSharpTranslator();
        }

        public MethodResult TranslateCode()
        {
            if (_compilationUnitSyntax == null)
            {
                return new MethodResult(false, "Compilation unit was null");
            }
            
            SyntaxList<MemberDeclarationSyntax> rootMembers = _compilationUnitSyntax.Members;

            if (rootMembers.Count == 0)
            {
                return new MethodResult(false, "Code root contained no members");
            }
            
            if (rootMembers.All(x => x.Kind() != SyntaxKind.ClassDeclaration) &&
                rootMembers.All(x => x.Kind() != SyntaxKind.MethodDeclaration))
            {
                return new MethodResult(false, "No methods or classes found in code");
            }

            TranslatedCode = ProcessRootMembers(rootMembers);

            return new MethodResult(true);
        }

        private string ProcessRootMembers(SyntaxList<MemberDeclarationSyntax> memberDeclarations)
        {
            string translatedCode = string.Empty;
            
            foreach (MemberDeclarationSyntax declaration in memberDeclarations)
            {
                SyntaxKind memberKind = declaration.Kind();
                
                switch (memberKind)
                {
                    case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax) declaration;
                        SyntaxList<MemberDeclarationSyntax> classMembers = classDeclaration.Members;

                        translatedCode += _cSharpTranslator.TranslateClassDeclaration(classDeclaration);

                        translatedCode += ProcessRootMembers(classMembers);
                        break;
                    }
                    case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax) declaration;
                        SyntaxList<StatementSyntax> methodBodyStatements = methodDeclaration.Body.Statements;

                        translatedCode += _cSharpTranslator.TranslateMemberDeclaration(methodDeclaration);

                        foreach (StatementSyntax methodBodyStatement in methodBodyStatements)
                        { 
                            translatedCode += TranslateStatement(methodBodyStatement);
                        }
                        
                        break;
                    }
                    default:
                        return translatedCode;
                }
            }

            return translatedCode;
        }

        private string TranslateStatement(StatementSyntax statement)
        {
            SyntaxKind statementKind = statement.Kind();

            switch (statementKind)
            {
                case SyntaxKind.ExpressionStatement: // this doesn't work, to translate input, it needs the writeline and the assignment
                    return _cSharpTranslator.TranslateExpressionStatement((ExpressionStatementSyntax) statement);
                default:
                    return "";
            }
        }
    }
}