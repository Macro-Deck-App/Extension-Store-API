using System.ComponentModel;
using ExtensionStoreAPI.Core.Attributes;

namespace ExtensionStoreAPI.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue) 
    {
        var description = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString() 
                                                     ?? throw new InvalidOperationException());

        if (fieldInfo is null)
        {
            return description;
        }
        
        var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
        if (attrs.Length > 0)
        {
            description = ((DescriptionAttribute)attrs[0]).Description;
        }

        return description;
    }

    public static int? GetStatusCode(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString() 
                                                     ?? throw new InvalidOperationException());

        var attrs = fieldInfo?.GetCustomAttributes(typeof(StatusCodeAttribute), true);
        return attrs?.Length > 0 ? ((StatusCodeAttribute)attrs[0]).StatusCode : null;
    }
}