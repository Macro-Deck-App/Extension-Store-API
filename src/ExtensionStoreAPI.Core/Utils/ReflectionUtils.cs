using System.Reflection;

namespace ExtensionStoreAPI.Core.Utils;

public static class ReflectionUtils
{
    private static IEnumerable<Assembly> GetExtensionStoreApiAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName != null && x.FullName.StartsWith("ExtensionStoreAPI")).ToArray();
        return assemblies;
    }

    public static IEnumerable<Type> GetExtensionStoreApiTypes(Func<Type, bool> predicate)
    {
        var assemblies = GetExtensionStoreApiAssemblies();
        var types = new List<Type>();
        foreach (var assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes().Where(predicate));
        }

        return types.ToArray();
    }
}