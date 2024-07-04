using System.Diagnostics;

namespace UnityEditorPatch.Utilities;

public static class ProcessUtility
{
    public static string ReadOutputFrom(string command, string withArgument)
    {
        try
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = withArgument,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var reader = Process.Start(processStartInfo)!.StandardOutput;

            return reader.ReadToEnd();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}