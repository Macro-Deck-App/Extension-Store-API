using MacroDeckExtensionStoreLibrary.Models;

namespace MacroDeckExtensionStoreLibrary.Interfaces;

public interface IGitHubRepositoryService
{
    public Task<string> GetDefaultBranchName(string repositoryUrl);
    public Task<string> GetReadmeAsync(string repositoryUrl);
    public Task<GitHubLicense> GetLicenseAsync(string repositoryUrl);
}