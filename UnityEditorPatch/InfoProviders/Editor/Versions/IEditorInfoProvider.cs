namespace UnityEditorPatch.InfoProviders.Editor.Versions;

public interface IEditorInfoProvider
{
    bool TryGet(string lookupPath, out EditorInfo info);
}