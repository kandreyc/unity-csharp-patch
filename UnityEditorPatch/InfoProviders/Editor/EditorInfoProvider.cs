using System.Runtime.InteropServices;
using UnityEditorPatch.Utilities;
using UnityEditorPatch.Interactors;

namespace UnityEditorPatch.InfoProviders.Editor;

public static class EditorInfoProvider
{
    public static bool TryGet(UnityVersion version, OSPlatform platform, string lookupPath, out EditorInfo info)
    {
        if (!PathSpecifications.TryGetLatest(version, platform, out PathSpecification pathSpecification))
        {
            info = null!;
            return false;
        }

        var contentPath = UnityLocationUtility.GetContentPath(lookupPath);
        var runtimePath = Path.Combine(contentPath, pathSpecification.RuntimePath);
        var roslynPath = Path.Combine(contentPath, pathSpecification.RoslynLocation);
        var sourceGeneratorLocations = pathSpecification.SourceGeneratorLocations
            .Select(location => Path.Combine(contentPath, location))
            .Where(File.Exists).ToArray();

        if (!FileSystemUtility.IsDirectoriesExists(contentPath, runtimePath, roslynPath) || sourceGeneratorLocations.Length is 0)
        {
            info = null!;
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