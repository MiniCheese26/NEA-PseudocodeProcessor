using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpTranslator
    {
        private const string Indent = "    ";
        
        public string TranslateExpressionStatement(ExpressionStatementSyntax expressionStatementSyntax)
        {
            var invocation = expressionStatementSyntax.Expression;
            var o = invocation.Kind();

            return "";
        }

        public string TranslateClassDeclaration(ClassDeclarationSyntax classDeclarationSyntax)
        {
            string className = classDeclarationSyntax.Identifier.Text;

            return $"module {className}\n{Indent}";
        }

        public string TranslateMemberDeclaration(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            string methodName = methodDeclarationSyntax.Identifier.Text;
                        
            var methodParameters = methodDeclarationSyntax.ParameterList.Parameters
                .Select(x => x.Identifier.Text);
            string methodParametersJoined = string.Join(", ", methodParameters);
                        
            string returnType = methodDeclarationSyntax.ReturnType.ToString().Trim();

            string translatedMethodDeclaration =
                string.Equals("void", returnType, StringComparison.OrdinalIgnoreCase)
                    ? "procedure"
                    : "function";

            translatedMethodDeclaration += $" {methodName}({methodParametersJoined})\n{Indent}";

            return translatedMethodDeclaration;
        }
    }
}