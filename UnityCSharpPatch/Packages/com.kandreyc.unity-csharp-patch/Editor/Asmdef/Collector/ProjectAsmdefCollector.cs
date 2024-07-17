using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace UnityCSharpPatch.Editor.Asmdef.Collector
{
    public static class ProjectAsmdefCollector
    {
        public static Location[] Collect()
        {
            var projectRoot = Directory.GetParent(Application.dataPath)!.FullName;

            return AssetDatabase.FindAssets($"t:{nameof(AssemblyDefinitionAsset)}", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(relative => new Location
                {
                    Relative = relative,
                    Absolute = Path.Combine(projectRoot, relative)
                })
                .ToArray();
        }
    }

    public class Location
    {
        public string Relative { get; set; }
        public string Absolute { get; set; }
    }
}