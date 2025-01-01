using System.IO;
using System.Linq;
using System.Xml.Linq;
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
            var langVersionElement = xDocument.Find("LangVersion");

            if (info.Patch.TryGetValue("langVersion", out var version))
            {
                langVersionElement!.Value = version;
            }

            if (info.Patch.TryGetValue("nullable", out var nullable))
            {
                langVersionElement!.AddAfterSelf(xDocument.CreateElement("Nullable", nullable));
            }

            return xDocument.ToString();
        }

        private static XElement Find(this XDocument document, string name)
        {
            var fullName = document.Root!.GetDefaultNamespace().GetName(name);
            return document.Descendants().First(e => e.Name == fullName);
        }

        private static XElement CreateElement(this XDocument document, string name, string value)
        {
            return new XElement(
                document.Root!.GetDefaultNamespace().GetName(name),
                value
            );
        }
    }
}