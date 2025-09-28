using NuGet.Versioning;
using UnityEditorPatch.Utilities;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace UnityEditorPatch.InfoProviders.Editor;

public partial class UnityVersion : SemanticVersion
{
    public UnityVersion(int major) : base(major, 0, 0) { }
    public UnityVersion(int major, int minor) : base(major, minor, 0) { }
    public UnityVersion(int major, int minor, int patch) : base(major, minor, patch) { }
}

public partial class UnityVersion
{
    private static readonly Regex VersionRegex = new(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(?<type>[abf])(?<build>\d+)$");

    public static bool TryGet(string editorPath, OSPlatform platform, out UnityVersion semanticVersion)
    {
        var version = ReadVersion(editorPath, platform);
        var match = VersionRegex.Match(version);

        if (!match.Success)
        {
            semanticVersion = null!;
            return false;
        }

        semanticVersion = new UnityVersion(
            major: int.Parse(match.Groups["major"].Value),
            minor: int.Parse(match.Groups["minor"].Value),
            patch: int.Parse(match.Groups["patch"].Value)
        );

        return true;
    }

    private static string ReadVersion(string editorPath, OSPlatform platform)
    {
        if (platform == OSPlatform.OSX)
        {
            var executable = Path.Combine(editorPath, "Unity.app", "Contents", "MacOS", "Unity");
            return ProcessUtility.ReadOutputFrom(command: executable, withArgument: "-version");
        }

        if (platform == OSPlatform.Linux)
        {
            var executable = Path.Combine(editorPath, "Editor", "Unity");
            return ProcessUtility.ReadOutputFrom(command: executable, withArgument: "-version");
        }

        if (platform == OSPlatform.Windows)
        {
            var executable = Path.Combine(editorPath, "Editor", "Unity.exe");
            return ProcessUtility.ReadOutputFrom(command: executable, withArgument: "-version");
        }

        return string.Empty;
    }
}