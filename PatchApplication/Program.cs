using CommandLine;
using PatchApplication;
using PatchApplication.Input;
using PatchApplication.Interactors;

var result = Parser.Default
    .ParseArguments<ApplyPatchOptions, RevertPatchOptions>(args)
    .MapResult<RevertPatchOptions, ApplyPatchOptions, Result>
    (
        options => PatchReverter.Perform(options.EditorPath),
        options => PatchApplier.Perform(options.EditorPath),
        errors => Result.Error(errors.Select(e => e.ToString()).ToArray()!)
    );

Console.WriteLine(result);