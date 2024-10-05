using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace UnityCSharpPatch.Editor.Csc
{
    public static class ProjectCscCollector
    {
        public static CscInfo[] Collect()
        {
            var projectRoot = Directory.GetParent(Application.dataPath)!.FullName;

            return AssetDatabase.FindAssets($"t:{nameof(AssemblyDefinitionAsset)}", new[] { "Assets" })
                .Select(Unpack)
                .Select(t =>
                {
                    var location = new Location
                    {
                        Relative = t.cscPath,
                        Absolute = Path.Combine(projectRoot, t.cscPath)
                    };

                    return CscParser.TryParse(location.Absolute, out var info)
                        ? new CscInfo { AsmdefName = t.asmdefName, Location = location, Patch = info }
                        : default;
                })
                .Where(info => info is not null)
                .ToArray();
        }

        private static (string asmdefName, string cscPath) Unpack(string asmdef)
        {
            var asmdefPath = AssetDatabase.GUIDToAssetPath(asmdef);

            return (
                Path.GetFileNameWithoutExtension(asmdefPath),
                Path.Combine(Path.GetDirectoryName(asmdefPath)!, "csc.rsp")
            );
        }
    }
}