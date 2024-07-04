namespace UnityEditorPatch.Utilities;

public static class FileSystemUtility
{
    public static void CopyDirectory(string sourceDirectory, string targetDirectory)
    {
        Console.WriteLine($"Copying directory '{sourceDirectory}' -> {targetDirectory}");
        CopyDirectoryInternal(sourceDirectory, targetDirectory);
    }

    private static void CopyDirectoryInternal(string sourceDirectory, string targetDirectory)
    {
        Directory.CreateDirectory(targetDirectory);

        foreach (var file in Directory.GetFiles(sourceDirectory))
        {
            File.Copy(file, Path.Combine(targetDirectory, Path.GetFileName(file)), true);
        }

        foreach (var directory in Directory.GetDirectories(sourceDirectory))
        {
            CopyDirectoryInternal(directory, Path.Combine(targetDirectory, Path.GetFileName(directory)));
        }
    }

    public static void ReplaceDirectory(string directory, string with)
    {
        if (!Directory.Exists(with)) return;

        Console.WriteLine($"Replacing directory '{directory}' -> '{with}'");
        Directory.Delete(directory, recursive: true);
        Directory.CreateDirectory(directory);
        CopyDirectoryInternal(with, directory);
    }

    public static void ReplaceFile(string file, string with)
    {
        if (!File.Exists(with)) return;

        Console.WriteLine($"Replacing file '{file}' -> '{with}'");
        File.Delete(file);
        File.Copy(with, file);
    }

    public static void CopyFile(string file, string destination)
    {
        Console.WriteLine($"Copying file '{file}' -> {destination}");
        File.Copy(file, destination);
    }
}