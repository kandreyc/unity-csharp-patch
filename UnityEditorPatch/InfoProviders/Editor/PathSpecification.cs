namespace UnityEditorPatch.InfoProviders.Editor;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class PathSpecification
{
    public string RuntimePath { get; init; }
    public string RoslynLocation { get; init; }
    public string[] SourceGeneratorLocations { get; init; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.