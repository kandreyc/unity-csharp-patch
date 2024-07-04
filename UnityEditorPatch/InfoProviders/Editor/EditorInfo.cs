#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UnityEditorPatch.InfoProviders.Editor;

public class EditorInfo
{
    public bool IsPatched { get; init; }
    public string Version { get; init; }
    public string Location { get; init; }
    public string ContentLocation { get; init; }
    public string RuntimeLocation { get; init; }
    public string RoslynLocation { get; init; }
    public string[] SourceGeneratorLocations { get; init; }
}

#pragma warning restore CS8618
