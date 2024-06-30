using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using PatchApplication.Extensions;
using PatchApplication.InfoProviders.Editor;

namespace PatchApplication.Interactors;

//
// Starting with Unity 2022, Unity.SourceGenerators.dll is used by Unity to process
// .cs files and discover MonoBehaviour implementations in them. However, currently
// it uses the NamespaceDeclarationSyntax class, not BaseNamespaceDeclarationSyntax 
// which causes it to fail on FileScopedNamespaceDeclarationSyntax.
//
// We patch the TypeNameHelper.GetTypeInformation function to replace all references
// to NamespaceDeclarationSyntax with BaseNamespaceDeclarationSyntax, which fixes it.
//
public static class SourceGeneratorPatch
{
    private const string TargetMethod = "GetTypeInformation";
    private const string TargetType = "Unity.MonoScriptGenerator.TypeNameHelper";
    private const string CodeAnalysisAssembly = "Microsoft.CodeAnalysis.CSharp";
    private const string SyntaxName = "BaseNamespaceDeclarationSyntax";
    private const string SyntaxNamespace = "Microsoft.CodeAnalysis.CSharp.Syntax";
    private const string NamespaceDeclarationSyntax = "Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax";

    public static bool TryPerform(EditorInfo info)
    {
        var isAnyPatched = false;
        foreach (var sourceGeneratorLocation in info.SourceGeneratorLocations)
        {
            if (!TryPatchSourceGenerator(sourceGeneratorLocation)) continue;

            isAnyPatched = true;
            Console.WriteLine($"Patched source generator at {Path.GetRelativePath(info.Location, sourceGeneratorLocation)}.");
        }

        return isAnyPatched;
    }

    private static bool TryPatchSourceGenerator(string path)
    {
        var assembly = AssemblyDefinition.FromFile(path);
        var module = assembly.Modules[0];
        var target = module.GetTypeByFullName(TargetType);
        var method = target?.GetMethodByName(TargetMethod);
        var patches = 0;

        if (target is null || method is null)
        {
            return false;
        }

        if (method.CilMethodBody is not { Instructions: var instructions } body)
        {
            return false;
        }

        var baseNamespaceDeclarationSyntax = module.DefaultImporter.ImportType(
            new TypeReference(module.GetAssemblyReferenceByName(CodeAnalysisAssembly), SyntaxNamespace, SyntaxName)
        );

        foreach (var local in body.LocalVariables)
        {
            if (local.VariableType is not TypeDefOrRefSignature { FullName: NamespaceDeclarationSyntax }) continue;

            local.VariableType = new TypeDefOrRefSignature(baseNamespaceDeclarationSyntax);
            patches++;
        }

        foreach (var instruction in instructions)
        {
            switch (instruction.Operand)
            {
                case TypeReference { FullName: NamespaceDeclarationSyntax }:
                {
                    instruction.Operand = baseNamespaceDeclarationSyntax;
                    patches++;
                    break;
                }
                case MemberReference { IsMethod: true, Parent: TypeReference { FullName: NamespaceDeclarationSyntax } } member:
                {
                    var reference = new MemberReference(baseNamespaceDeclarationSyntax, member.Name,
                        member.Signature as MemberSignature);
                    instruction.Operand = module.DefaultImporter.ImportMethod(reference);
                    patches++;
                    break;
                }
            }
        }

        if (patches > 0)
        {
            assembly.Write(path);
        }

        return true;
    }
}