#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using NuGet.Versioning;

namespace PatchApplication.InfoProviders.Sdk;

public class SDKInfo
{
    public string Location { get; init; }
    public string SDKLocation { get; init; }

    public string RoslynLocation { get; init; }
    public SemanticVersion Version { get; init; }
    public string LatestCSharpVersion { get; init; }
}

#pragma warning restore CS8618