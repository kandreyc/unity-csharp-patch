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
        var dotNetSdkHostPath = CombineOptional(contentPath, pathSpecification.DotNetSdkHostLocation);
        var dotNetSdkSharedPath = CombineOptional(contentPath, pathSpecification.DotNetSdkSharedLocation);
        var sourceGeneratorLocations = pathSpecification.SourceGeneratorLocations
            .Select(location => Path.Combine(contentPath, location))
            .Where(File.Exists).ToArray();

        var requiredLocations = new[] { contentPath, runtimePath, roslynPath, dotNetSdkHostPath, dotNetSdkSharedPath }
            .OfType<string>().ToArray();

        if (!FileSystemUtility.IsDirectoriesExists(requiredLocations) || sourceGeneratorLocations.Length is 0)
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
            DotNetSdkHostLocation = dotNetSdkHostPath,
            DotNetSdkSharedLocation = dotNetSdkSharedPath,
            IsPatched = Backup.IsBackupExist(contentPath),
            SourceGeneratorLocations = sourceGeneratorLocations
        };

        return true;
    }

    private static string? CombineOptional(string contentPath, string? relativeLocation)
    {
        return relativeLocation is null ? null : Path.Combine(contentPath, relativeLocation);
    }
}
