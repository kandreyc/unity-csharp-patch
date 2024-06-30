using AsmResolver.DotNet;

namespace PatchApplication.Extensions;

public static class ModuleDefinitionExtensions
{
    public static TypeDefinition? GetTypeByFullName(this ModuleDefinition module, string typeName)
    {
        ArgumentNullException.ThrowIfNull(module);

        foreach (TypeDefinition type in module.GetAllTypes())
        {
            if (type.FullName == typeName)
            {
                return type;
            }
        }

        return null;
    }

    public static AssemblyReference? GetAssemblyReferenceByName(this ModuleDefinition module, string name)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(name);

        foreach (AssemblyReference reference in module.AssemblyReferences)
        {
            if (reference.Name == name)
            {
                return reference;
            }
        }

        return null;
    }
}
