using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityCSharpPatch.Editor
{
    public class AssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] movedTo, string[] movedFrom)
        {
            var isAnyChange = false;

            foreach (var asset in imported)
            {
                if (!asset.EndsWith(".asmdef")) continue;

                isAnyChange = true;
                CscModifier.ProcessImportedAsmdef(Path.GetFileNameWithoutExtension(asset)); // path is wrong, and the problem is that i cant know the absolute path (local packages)
            }

            foreach (var asset in deleted)
            {
                if (!asset.EndsWith(".asmdef")) continue;

                isAnyChange = true;
                CscModifier.ProcessDeletedAsmdef(asset);
            }

            if (!isAnyChange)
            {
                return;
            }

            SolutionUtility.RegenerateProjects();
            AssetDatabase.Refresh();
        }

        public static string OnGeneratedCSProject(string path, string content)
        {
            return PatchInfo.IsEditorPatched()
                ? CsProjectModifier.Process(path, content)
                : content;
        }
    }
}