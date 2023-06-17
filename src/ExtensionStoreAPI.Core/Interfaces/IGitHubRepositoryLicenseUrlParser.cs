namespace ExtensionStoreAPI.Core.Interfaces;

public interface IGitHubRepositoryLicenseUrlParser
{
    public string Parse(string repositoryUrl, string mainBranch);
}