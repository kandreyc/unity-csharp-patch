namespace UnityEditorPatch.InfoProviders.Editor;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class PathSpecification
{
    public string RuntimePath { get; init; }
    public string RoslynLocation { get; init; }

    // Set only for editors that run roslyn out of a dotnet sdk bundled with the editor,
    // because that sdk brings its own runtime that has to be patched alongside RuntimePath.
    public string? DotNetSdkHostLocation { get; init; }
    public string? DotNetSdkSharedLocation { get; init; }

    public string[] SourceGeneratorLocations { get; init; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.