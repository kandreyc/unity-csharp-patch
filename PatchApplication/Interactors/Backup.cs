using PatchApplication.InfoProviders.Editor;
using PatchApplication.Utilities;

namespace PatchApplication.Interactors;

public static class Backup
{
    private const string BackupDirectory = "__patch_backup";

    public static bool IsBackupExist(string editorLocation)
    {
        return Directory.Exists(Path.Combine(editorLocation, BackupDirectory));
    }

    public static bool TryPerform(EditorInfo info)
    {
        try
        {
            if (IsBackupExist(info.ContentLocation))
            {
                return false;
            }

            var backupPath = Path.Combine(info.ContentLocation, BackupDirectory);

            Directory.CreateDirectory(backupPath);
            FileSystemUtility.CopyDirectory(info.RoslynLocation, BackupLocation(backupPath, info.RoslynLocation));
            FileSystemUtility.CopyDirectory(info.RuntimeLocation, BackupLocation(backupPath, info.RuntimeLocation));

            foreach (var sourceGeneratorLocation in info.SourceGeneratorLocations)
            {
                FileSystemUtility.CopyFile(sourceGeneratorLocation, BackupLocation(backupPath, sourceGeneratorLocation));
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public static bool TryRestore(EditorInfo info)
    {
        try
        {
            if (!IsBackupExist(info.ContentLocation))
            {
                return false;
            }

            var backupPath = Path.Combine(info.ContentLocation, BackupDirectory);

            FileSystemUtility.ReplaceDirectory(info.RoslynLocation, with: BackupLocation(backupPath, info.RoslynLocation));
            FileSystemUtility.ReplaceDirectory(info.RuntimeLocation, with: BackupLocation(backupPath, info.RuntimeLocation));

            foreach (var sourceGeneratorLocation in info.SourceGeneratorLocations)
            {
                FileSystemUtility.ReplaceFile(sourceGeneratorLocation, BackupLocation(backupPath, sourceGeneratorLocation));
            }

            Directory.Delete(backupPath, recursive: true);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private static string BackupLocation(string backupLocation, string location)
    {
        return Path.Combine(backupLocation, Path.GetFileName(location));
    }
}