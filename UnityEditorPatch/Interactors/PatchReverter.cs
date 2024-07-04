using UnityEditorPatch.InfoProviders.Editor;

namespace UnityEditorPatch.Interactors;

public static class PatchReverter
{
    public static Result Perform(string editorPath)
    {
        if (!EditorInfoProvider.TryGet(editorPath, out var editorInfo))
        {
            return Result.Error("Failed to get unity editor info.");
        }

        Console.WriteLine($"Editor: {editorInfo.Version}");

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