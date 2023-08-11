namespace ExtensionStoreAPI.Core.Attributes;

public class StatusCodeAttribute : Attribute
{
    public int StatusCode { get; set; }

    public StatusCodeAttribute(int statusCode)
    {
        StatusCode = statusCode;
    }
}