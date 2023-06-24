using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using ExtensionStoreAPI.Core.DataTypes.GitHub;
using ExtensionStoreAPI.Core.Interfaces;
using Newtonsoft.Json.Linq;
using Serilog;

namespace ExtensionStoreAPI.Core.Services;

public class GitHubRepositoryService : IGitHubRepositoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger = Log.ForContext<GitHubRepositoryService>();

    private const string GitHubApi = "https://api.github.com";
    private const string RawGitHubUrl = "https://raw.githubusercontent.com";

    public GitHubRepositoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public static string? GetRepositoryNameFromUrl(string? repositoryUrl)
    {
        if (string.IsNullOrWhiteSpace(repositoryUrl))
        {
            return string.Empty;
        }
        const string pattern = @"(?:https?://)?(?:www\.)?github\.com/([\w-]+/[\w-]+)";
        var match = Regex.Match(repositoryUrl, pattern);
        return match.Success ? match.Groups[1].Value : null;
    }

    public async Task<string?> GetDefaultBranchName(string? repositoryUrl)
    {  
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var response = await GetRequestAsync(GitHubApi, "repos", repositoryName);
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return "main"; // Fallback
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Verbose("Response: {ResponseString}", responseString);
        
        return JObject.Parse(responseString)["default_branch"]?.ToString();
    }

    public async Task<string> GetReadmeAsync(string? repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var defaultBranchName = await GetDefaultBranchName(repositoryUrl);
        var response = await GetRequestAsync(RawGitHubUrl, repositoryName, defaultBranchName, "README.md");
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return string.Empty;
        }
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<GitHubLicense> GetLicenseAsync(string? repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var gitHubLicense = new GitHubLicense();
        var response = await GetRequestAsync(GitHubApi,"repos", repositoryName);
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
        var response = await GetRequestAsync(GitHubApi, "repos", repositoryName);
        if (response is not { StatusCode: HttpStatusCode.OK })
        {
            return string.Empty;
        }
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Verbose("Response: {ResponseString}", responseString);
        
        return JObject.Parse(responseString)["description"]?.ToString() ?? string.Empty;
    }

    private async Task<HttpResponseMessage?> GetRequestAsync(
        string api,
        string? endpoint,
        params string?[] parameters)
    {
        var requestUrl = $"{api}/{endpoint}/{string.Join("/",parameters)}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("MacroDeckExtensionStore", "1.0"));
        
        _logger.Verbose("Request url: {RequestUrl}", requestUrl);
        return await _httpClient.SendAsync(request);
    }
}