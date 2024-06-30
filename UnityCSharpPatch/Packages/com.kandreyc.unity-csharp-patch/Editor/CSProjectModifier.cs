using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace UnityCSharpPatch.Editor
{
    public static class CsProjectModifier
    {
        public static string Process(string path, string content)
        {
            var asmdefName = $"{Path.GetFileNameWithoutExtension(path)}.asmdef";

            if (!AsmdefUtility.TryFindAsmdef(asmdefName, out var info) || !info.IsModified)
            {
                return content;
            }

            var xDocument = XDocument.Parse(content);
            var xNamespace = xDocument.Root!.GetDefaultNamespace();
            var projectE = xDocument.Element(xNamespace.GetName("Project"));

            var children = new List<XElement>();

            if (!string.IsNullOrEmpty(info.LangVersion))
            {
                children.Add(new XElement(xNamespace.GetName("LangVersion"), info.LangVersion));
            }

            if (!string.IsNullOrEmpty(info.Nullable))
            {
                children.Add(new XElement(xNamespace.GetName("Nullable"), info.Nullable));
            }

            projectE!.Add(new XElement(xNamespace.GetName("PropertyGroup"), children));

            return xDocument.ToString();
        }
    }
}