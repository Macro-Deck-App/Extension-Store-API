using System.Text;
using System.Web;
using ExtensionStoreAPI.Core.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace ExtensionStoreAPI.Logging;

public class MacroBotSink : ILogEventSink
{
    private readonly IServiceProvider _serviceProvider;

    public MacroBotSink(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < LogEventLevel.Warning
            || string.IsNullOrWhiteSpace(ExtensionStoreApiConfig.MacroBotLoggingUrl))
        {
            return;
        }

        var message = HttpUtility.JavaScriptStringEncode(logEvent.RenderMessage());

        var fields = new List<string>
        {
            "{\"name\":\"Time/Date (UTC)\n\",\"value\":\"" + DateTime.UtcNow + "\",\"inline\":false}",
            "{\"name\":\"Level\",\"value\":\"" + logEvent.Level + "\",\"inline\":false}"
        };

        if (logEvent.Exception is { } exception)
        {
            fields.Add("{\"name\":\"Message\",\"value\":\"" +
                       HttpUtility.JavaScriptStringEncode(exception.Message).Truncate(1024) +
                       "\",\"inline\":false}");
            fields.Add("{\"name\":\"Stack Trace\",\"value\":\"" +
                       HttpUtility.JavaScriptStringEncode(exception.StackTrace).Truncate(1024) +
                       "\",\"inline\":false}");
        }

        var requestBody =
            "{" +
                "\"toEveryone\":false," +
                "\"embed\":{" +
                    "\"color\":{\"r\":1,\"g\":"+ (logEvent.Level == LogEventLevel.Warning ? "0.7" : "0") + ",\"b\":0}," +
                    "\"description\":\"" + message + ".\"," +
                    "\"fields\":[" +
                        string.Join(",", fields) +
                    "]" +
                "}" +
            "}";
        
        var data = new StringContent(requestBody, Encoding.UTF8, "application/json");

        using var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ExtensionStoreApiConfig.MacroBotLoggingToken}");
        await httpClient.PostAsync(ExtensionStoreApiConfig.MacroBotLoggingUrl, data);
    }
}