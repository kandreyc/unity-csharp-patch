using UnityEditorPatch.InfoProviders.Editor;

namespace UnityEditorPatch.Interactors;

public static class PatchReverter
{
    public static Result Perform(string editorPath)
    {
        if (!UnityVersion.TryParse(Path.GetFileName(editorPath), out var version))
        {
            return Result.Error("Failed to parse editor version.");
        }

        if (!EditorVersionVerifier.IsSupported(version))
        {
            return Result.Error($"Editor version '{version}' is not supported.");
        }

        if (!EditorInfoProvider.TryGet(version, editorPath, out var editorInfo))
        {
            return Result.Error("Failed to get unity editor info.");
        }

        Console.WriteLine($"Editor: {version}");

        if (!editorInfo.IsPatched)
        {
            return Result.Error("Failed to restore. Editor is not patched or backup data is missing.");
        }

        if (!Backup.TryRestore(editorInfo))
        {
            return Result.Error("Failed to restore patched data.");
        }

        return Result.Success("Patch is reverted.");
    }
}