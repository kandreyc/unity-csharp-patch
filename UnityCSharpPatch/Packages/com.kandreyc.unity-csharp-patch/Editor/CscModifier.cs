using System.IO;
using UnityEngine;
using UnityCSharpPatch.Editor.Asmdef;

namespace UnityCSharpPatch.Editor
{
    public static class CscModifier
    {
        private const string CscFileName = "csc.rsp";

        public static void ProcessImportedAsmdef(string pathToAsmdef)
        {
            if (!AsmdefUtility.TryGetAsmdefPatchInfo(pathToAsmdef, out var info)) return;

            if (!PatchInfo.IsEditorPatched())
            {
                Debug.LogError("[UnityCSharpPatch] Editor is not patched to support custom C# version.");
                return;
            }

            var content = string.Empty;

            if (!string.IsNullOrEmpty(info.LangVersion))
            {
                content += $"-langVersion:{info.LangVersion}";
            }

            if (!string.IsNullOrEmpty(info.Nullable))
            {
                content += '\n';
                content += $"-nullable:{info.Nullable}";
            }

            if (string.IsNullOrEmpty(content))
            {
                ProcessDeletedAsmdef(pathToAsmdef);
                return;
            }

            var cscFile = Path.Combine(Path.GetDirectoryName(info.Location.Absolute)!, CscFileName);

            File.WriteAllText(cscFile, content);
        }

        public static void ProcessDeletedAsmdef(string pathToAsmdef)
        {
            if (!PatchInfo.IsEditorPatched()) return;

            var cscFile = Path.Combine(Path.GetDirectoryName(pathToAsmdef)!, CscFileName);

            if (File.Exists(cscFile))
            {
                File.Delete(cscFile);
            }
        }
    }
}