using System;
using System.IO;
using System.Collections.Generic;

namespace UnityCSharpPatch.Editor.Csc
{
    public static class CscParser
    {
        public static bool TryParse(string csc, out Dictionary<string, string> info)
        {
            if (!File.Exists(csc))
            {
                info = default;
                return false;
            }

            info = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(csc))
            {
                var option = line.Split(":", StringSplitOptions.RemoveEmptyEntries);

                if (option.Length != 2)
                {
                    continue;
                }

                var (name, value) = (option[0].Trim(), option[1].Trim());

                switch (name)
                {
                    case "-langVersion" when int.TryParse(value, out _):
                    {
                        info.TryAdd("langVersion", value);
                        break;
                    }
                    case "-nullable" when value is "enable" or "disable":
                    {
                        info.TryAdd("nullable", value);
                        break;
                    }
                }
            }

            return info.Count > 0;
        }
    }
}