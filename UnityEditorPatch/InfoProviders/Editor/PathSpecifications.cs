using System.Runtime.InteropServices;

using static System.IO.Path;
using PathSpecificationMap = System.Collections.Generic.Dictionary<System.Runtime.InteropServices.OSPlatform, UnityEditorPatch.InfoProviders.Editor.PathSpecification>;

namespace UnityEditorPatch.InfoProviders.Editor;

public static class PathSpecifications
{
    private static readonly List<(UnityVersion version, PathSpecificationMap platforms)> Specifications =
    [
        (new UnityVersion(2022, 0), new PathSpecificationMap
        {
            [OSPlatform.OSX] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = "DotNetSdkRoslyn",
                SourceGeneratorLocations =
                [
                    Combine("Tools", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
            [OSPlatform.Linux] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = "DotNetSdkRoslyn",
                SourceGeneratorLocations =
                [
                    Combine("Tools", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
            [OSPlatform.Windows] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = "DotNetSdkRoslyn",
                SourceGeneratorLocations =
                [
                    Combine("Tools", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
        }),

        (new UnityVersion(6000, 3), new PathSpecificationMap
        {
            [OSPlatform.OSX] = new()
            {
                RuntimePath = Combine("Resources", "Scripting", "NetCoreRuntime"),
                RoslynLocation = Combine("Resources", "Scripting", "DotNetSdkRoslyn"),
                SourceGeneratorLocations =
                [
                    Combine("Resources", "BuildPipeline", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Resources", "BuildPipeline", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
            [OSPlatform.Windows] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = "DotNetSdkRoslyn",
                SourceGeneratorLocations =
                [
                    Combine("Tools", "BuildPipeline", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "BuildPipeline", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
        })
    ];

    public static bool TryGetLatest(UnityVersion unityVersion, OSPlatform platform, out PathSpecification pathSpecification)
    {
        var platforms = Specifications.LastOrDefault(t => unityVersion >= t.version).platforms;

        pathSpecification = platforms?[platform]!;
        return pathSpecification != null!;
    }
}