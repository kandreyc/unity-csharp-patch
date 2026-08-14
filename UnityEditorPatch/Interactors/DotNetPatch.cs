using UnityEditorPatch.InfoProviders.Editor;
using UnityEditorPatch.InfoProviders.Sdk;
using UnityEditorPatch.Utilities;

namespace UnityEditorPatch.Interactors;

public class DotNetPatch
{
    public static bool TryPerform(SDKInfo sdkInfo, EditorInfo editorInfo)
    {
        try
        {
            FileSystemUtility.ReplaceDirectory(editorInfo.RuntimeLocation, with: sdkInfo.Location);
            FileSystemUtility.ReplaceDirectory(editorInfo.RoslynLocation, with: sdkInfo.RoslynLocation);
            CopyDotNetSdkRuntimeSupport(editorInfo.DotNetSdkHostLocation, sdkInfo.Location, "host");
            CopyDotNetSdkRuntimeSupport(editorInfo.DotNetSdkSharedLocation, sdkInfo.Location, "shared");
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private static void CopyDotNetSdkRuntimeSupport(string? targetDirectory, string sdkRoot, string subdirectory)
    {
        if (string.IsNullOrEmpty(targetDirectory))
        {
            return;
        }

        var sourceDirectory = Path.Combine(sdkRoot, subdirectory);
        if (!Directory.Exists(sourceDirectory))
        {
            return;
        }

        FileSystemUtility.CopyDirectory(sourceDirectory, targetDirectory);
    }
}
