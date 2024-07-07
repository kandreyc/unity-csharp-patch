using System.IO;
using UnityEditor;

namespace UnityCSharpPatch.Editor
{
    public static class PatchInfo
    {
        public static bool IsEditorPatched()
        {
            return Directory.Exists(Path.Combine(EditorApplication.applicationContentsPath, "__patch_backup"));
        }
    }
}