using System.IO;
using System.Linq;
using UnityEditor;
using UnityCSharpPatch.Editor.Csc;

namespace UnityCSharpPatch.Editor
{
    public class AssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] movedTo, string[] movedFrom)
        {
            if (!imported.Any(IsCsc) && !deleted.Any(IsCsc))
            {
                return;
            }

            CscCache.Reset();
            SolutionUtility.RegenerateProjects();
            AssetDatabase.Refresh();
        }

        private static bool IsCsc(string asset)
        {
            return Path.GetFileName(asset) is "csc.rsp";
        }

        public static string OnGeneratedCSProject(string path, string content)
        {
            return PatchInfo.IsEditorPatched()
                ? CsProjectModifier.Process(path, content)
                : content;
        }
    }
}