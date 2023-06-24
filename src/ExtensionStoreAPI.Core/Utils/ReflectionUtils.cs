using System.Reflection;

namespace ExtensionStoreAPI.Core.Utils;

public static class ReflectionUtils
{
    public static IEnumerable<Assembly> GetMacroDeckAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName != null && x.FullName.StartsWith("MacroDeck")).ToArray();
        return assemblies;
    }

    public static IEnumerable<Type> GetMacroDeckTypes(Func<Type, bool> predicate)
    {
        var assemblies = GetMacroDeckAssemblies();
        var types = new List<Type>();
        foreach (var assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes().Where(predicate));
        }

        return types.ToArray();
    }
}