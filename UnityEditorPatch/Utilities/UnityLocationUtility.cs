using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace UnityEditorPatch.Utilities;

public class UnityLocationUtility
{
    public static string GetContentPath(string lookupPath)
    {
        if (IsOSPlatform(OSPlatform.OSX))
        {
            return Path.Combine(lookupPath, "Unity.app", "Contents");
        }

        if (IsOSPlatform(OSPlatform.Linux) || IsOSPlatform(OSPlatform.Windows))
        {
            return Path.Combine(lookupPath, "Editor", "Data");
        }

        return lookupPath;
    }
}