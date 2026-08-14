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
        var roslynPath = ResolveRoslynPath(contentPath, pathSpecification.RoslynLocation);
        var dotNetSdkHostPath = ResolveDotNetSdkSubdirectory(contentPath, roslynPath, "host");
        var dotNetSdkSharedPath = ResolveDotNetSdkSubdirectory(contentPath, roslynPath, "shared");
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
            DotNetSdkHostLocation = dotNetSdkHostPath,
            DotNetSdkSharedLocation = dotNetSdkSharedPath,
            IsPatched = Backup.IsBackupExist(contentPath),
            SourceGeneratorLocations = sourceGeneratorLocations
        };

        return true;
    }

    static string ResolveRoslynPath(string contentPath, string preferredRelativePath)
    {
        var preferredPath = Path.Combine(contentPath, preferredRelativePath);
        if (Directory.Exists(preferredPath))
        {
            return preferredPath;
        }

        var dotNetSdkPath = Path.Combine(contentPath, "DotNetSdk", "sdk");
        if (!Directory.Exists(dotNetSdkPath))
        {
            return preferredPath;
        }

        var roslynCandidates = Directory.GetDirectories(dotNetSdkPath)
            .Select(versionPath => Path.Combine(versionPath, "Roslyn", "bincore"))
            .Where(Directory.Exists)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return roslynCandidates.LastOrDefault() ?? preferredPath;
    }

    private static string? ResolveDotNetSdkSubdirectory(string contentPath, string roslynPath, string subdirectory)
    {
        var dotNetSdkPath = Path.Combine(contentPath, "DotNetSdk");
        if (!roslynPath.StartsWith(dotNetSdkPath, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var path = Path.Combine(dotNetSdkPath, subdirectory);
        return Directory.Exists(path) ? path : null;
    }
}
