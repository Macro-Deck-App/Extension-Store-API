using ExtensionStoreAPI.Core.Interfaces;

namespace ExtensionStoreAPI.Core.Parsers;

public class GitHubRepositoryLicenseUrlParser : IGitHubRepositoryLicenseUrlParser
{
    
    public string Parse(string repositoryUrl, string mainBranch)
    {
        var result = repositoryUrl
            .Replace("github.com", "raw.githubusercontent.com")
            + $"/{mainBranch}/LICENSE";
        
        return result;
    }
}