using UnityEditorPatch.InfoProviders.Editor;
using UnityEditorPatch.InfoProviders.Sdk;
using static UnityEditorPatch.Utilities.FileSystemUtility;

namespace UnityEditorPatch.Interactors;

public class DotNetPatch
{
    public static bool TryPerform(SDKInfo sdkInfo, EditorInfo editorInfo)
    {
        try
        {
            ReplaceDirectory(editorInfo.RuntimeLocation, with: sdkInfo.Location);
            ReplaceDirectory(editorInfo.RoslynLocation, with: sdkInfo.RoslynLocation);

            ReplaceDirectory(editorInfo.DotNetSdkHostLocation, with: sdkInfo.HostLocation);
            ReplaceDirectory(editorInfo.DotNetSdkSharedLocation, with: sdkInfo.SharedLocation);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}