using UnityEditorPatch.Interactors;
using UnityEditorPatch.Utilities;

namespace UnityEditorPatch.InfoProviders.Editor.Versions;

// ReSharper disable once InconsistentNaming
public class EditorInfoProviderSince_2022 : IEditorInfoProvider
{
    public bool TryGet(string lookupPath, out EditorInfo info)
    {
        var contentPath = UnityLocationUtility.GetContentPath(lookupPath);
        var runtimePath = Path.Combine(contentPath, "NetCoreRuntime");
        var roslynPath = Path.Combine(contentPath, "DotNetSdkRoslyn");
        var sourceGeneratorLocations = new[]
        {
            Path.Combine(contentPath, "Tools", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
            Path.Combine(contentPath, "Tools", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
        }.Where(File.Exists).ToArray();

        if (!FileSystemUtility.IsDirectoriesExists(contentPath, runtimePath, roslynPath) || sourceGeneratorLocations.Length is 0)
        {
            info = default!;
            return false;
        }

        info = new EditorInfo
        {
            Location = lookupPath,
            RoslynLocation = roslynPath,
            ContentLocation = contentPath,
            RuntimeLocation = runtimePath,
            IsPatched = Backup.IsBackupExist(contentPath),
            SourceGeneratorLocations = sourceGeneratorLocations
        };

        return true;
    }
}