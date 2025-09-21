using UnityEditorPatch.InfoProviders.Editor.Versions;

namespace UnityEditorPatch.InfoProviders.Editor;

public static class EditorInfoProvider
{
    private static (UnityVersion version, IEditorInfoProvider provider)[] Providers { get; } =
    [
        (new UnityVersion(2022, 0), new EditorInfoProviderSince_2022()),
        (new UnityVersion(6000, 3), new EditorInfoProviderSince_6000_3())
    ];

    public static bool TryGet(UnityVersion version, string lookupPath, out EditorInfo info)
    {
        var provider = Providers.LastOrDefault(t => version >= t.version).provider;

        info = null!;
        return provider is not null && provider.TryGet(lookupPath, out info);
    }
}