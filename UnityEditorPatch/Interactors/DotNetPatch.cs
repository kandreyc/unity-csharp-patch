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
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}