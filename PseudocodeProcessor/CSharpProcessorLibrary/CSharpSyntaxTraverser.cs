using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PseudocodeProcessor.CSharpProcessorLibrary
{
    internal class CSharpSyntaxTraverser // might inherit from CSharpSyntaxWalker instead of an interface, then override methods instead of a singular organise method
    {
        private readonly CompilationUnitSyntax _compilationUnitSyntax;
        private bool _codeOrganised;
        
        public ICollection<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();
        public ICollection<MethodDeclarationSyntax> Methods { get; } = new List<MethodDeclarationSyntax>();
        public bool HasClasses => Classes.Count == 0;
        public bool HasMethods => Methods.Count == 0;

        public CSharpSyntaxTraverser(CompilationUnitSyntax compilationUnitSyntax)
        {
            _compilationUnitSyntax = compilationUnitSyntax;
        }
        
        public MethodResult OrganiseCode() // this will be converted to MethodResult
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

            SortDeclarationMembers(rootMembers); // don't like using void here

            _codeOrganised = true;
            return new MethodResult(true);
        }

        private void SortDeclarationMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            foreach (MemberDeclarationSyntax memberDeclarationSyntax in members)
            {
                switch (memberDeclarationSyntax.Kind())
                {
                    case SyntaxKind.NamespaceDeclaration:
                        var namespaceDeclaration = (NamespaceDeclarationSyntax) memberDeclarationSyntax;
                        SyntaxList<MemberDeclarationSyntax> namespaceMembers = namespaceDeclaration.Members;
                        
                        SortDeclarationMembers(namespaceMembers);
                        break;
                    case SyntaxKind.ClassDeclaration:
                        var classDeclaration = (ClassDeclarationSyntax) memberDeclarationSyntax;
                        Classes.Add(classDeclaration);
                        SyntaxList<MemberDeclarationSyntax> classMembers = classDeclaration.Members;

                        SortDeclarationMembers(classMembers);
                        break;
                    case SyntaxKind.MethodDeclaration:
                        Methods.Add((MethodDeclarationSyntax) memberDeclarationSyntax);
                        break;
                }
            }
        }
    }
}