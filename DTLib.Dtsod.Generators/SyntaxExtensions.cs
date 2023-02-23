using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DTLib.Dtsod.Generic;

public static class SyntaxExtensions
{
    public static bool ContainsClassAttribute(this ClassDeclarationSyntax classDeclaration, string attributeName) => 
        classDeclaration.AttributeLists
            .Any(x => x.Attributes
                .Any(z => z.Name.ToString() == attributeName));

    public static bool ContainsFieldAttribute(this ClassDeclarationSyntax classDeclaration, string attributeName) =>
        classDeclaration.Members.OfType<FieldDeclarationSyntax>()
            .Any(e => e.AttributeLists
                .Any(z => z.Attributes
                    .Any(y => y.Name.ToString() == attributeName)));
}