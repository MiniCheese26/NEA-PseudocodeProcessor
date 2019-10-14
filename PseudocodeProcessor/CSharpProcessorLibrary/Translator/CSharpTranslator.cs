using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PseudocodeProcessor.CSharpProcessorLibrary.Translator
{
    internal class CSharpTranslator // might be able to use DI here, write some more code and see viability
    {
        private const string Indent = "    ";
        
        public string TranslateExpressionStatement(ExpressionStatementSyntax expressionStatementSyntax)
        {
            ExpressionSyntax expressionSyntax = expressionStatementSyntax.Expression;
            SyntaxKind expressionKind = expressionSyntax.Kind();

            var expressionTranslator = new ExpressionStatementTranslator();
            
            switch (expressionKind)
            {
                case SyntaxKind.InvocationExpression:
                    return expressionTranslator.TranslateInvocationExpression(
                        (InvocationExpressionSyntax) expressionSyntax);
            }
            
            return "";
        }

        public string TranslateClassDeclaration(ClassDeclarationSyntax classDeclarationSyntax)
        {
            string className = classDeclarationSyntax.Identifier.Text;

            return $"Module {className}\n{Indent}";
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
                    ? "Procedure"
                    : "Function";

            translatedMethodDeclaration += $" {methodName}({methodParametersJoined})\n{Indent}";

            return translatedMethodDeclaration;
        }
    }

    internal class ExpressionStatementTranslator
    {
        private readonly Dictionary<string, InvocationType> test = new Dictionary<string, InvocationType>(StringComparer.OrdinalIgnoreCase)
        {
            {"console.writeline", InvocationType.Output},
            {"console.write", InvocationType.Output}
        };
        
        public string TranslateInvocationExpression(InvocationExpressionSyntax invocationExpressionSyntax)
        {
            var exp = (MemberAccessExpressionSyntax) invocationExpressionSyntax.Expression;
            var j = exp.ToString();

            var k = test.ContainsKey(j);

            if (!k)
            {
                return invocationExpressionSyntax.ToString();
            }

            var y = test[j];
            var i = invocationExpressionSyntax.ArgumentList;

            switch (y)
            {
                case InvocationType.Output:
                    if (i.Arguments.Count <= 0)
                    {
                        return invocationExpressionSyntax.ToString();
                    }

                    string message = i.Arguments[0].ToString().Trim('"');

                    return $"Output(\"{message}\")\n";
                case InvocationType.Unknown:
                    
                    break;
                default:
                    
                    break;
            }
            return "";
        }
    }

    internal enum InvocationType
    {
        Unknown,
        Output
    }

    internal interface IExpressionStatementTranslator<in T> where T : ExpressionSyntax
    {
        string Translate(T statementSyntax);
    }

    internal class InvocationExpressionTranslator : IExpressionStatementTranslator<InvocationExpressionSyntax>
    {
        public string Translate(InvocationExpressionSyntax statementSyntax)
        {
            throw new NotImplementedException();
        }
    }
}