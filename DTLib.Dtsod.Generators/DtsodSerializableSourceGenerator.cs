using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DTLib.Dtsod.Generic;

[Generator]
public class DtsodSerializableSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var compilation = context.Compilation;
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);;
            var targetTypes = syntaxTree.GetRoot().DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(x => x
                    .ContainsClassAttribute(nameof(SerializeAttribute).Replace("Attribute", "")))
                .Select(x => semanticModel.GetDeclaredSymbol(x))
                .OfType<ITypeSymbol>();
            foreach (var classDecl in targetTypes)
            {
                System.Console.WriteLine("generating DtsodV23 serializer for class " +
                        $"{classDecl.ContainingNamespace}.{classDecl.Name}");
                string source = GenerateSource(classDecl);
                context.AddSource($"{classDecl.ContainingNamespace}.{classDecl.Name}.DtsodSerializable.g.cs", source);
            }
        }
    }
    
    
    static string GenerateSource(ITypeSymbol classDecl)
    {
        var fields = classDecl.GetMembers().OfType<IFieldSymbol>();
        var propertiesWithAttr = classDecl.GetMembers().OfType<IPropertySymbol>()
            .Where(p => p.GetAttributes()
                .Any(a => a.AttributeClass?.Name == nameof(SerializableAttribute)));
        var b = new StringBuilder().Append(@"
            using System;
            using DTLib.Dtsod;
            
            namespace ").Append(classDecl.ContainingNamespace).Append(@";
            public partial class ").Append(classDecl.Name).Append("\n{\n")
            .Append(@"
                public Dtsod ToDtsod()
                {
                    var dtsod = new DtsodV23();
            ").AppendDtsodSetters(fields)
            .AppendDtsodSetters(propertiesWithAttr)
            .Append(@"
                    return dtsod;
                }
            ")
            .Append("}\n");
        return b.ToString();
    }
}

internal static class GeneratorStringBuilderExtensions
{
    public static StringBuilder AppendDtsodSetters<TSymbol>(this StringBuilder b, IEnumerable<TSymbol> fields)
         where TSymbol : ISymbol
    {
        foreach (var field in fields)
        {
            AttributeData attrData = field.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass!.Name == nameof(NotSerializeAttribute));
            if (attrData is not null)
            {
                var attrArgs = attrData.ConstructorArguments.FirstOrDefault();
                if (attrArgs.IsNull)
                {
                    b.Append("        // serialization of ").Append(field.Name).Append(" is disabled\n");
                    continue;
                }

                b.Append("        // if(!(").Append(attrArgs.Value).Append("))\n")
                    .Append("    ");
            }
            b.Append("        dtsod[nameof(").Append(field.Name).Append(")] = ")
                .Append(field.Name).Append(";\n");
        }

        return b;
    }
}