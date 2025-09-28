using UnityEditorPatch.InfoProviders.Sdk;
using UnityEditorPatch.InfoProviders.Editor;
using UnityEditorPatch.Utilities;

namespace UnityEditorPatch.Interactors;

public static class PatchApplier
{
    public static Result Perform(string editorPath, bool allowPrerelease)
    {
        if (!OperatingSystemUtility.TryGetOSPlatform(out var platform))
        {
            return Result.Error("Platform is not supported.");
        }

        if (!UnityVersion.TryGet(editorPath, platform, out var version))
        {
            return Result.Error("Failed to parse editor version.");
        }

        Console.WriteLine($"Editor: {version}");

        if (!EditorVersionVerifier.IsSupported(version))
        {
            return Result.Error($"Editor version '{version}' is not supported.");
        }

        if (!SDKInfoProvider.TryGet(out var sdkInfo, allowPrerelease))
        {
            return Result.Error("Failed to select dotnet sdk.");
        }

        if (!EditorInfoProvider.TryGet(version, platform, editorPath, out var editorInfo))
        {
            return Result.Error("Failed to get unity editor info.");
        }

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