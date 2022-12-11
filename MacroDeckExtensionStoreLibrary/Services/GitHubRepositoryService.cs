using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Models;
using Markdig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace MacroDeckExtensionStoreLibrary.Services;

public class GitHubRepositoryService : IGitHubRepositoryService
{
    private readonly ILogger<GitHubRepositoryService> _logger;
    private readonly IGitHubRepositoryLicenseUrlParser _gitHubRepositoryLicenseUrlParser;
    private readonly HttpClient _httpClient;

    private const string gitHubAPI = "https://api.github.com";
    private const string rawGitHubUrl = "https://raw.githubusercontent.com";


    public GitHubRepositoryService(ILogger<GitHubRepositoryService> logger,
        IGitHubRepositoryLicenseUrlParser gitHubRepositoryLicenseUrlParser,
        HttpClient httpClient)
    {
        _logger = logger;
        _gitHubRepositoryLicenseUrlParser = gitHubRepositoryLicenseUrlParser;
        _httpClient = httpClient;
    }

    private static string GetRepositoryNameFromUrl(string repositoryUrl)
    {
        repositoryUrl = repositoryUrl.Replace("www.", "")
            .Replace("https://", "")
            .Replace("github.com/", "")
            .Replace(".git", "");

        return repositoryUrl;
    }

    public async Task<string> GetDefaultBranchName(string repositoryUrl)
    {  
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var response = await MakeGetRequestAsync(gitHubAPI, "repos", repositoryName);
        if (response is { StatusCode: HttpStatusCode.OK })
        {
            var responseString = await response.Content.ReadAsStringAsync();
            _logger.LogTrace($"Response: {responseString}");
            var defaultBranch = JObject.Parse(responseString)["default_branch"].ToString();
            return defaultBranch;
        }

        return "main"; // Fallback
    }

    public async Task<string> GetReadmeAsync(string repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var defaultBranchName = await GetDefaultBranchName(repositoryUrl);
        var response = await MakeGetRequestAsync(rawGitHubUrl, repositoryName, defaultBranchName, "README.md");
        if (response is { StatusCode: HttpStatusCode.OK })
        {
            var responseString = await response.Content.ReadAsStringAsync();
            _logger.LogTrace($"Response: {responseString}");
            
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var result = Markdown.ToHtml(responseString, pipeline);
            
            return result;
        }

        return "";
    }
    
    public async Task<GitHubLicense> GetLicenseAsync(string repositoryUrl)
    {
        var repositoryName = GetRepositoryNameFromUrl(repositoryUrl);
        var gitHubLicense = new GitHubLicense();
        var response = await MakeGetRequestAsync(gitHubAPI,"repos", repositoryName);
        if (response is { StatusCode: HttpStatusCode.OK })
        {
            var responseString = await response.Content.ReadAsStringAsync();
            _logger.LogTrace($"Response: {responseString}");
            var licenseObject = JObject.Parse(responseString)["license"];
            gitHubLicense.Name = licenseObject["name"].ToString();
            gitHubLicense.Url = licenseObject["url"].ToString();
        }
       
        return gitHubLicense;
    }

    private async Task<HttpResponseMessage?> MakeGetRequestAsync(string api, string endpoint, params string[] parameters)
    {
        var requestUrl = $"{api}/{endpoint}/{string.Join("/",parameters)}";
        _logger.LogTrace($"Request url: {requestUrl}");
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var productValue = new ProductInfoHeaderValue("MacroDeckExtensionStore", "1.0");
        request.Headers.UserAgent.Add(productValue);
        var response = await _httpClient.SendAsync(request);
        return response;
    }
}