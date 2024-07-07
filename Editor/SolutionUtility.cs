using System;
using Unity.CodeEditor;

namespace UnityCSharpPatch.Editor
{
    public static class SolutionUtility
    {
        public static void RegenerateProjects()
        {
            // HACK: Make it look like a dummy file has been added.
            var _ = Array.Empty<string>();
            var dummyFile = new[] { "RegenerateProject.cs" };
            CodeEditor.CurrentEditor.SyncIfNeeded(dummyFile, _, _, _, _);
        }
    }
}