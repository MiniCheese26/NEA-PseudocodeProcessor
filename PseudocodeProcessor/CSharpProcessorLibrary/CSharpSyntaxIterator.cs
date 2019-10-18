using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PseudocodeProcessor.CSharpProcessorLibrary.Translator;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpSyntaxTranslatorBase // this setup might work... will need to write some pseudocode and plan it out
    {
        protected MethodDeclarationSyntax CurrentMethodSyntax;
        protected SyntaxList<StatementSyntax> CurrentMethodStatements => CurrentMethodSyntax.Body.Statements;
        protected StatementSyntax CurrentStatement => CurrentMethodStatements[CurrentStatementIndex];
        protected int CurrentStatementIndex;
    }

    internal class CSharpSyntaxTranslator : CSharpSyntaxTranslatorBase
    {
        public void Ma()
        {

        }
    }

    internal class CSharpStatementTranslator : CSharpSyntaxTranslator
    {
        public void M()
        {
            Cu
        }
    }

    internal class CSharpSyntaxIterator
    {
        public string TranslatedCode { get; private set; }
        private readonly CompilationUnitSyntax _compilationUnitSyntax;
        private readonly CSharpTranslator _cSharpTranslator;

        public CSharpSyntaxIterator(CompilationUnitSyntax compilationUnitSyntax)
        {
            _compilationUnitSyntax = compilationUnitSyntax;
            _cSharpTranslator = new CSharpTranslator();
        }

        public MethodResult TranslateCode() // this needs a total re-write, any translation method/class needs access to the current method syntax, for example if a writeline is encountered, need to check following statements for a readline
        { // could use inheritance, base class with CurrentMethodSyntax field, would prefer composition though
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
                case SyntaxKind.ExpressionStatement: // this doesn't work, to translate input, it needs the writeline and the assignment, needs total re-design
                    return _cSharpTranslator.TranslateExpressionStatement((ExpressionStatementSyntax) statement);
                default:
                    return "";
            }
        }
    }
}