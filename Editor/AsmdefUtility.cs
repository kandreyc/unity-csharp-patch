using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityCSharpPatch.Editor
{
    public static class AsmdefUtility
    {
        public static bool TryFindAsmdef(string asmdefName, out AsmdefInfo info)
        {
            var lookupDirectories = new[]
            {
                Application.dataPath,
                Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, "Packages")
            };

            foreach (var lookupDirectory in lookupDirectories)
            {
                var asmdefFile = Directory.GetFiles(lookupDirectory, asmdefName, SearchOption.AllDirectories)
                    .FirstOrDefault();

                if (asmdefFile is not null)
                {
                    info = GetAsmdefInfo(asmdefFile);
                    return true;
                }
            }

            info = default;
            return false;
        }

        public static AsmdefInfo GetAsmdefInfo(string pathToAsmdef)
        {
            if (!TryParse(File.ReadAllText(pathToAsmdef), out var parameters))
            {
                return default;
            }

            return new AsmdefInfo
            {
                Nullable = parameters.TryGetValue("nullable", out var nullable) ? nullable : string.Empty,
                LangVersion = parameters.TryGetValue("langVersion", out var langVersion) ? langVersion : string.Empty
            };
        }

        private static bool TryParse(string content, out Dictionary<string, string> parameters)
        {
            const string sectionPattern = "\"unityCSharpPatch\"\\s*:\\s*\\{(.+?)\\}";
            const string optionsPattern = "\"(.+?)\"\\s*:\\s*\"(.+?)\"";

            var match = Regex.Match(content, sectionPattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                parameters = default!;
                return false;
            }

            parameters = new Dictionary<string, string>();
            var csmodifierContent = match.Groups[1].Value;

            foreach (Match m in Regex.Matches(csmodifierContent, optionsPattern))
            {
                parameters[m.Groups[1].Value] = m.Groups[2].Value;
            }

            return true;
        }
    }
}