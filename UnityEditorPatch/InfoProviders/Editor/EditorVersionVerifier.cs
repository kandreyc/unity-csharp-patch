namespace UnityEditorPatch.InfoProviders.Editor;

public class EditorVersionVerifier
{
    public static bool IsSupported(UnityVersion version)
    {
        return version >= new UnityVersion(2022);
    }
}