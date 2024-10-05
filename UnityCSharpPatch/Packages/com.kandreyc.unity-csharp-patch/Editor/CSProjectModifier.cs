using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using UnityCSharpPatch.Editor.Csc;

namespace UnityCSharpPatch.Editor
{
    public static class CsProjectModifier
    {
        public static string Process(string path, string content)
        {
            var asmdefName = Path.GetFileNameWithoutExtension(path);

            if (!CscCache.Value.TryGetValue(asmdefName, out var info))
            {
                return content;
            }

            var xDocument = XDocument.Parse(content);
            var xNamespace = xDocument.Root!.GetDefaultNamespace();
            var projectE = xDocument.Element(xNamespace.GetName("Project"));

            var children = new List<XElement>();

            if (info.Patch.TryGetValue("langVersion", out var version))
            {
                children.Add(new XElement(xNamespace.GetName("LangVersion"), version));
            }

            if (info.Patch.TryGetValue("nullable", out var nullable))
            {
                children.Add(new XElement(xNamespace.GetName("Nullable"), nullable));
            }

            projectE!.Add(new XElement(xNamespace.GetName("PropertyGroup"), children));

            return xDocument.ToString();
        }
    }
}