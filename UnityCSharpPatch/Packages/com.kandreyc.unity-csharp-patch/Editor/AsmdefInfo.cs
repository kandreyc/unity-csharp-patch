namespace UnityCSharpPatch.Editor
{
    public struct AsmdefInfo
    {
        public string Nullable { get; set; }
        public string LangVersion { get; set; }

        public bool IsModified => !string.IsNullOrEmpty(Nullable) && !string.IsNullOrEmpty(LangVersion);
    }
}