using ExtensionStoreAPI.Core.Utils;

namespace ExtensionStoreAPI.Setup;

public static class DependencyInjection
{
    public static void RegisterClassesEndsWithAsScoped(this IServiceCollection services, string endsWith)
    {
        var types = ReflectionUtils.GetMacroDeckTypes(type =>
            type.Name.EndsWith(endsWith) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in types)
        {
            var typeInterface = type.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{type.Name}");

            if (typeInterface != null)
            {
                services.AddScoped(typeInterface, type);
            }
        }
    }
}