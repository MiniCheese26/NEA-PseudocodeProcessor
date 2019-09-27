using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpSyntaxTraverser : ISyntaxTraverser // might inherit from CSharpSyntaxWalker instead of an interface, then override methods instead of a singular organise method
    {
        private readonly CompilationUnitSyntax _compilationUnitSyntax;
        private bool _codeOrganised;
        
        public ICollection<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();
        public ICollection<MethodDeclarationSyntax> Methods { get; } = new List<MethodDeclarationSyntax>();

        public CSharpSyntaxTraverser(CompilationUnitSyntax compilationUnitSyntax)
        {
            _compilationUnitSyntax = compilationUnitSyntax;
        }
        
        public bool OrganiseCode()
        {
            List<SyntaxNode> rootChildNodes = _compilationUnitSyntax.ChildNodes().ToList();

            List<NamespaceDeclarationSyntax> namespaceNodes = rootChildNodes
                .Where(x => x.Kind() == SyntaxKind.NamespaceDeclaration)
                .Select(y => (NamespaceDeclarationSyntax) y)
                .ToList();

            var classNodes = new List<ClassDeclarationSyntax>();

            if (namespaceNodes.Count == 0)
            {
                classNodes.AddRange(rootChildNodes
                    .Where(x => x.Kind() == SyntaxKind.ClassDeclaration)
                    .Select(y => (ClassDeclarationSyntax) y)
                    .ToList());
            }
            else
            {
                // need to enumerate all namespacenodes and add all results from each enumeration
            }
            
            _codeOrganised = true;
            return true;
        }
    }
}