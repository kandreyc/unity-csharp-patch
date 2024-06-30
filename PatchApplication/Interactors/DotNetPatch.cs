using PatchApplication.InfoProviders.Editor;
using PatchApplication.InfoProviders.Sdk;
using PatchApplication.Utilities;

namespace PatchApplication.Interactors;

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