using System;
using TinyJson;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

namespace UnityCSharpPatch.Editor.Csc
{
    public static class LocalPackagesCscCollector
    {
        private const string LocalVersionPrefix = "file:";
        private static readonly string ProjectRoot = Directory.GetParent(Application.dataPath)!.FullName;
        private static readonly string PackagesRoot = Path.Combine(ProjectRoot, "Packages");

        public static CscInfo[] Collect()
        {
            Location[] localPackages;
            if (TryGetFromPackageLock(out var packages))
            {
                localPackages = packages;
            }
            else if (TryGetFromManifest(out packages))
            {
                localPackages = packages;
            }
            else
            {
                return Array.Empty<CscInfo>();
            }

            return localPackages
                .Select(location => (location, assets: AssetDatabase.FindAssets($"t:{nameof(AssemblyDefinitionAsset)}", new[] { location.Relative })))
                .SelectMany(t => t.assets.Select(guid =>
                {
                    var (asmdefName, cscPath) = Unpack(guid);

                    var pathFromPackage = cscPath.Replace(t.location.Relative + Path.DirectorySeparatorChar, string.Empty);
                    var absolutePath = Path.Combine(t.location.Absolute, pathFromPackage);

                    var location = new Location
                    {
                        Relative = cscPath,
                        Absolute = absolutePath
                    };

                    return CscParser.TryParse(location.Absolute, out var info)
                        ? new CscInfo { AsmdefName = asmdefName, Location = location, Patch = info }
                        : default;

                }))
                .Where(info => info is not null)
                .ToArray();
        }

        private static bool TryGetFromManifest(out Location[] packages)
        {
            try
            {
                var location = Path.Combine(PackagesRoot, "manifest.json");

                if (!File.Exists(location))
                {
                    packages = Array.Empty<Location>();
                    return false;
                }

                packages = File.ReadAllText(location).FromJson<Manifest>()
                    .Dependencies
                    .Where(t => t.Value.StartsWith(LocalVersionPrefix))
                    .Select(t => CreateLocation(t.Key, t.Value))
                    .ToArray();

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                packages = Array.Empty<Location>();
                return false;
            }
        }

        private static bool TryGetFromPackageLock(out Location[] packages)
        {
            try
            {
                var location = Path.Combine(PackagesRoot, "packages-lock.json");

                if (!File.Exists(location))
                {
                    packages = Array.Empty<Location>();
                    return false;
                }

                packages = File.ReadAllText(location).FromJson<PackageLock>()
                    .Dependencies
                    .Where(t => t.Value.Source is "local" or "embedded")
                    .Where(t => t.Value.Version?.StartsWith(LocalVersionPrefix) ?? false)
                    .Select(t => CreateLocation(t.Key, t.Value.Version))
                    .ToArray();

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                packages = Array.Empty<Location>();

                return false;
            }
        }

        private static Location CreateLocation(string packageName, string packageVersion)
        {
            var projectRelative = Path.Combine("Packages", packageName);
            var packagesRelative = packageVersion.Replace(LocalVersionPrefix, string.Empty);
            var absolute = Path.GetFullPath(Path.Combine(PackagesRoot, packagesRelative));

            return new Location
            {
                Absolute = absolute,
                Relative = projectRelative,
            };
        }

        private static (string asmdefName, string cscPath) Unpack(string asmdef)
        {
            var asmdefPath = AssetDatabase.GUIDToAssetPath(asmdef);

            return (
                Path.GetFileNameWithoutExtension(asmdefPath),
                Path.Combine(Path.GetDirectoryName(asmdefPath)!, "csc.rsp")
            );
        }

        public class PackageLock
        {
            // ReSharper disable once CollectionNeverUpdated.Global
            public Dictionary<string, Package> Dependencies { get; set; }
        }

        public class Package
        {
            public string Source { get; set; }
            public string Version { get; set; }
        }

        public class Manifest
        {
            // ReSharper disable once CollectionNeverUpdated.Global
            public Dictionary<string, string> Dependencies { get; set; }
        }
    }
}