using AsmResolver.DotNet;

namespace UnityEditorPatch.Extensions;

public static class TypeDefinitionExtensions
{
    public static MethodDefinition? GetMethodByName(this TypeDefinition type, string name)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(name);

        foreach (MethodDefinition method in type.Methods)
        {
            if (method.Name == name)
            {
                return method;
            }
        }

        return null;
    }

    public static MethodDefinition? GetDefaultConstructor(this TypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        foreach (MethodDefinition method in type.Methods)
        {
            if (method.IsConstructor && method.Parameters.Count == 0)
            {
                return method;
            }
        }

        return null;
    }
}
