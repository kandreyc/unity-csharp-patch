using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityCSharpPatch.Editor.Asmdef.Collector;

namespace UnityCSharpPatch.Editor.Asmdef
{
    public static class AsmdefCache
    {
        private static Dictionary<string, AssemblyDefinition> _map;

        public static IReadOnlyDictionary<string, AssemblyDefinition> Value => _map ??= Fetch();

        [InitializeOnLoadMethod]
        public static void Reset()
        {
            _map = null;
        }

        private static Dictionary<string, AssemblyDefinition> Fetch()
        {
            var locations = ProjectAsmdefCollector.Collect().Concat(LocalPackagesAsmdefCollector.Collect());
            var map = new Dictionary<string, AssemblyDefinition>();

            foreach (var location in locations)
            {
                var name = Path.GetFileNameWithoutExtension(location.Relative);
                var asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(location.Relative);

                if (!map.TryAdd(name, new AssemblyDefinition { Location = location, Asset = asset }))
                {
                    Debug.LogWarningFormat(
                        "Detected multiple {0}.asmdef files with the same name. This is not supported. None of them will be used.",
                        name
                    );
                }
            }

            return map;
        }
    }

    public class AssemblyDefinition
    {
        public Location Location { get; set; }
        public AssemblyDefinitionAsset Asset { get; set; }
    }
}