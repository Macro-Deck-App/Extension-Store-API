using System.Net;
using System.Net.Http.Headers;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Markdig;
using Newtonsoft.Json.Linq;
using Serilog;

namespace MacroDeckExtensionStoreLibrary.Services;

public class GitHubRepositoryService : IGitHubRepositoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger = Log.ForContext<GitHubRepositoryService>();

    private const string GitHubApi = "https://api.github.com";
    private const string RawGitHubUrl = "https://raw.githubusercontent.com";

    private readonly Dictionary<string, HttpResponseMessage?> _cache = new();

    public GitHubRepositoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    private static string? GetRepositoryNameFromUrl(string? repositoryUrl)
    {
        repositoryUrl = repositoryUrl?.Replace("www.", "")
            .Replace("https://", "")
            .Replace("github.com/", "")
            .Replace(".git", "");
        return repositoryUrl;
    }

    public async Task<string?> GetDefaultBranchName(string? repositoryUrl)
    {  
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var response = await MakeGetRequestAsync(GitHubApi, "repos", repositoryName);
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return "main"; // Fallback
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Verbose("Response: {ResponseString}", responseString);
        var defaultBranch = JObject.Parse(responseString)["default_branch"]?.ToString();
        return defaultBranch;
    }

    public async Task<string> GetReadmeAsync(string? repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var defaultBranchName = await GetDefaultBranchName(repositoryUrl);
        var response = await MakeGetRequestAsync(RawGitHubUrl, repositoryName, defaultBranchName, "README.md");
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return string.Empty;
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Verbose("Response: {ResponseString}", responseString);
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var result = Markdown.ToHtml(responseString, pipeline);
            
        return result;

    }
    
    public async Task<GitHubLicense> GetLicenseAsync(string? repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var gitHubLicense = new GitHubLicense();
        var response = await MakeGetRequestAsync(GitHubApi,"repos", repositoryName);
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return gitHubLicense;
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Verbose("Response: {ResponseString}", responseString);
        var licenseObject = JObject.Parse(responseString)["license"];
        gitHubLicense.Name = licenseObject?["name"]?.ToString() ?? string.Empty;
        gitHubLicense.Url = licenseObject?["url"]?.ToString() ?? string.Empty;

        return gitHubLicense;
    }

    public async Task<string> GetDescriptionAsync(string? repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var response = await MakeGetRequestAsync(GitHubApi, "repos", repositoryName);
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return string.Empty;
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Verbose("Response: {ResponseString}", responseString);
        var description = JObject.Parse(responseString)["description"]?.ToString();
        return description ?? string.Empty;
    }

    private async Task<HttpResponseMessage?> MakeGetRequestAsync(string api, string? endpoint, params string?[] parameters)
    {
        var requestUrl = $"{api}/{endpoint}/{string.Join("/",parameters)}";
        if (_cache.ContainsKey(requestUrl))
        {
            _logger.Verbose("Return cached {RequestUrl}", requestUrl);
            return _cache[requestUrl];
        }
        _logger.Verbose("Request url: {RequestUrl}", requestUrl);
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var productValue = new ProductInfoHeaderValue("MacroDeckExtensionStore", "1.0");
        request.Headers.UserAgent.Add(productValue);
        var response = await _httpClient.SendAsync(request);
        _cache.Add(requestUrl, response);
        return response;
    }
}