#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using CommandLine;

namespace UnityEditorPatch.Input;

[Verb("apply", HelpText = "Apply patch.")]
public class ApplyPatchOptions
{
    [Option("editor", Required = true, HelpText = "Path to the Unity Editor.")]
    public string EditorPath { get; set; }

    [Option("allow-prerelease", HelpText = "Allow prerelease versions of SDKs.")]
    public bool AllowPrerelease { get; set; }
}

#pragma warning restore CS8618