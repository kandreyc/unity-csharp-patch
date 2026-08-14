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
            }
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
            [OSPlatform.Linux] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = "DotNetSdkRoslyn",
                SourceGeneratorLocations =
                [
                    Combine("Tools", "BuildPipeline", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "BuildPipeline", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            }
        }),

        // 6000.5 replaced the standalone 'DotNetSdkRoslyn' folder with a whole dotnet sdk sitting in
        // the same place. Roslyn now lives inside it, and that sdk carries its own runtime next to
        // the standalone one, so both have to be patched.
        //
        // The sdk version is part of the path on purpose - an editor shipping a different one should
        // fail to resolve and get a specification of its own rather than silently pick something up.
        (new UnityVersion(6000, 5), new PathSpecificationMap
        {
            [OSPlatform.OSX] = new()
            {
                RuntimePath = Combine("Resources", "Scripting", "NetCoreRuntime"),
                RoslynLocation = Combine("Resources", "Scripting", "DotNetSdk", "sdk", "8.0.318", "Roslyn", "bincore"),
                DotNetSdkHostLocation = Combine("Resources", "Scripting", "DotNetSdk", "host"),
                DotNetSdkSharedLocation = Combine("Resources", "Scripting", "DotNetSdk", "shared"),
                SourceGeneratorLocations =
                [
                    Combine("Resources", "BuildPipeline", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Resources", "BuildPipeline", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
            [OSPlatform.Windows] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = Combine("DotNetSdk", "sdk", "8.0.318", "Roslyn", "bincore"),
                DotNetSdkHostLocation = Combine("DotNetSdk", "host"),
                DotNetSdkSharedLocation = Combine("DotNetSdk", "shared"),
                SourceGeneratorLocations =
                [
                    Combine("Tools", "BuildPipeline", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "BuildPipeline", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            },
            [OSPlatform.Linux] = new()
            {
                RuntimePath = "NetCoreRuntime",
                RoslynLocation = Combine("DotNetSdk", "sdk", "8.0.318", "Roslyn", "bincore"),
                DotNetSdkHostLocation = Combine("DotNetSdk", "host"),
                DotNetSdkSharedLocation = Combine("DotNetSdk", "shared"),
                SourceGeneratorLocations =
                [
                    Combine("Tools", "BuildPipeline", "Unity.SourceGenerators", "Unity.SourceGenerators.dll"),
                    Combine("Tools", "BuildPipeline", "Compilation", "Unity.SourceGenerators", "Unity.SourceGenerators.dll")
                ]
            }
        })
    ];

    public static bool TryGetLatest(UnityVersion unityVersion, OSPlatform platform, out PathSpecification pathSpecification)
    {
        var platforms = Specifications.LastOrDefault(t => unityVersion >= t.version).platforms;

        pathSpecification = platforms?[platform]!;
        return pathSpecification != null!;
    }
}