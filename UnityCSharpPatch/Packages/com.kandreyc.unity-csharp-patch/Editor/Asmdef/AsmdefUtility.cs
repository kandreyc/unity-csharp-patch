using TinyJson;
using UnityCSharpPatch.Editor.Asmdef.Collector;

namespace UnityCSharpPatch.Editor.Asmdef
{
    public static class AsmdefUtility
    {
        public static bool TryGetAsmdefPatchInfo(string asmdefName, out PatchInfo info)
        {
            if (!AsmdefCache.Value.TryGetValue(asmdefName, out var asmdef))
            {
                info = default;
                return false;
            }

            var data = asmdef.Asset.text.FromJson<Asmdef>()?.UnityCSharpPatch;

            if (data is null)
            {
                info = default;
                return false;
            }

            info = new PatchInfo
            {
                Location = asmdef.Location,
                Nullable = data.Nullable,
                LangVersion = data.LangVersion
            };

            return true;
        }

        public class PatchInfo
        {
            public Location Location { get; set; }
            public string Nullable { get; set; }
            public string LangVersion { get; set; }
        }

        public class Asmdef
        {
            public Data UnityCSharpPatch { get; set; }
        }

        public class Data
        {
            public string LangVersion { get; set; }
            public string Nullable { get; set; }
        }
    }
}