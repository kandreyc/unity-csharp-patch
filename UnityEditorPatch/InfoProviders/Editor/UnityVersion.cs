using System.Text.RegularExpressions;
using NuGet.Versioning;

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

    public static bool TryParse(string version, out UnityVersion semanticVersion)
    {
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
}