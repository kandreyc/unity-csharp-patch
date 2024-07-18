using System.Collections.Generic;

namespace UnityCSharpPatch.Editor.Csc
{
    public class CscInfo
    {
        public string AsmdefName { get; set; }
        public Location Location { get; set; }
        public Dictionary<string, string> Patch { get; set; }
    }
}