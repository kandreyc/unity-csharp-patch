using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace UnityCSharpPatch.Editor.Csc
{
    public static class CscCache
    {
        private static Dictionary<string, CscInfo> _map;

        public static IReadOnlyDictionary<string, CscInfo> Value => _map ??= Fetch();

        [InitializeOnLoadMethod]
        public static void Reset()
        {
            _map = null;
        }

        private static Dictionary<string, CscInfo> Fetch()
        {
            var cscInfos = ProjectCscCollector.Collect().Concat(LocalPackagesCscCollector.Collect());
            var map = new Dictionary<string, CscInfo>();

            foreach (var info in cscInfos)
            {
                if (!map.TryAdd(info.AsmdefName, info))
                {
                    Debug.LogWarningFormat(
                        "Detected multiple {0}.asmdef files with the same name. This is not supported. None of them will be used.",
                        info.AsmdefName
                    );
                }
            }

            return map;
        }
    }
}