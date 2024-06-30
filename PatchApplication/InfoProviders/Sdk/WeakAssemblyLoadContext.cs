using System.Reflection;
using System.Runtime.Loader;

namespace PatchApplication.InfoProviders.Sdk;

public class WeakAssemblyLoadContext(string mainAssemblyToLoadPath) : AssemblyLoadContext(isCollectible: true)
{
    private readonly AssemblyDependencyResolver _resolver = new(mainAssemblyToLoadPath);

    protected override Assembly Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        if (assemblyPath == null)
        {
            var assumedPath = Path.Combine(mainAssemblyToLoadPath, assemblyName.Name + ".dll");
            if (File.Exists(assumedPath))
            {
                assemblyPath = assumedPath;
            }
        }

        return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null!;
    }
}