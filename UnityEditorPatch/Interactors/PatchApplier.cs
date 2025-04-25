using UnityEditorPatch.InfoProviders.Editor;
using UnityEditorPatch.InfoProviders.Sdk;

namespace UnityEditorPatch.Interactors;

public static class PatchApplier
{
    public static Result Perform(string editorPath, bool allowPrerelease)
    {
        if (!SDKInfoProvider.TryGet(out var sdkInfo, allowPrerelease))
        {
            return Result.Error("Failed to select dotnet sdk.");
        }

        if (!EditorInfoProvider.TryGet(editorPath, out var editorInfo))
        {
            return Result.Error("Failed to get unity editor info.");
        }

        Console.WriteLine($"Editor: {editorInfo.Version}");

        if (editorInfo.IsPatched)
        {
            return Result.Error("Patch already applied.");
        }

        if (!Backup.TryPerform(editorInfo))
        {
            return Result.Error("Failed to perform backup before patching.");
        }

        if (!SourceGeneratorPatch.TryPerform(editorInfo))
        {
            return Result.Error("Failed to patch source generator.");
        }

        if (!DotNetPatch.TryPerform(sdkInfo, editorInfo))
        {
            return Result.Error("Failed to patch dotnet runtime / sdk.");
        }

        return Result.Success("Patch is applied.");
    }
}