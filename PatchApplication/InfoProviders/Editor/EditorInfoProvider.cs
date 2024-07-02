using PatchApplication.Interactors;

namespace PatchApplication.InfoProviders.Editor;

public static class EditorInfoProvider
{
    public static bool TryGet(string lookupPath, out EditorInfo info)
    {
        var contentPath = GetContentPath(lookupPath);
        Console.WriteLine($"Content Path -> {contentPath}");
        var runtimePath = Path.Combine(contentPath, "NetCoreRuntime");
        var roslynPath = Path.Combine(contentPath, "DotNetSdkRoslyn");
        var sourceGeneratorLocations = new[]
        {
            Path.Combine(contentPath, "Tools", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
            Path.Combine(contentPath, "Tools", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
        }.Where(File.Exists).ToArray();

        if (!IsDirectoriesExists(contentPath, runtimePath, roslynPath) || sourceGeneratorLocations.Length is 0)
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
            Version = Path.GetFileName(lookupPath),
            IsPatched = Backup.IsBackupExist(contentPath),
            SourceGeneratorLocations = sourceGeneratorLocations
        };

        return true;
    }

#pragma warning disable CS0162 // Unreachable code detected
    private static string GetContentPath(string lookupPath)
    {
#if WINDOWS
        return Path.Combine(lookupPath, "Editor", "Data");
#elif LINUX
        return Path.Combine(lookupPath, "Editor", "Data");
#elif MACOS
        return Path.Combine(lookupPath, "Unity.app", "Contents");
#endif

        return lookupPath;
    }
#pragma warning restore CS0162

    private static bool IsDirectoriesExists(params string[] paths)
    {
        return paths.All(Directory.Exists);
    }
}