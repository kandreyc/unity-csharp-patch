using System.Runtime.InteropServices;

namespace UnityEditorPatch.Utilities;

public static class OperatingSystemUtility
{
    public static bool TryGetOSPlatform(out OSPlatform platform)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            platform = OSPlatform.OSX;
            return true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            platform = OSPlatform.Windows;
            return true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            platform = OSPlatform.Linux;
            return true;
        }

        platform = default;
        return false;
    }
}