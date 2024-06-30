using System.Runtime.Loader;
using NuGet.Versioning;

namespace PatchApplication.InfoProviders.Sdk;

public static class SDKInfoProvider
{
    public static bool TryGet(string sdkPath, out SDKInfo info)
    {
        // this version is used by unity
        var minVersion = SemanticVersion.Parse("6.0.21");

        if (!TryGetSdks(sdkPath, out var sdks))
        {
            info = default!;
            return false;
        }

        Console.WriteLine("dotnet sdks:");
        foreach (var sdkInfo in sdks)
        {
            Console.WriteLine($"- {sdkInfo.Version} (C# {sdkInfo.LatestCSharpVersion})");
        }

        var selectedSDK = sdks.LastOrDefault(sdk => sdk.Version > minVersion);

        if (selectedSDK is not null)
        {
            Console.WriteLine($"Selected Sdk: {selectedSDK.Version} (C# {selectedSDK.LatestCSharpVersion})");
            Console.WriteLine();
            info = selectedSDK;
            return true;
        }

        info = default!;
        return false;
    }

    private static bool TryGetSdks(string lookupPath, out IReadOnlyCollection<SDKInfo> sdks)
    {
        var sdksPath = Path.Combine(lookupPath, "sdk");

        if (!Directory.Exists(lookupPath) || !Directory.Exists(sdksPath))
        {
            sdks = default!;
            return false;
        }

        sdks = GetSdks(lookupPath, sdksPath).OrderBy(sdk => sdk.Version.ToString()).ToArray();
        return sdks.Count > 0;
    }

    private static IEnumerable<SDKInfo> GetSdks(string dotnetPath, string sdksPath)
    {
        foreach (var sdk in Directory.EnumerateDirectories(sdksPath))
        {
            if (!SemanticVersion.TryParse(Path.GetFileName(sdk), out var version)) continue;

            var roslynLocation = Path.Combine(sdk, "Roslyn", "bincore");

            if (!Directory.Exists(roslynLocation)) continue;

            yield return new SDKInfo
            {
                SDKLocation = sdk,
                Version = version,
                Location = dotnetPath,
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