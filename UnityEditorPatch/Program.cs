using CommandLine;
using UnityEditorPatch;
using UnityEditorPatch.Input;
using UnityEditorPatch.Interactors;

var result = Parser.Default
    .ParseArguments<ApplyPatchOptions, RevertPatchOptions>(args)
    .MapResult<RevertPatchOptions, ApplyPatchOptions, Result>
    (
        options => PatchReverter.Perform(options.EditorPath),
        options => PatchApplier.Perform(options.EditorPath),
        errors => Result.Error(errors.Select(e => e.ToString()).ToArray()!)
    );

Console.WriteLine(result);