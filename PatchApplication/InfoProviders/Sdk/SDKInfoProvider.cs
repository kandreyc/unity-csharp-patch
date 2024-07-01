using NuGet.Versioning;
using System.Runtime.Loader;
using PatchApplication.Utilities;
using System.Text.RegularExpressions;

namespace PatchApplication.InfoProviders.Sdk;

public static partial class SDKInfoProvider
{
    [GeneratedRegex(@"(\d+\.\d+\.\d+)[\s]\[(.+)\]", RegexOptions.Multiline)]
    private static partial Regex SDKInfoRegex();

    public static bool TryGet(out SDKInfo info)
    {
        // this version is used by unity
        var minVersion = SemanticVersion.Parse("6.0.21");

        if (!TryGetSdks(out var sdks))
        {
            info = default!;
            return false;
        }

        Console.WriteLine("dotnet sdks:");
        foreach (var sdkInfo in sdks)
        {
            Console.WriteLine($"- {sdkInfo.Version} (C# {sdkInfo.LatestCSharpVersion}) at {sdkInfo.SDKLocation}");
        }

        var selectedSDK = sdks.LastOrDefault(sdk => sdk.Version > minVersion);

        if (selectedSDK is not null)
        {
            Console.WriteLine($"Selected Sdk: {selectedSDK.Version} (C# {selectedSDK.LatestCSharpVersion}) at {selectedSDK.SDKLocation}");
            Console.WriteLine();
            info = selectedSDK;
            return true;
        }

        info = default!;
        return false;
    }

    private static bool TryGetSdks(out IReadOnlyCollection<SDKInfo> sdks)
    {
        sdks = GetSdks().OrderBy(sdk => sdk.Version.ToString()).ToArray();
        return sdks.Count > 0;
    }

    private static IEnumerable<SDKInfo> GetSdks()
    {
        var listSdks = ProcessUtility.ReadOutputFrom(command: "dotnet", withArgument: "--list-sdks");

        foreach (Match match in SDKInfoRegex().Matches(listSdks))
        {
            var path = match.Groups[2].Value;
            var version = match.Groups[1].Value;
            var sdkLocation = Path.Combine(path, version);
            var dotnetLocation = Path.GetDirectoryName(path);
            var roslynLocation = Path.Combine(sdkLocation, "Roslyn", "bincore");

            if (!SemanticVersion.TryParse(version, out var semanticVersion)) continue;
            if (!Directory.Exists(path) || !Directory.Exists(sdkLocation) || !Directory.Exists(dotnetLocation)) continue;

            yield return new SDKInfo
            {
                SDKLocation = sdkLocation,
                Location = dotnetLocation,
                Version = semanticVersion,
                RoslynLocation = roslynLocation,
                LatestCSharpVersion = GetLatestCSharpVersion(roslynLocation)
            };
        }
    }

    private static string GetLatestCSharpVersion(string roslynLocation)
    {
        AssemblyLoadContext context = default!;

        try
        {
            context = new WeakAssemblyLoadContext(roslynLocation);
            // Load the bundled Roslyn installation.
            var assembly = context.LoadFromAssemblyPath(Path.Combine(roslynLocation, "Microsoft.CodeAnalysis.CSharp.dll"));
            
            // We need the LanguageVersion and LanguageVersionFacts types.
            var versions = assembly.GetType("Microsoft.CodeAnalysis.CSharp.LanguageVersion")!;
            var facts = assembly.GetType("Microsoft.CodeAnalysis.CSharp.LanguageVersionFacts")!;

            // Retrieve the value of LanguageVersion.Latest, which we will resolve to a LangVersion string.
            var version = Enum.Parse(versions, "Latest");

            // Convert from "Latest" to a specific version.
            version = facts.GetMethod("MapSpecifiedToEffectiveVersion")!.Invoke(null, [version]);

            // Map the version to a LangVersion string.
            return (string)facts.GetMethod("ToDisplayString")!.Invoke(null, new[] { version })!;
        }
        catch (Exception)
        {
            return "none";
        }
        finally
        {
            context.Unload();
        }
    }
}