using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Parsers;
using MacroDeckExtensionStoreLibrary.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MacroDeckExtensionStoreTests;

public class GitHubServiceTest
{
    private GitHubRepositoryLicenseUrlParser _gitHubRepositoryLicenseUrlParser;
    private IGitHubRepositoryService _repositoryService;
    
    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        ILogger<GitHubRepositoryService> logger = new NullLogger<GitHubRepositoryService>();
        _gitHubRepositoryLicenseUrlParser = new GitHubRepositoryLicenseUrlParser();
        _repositoryService = new GitHubRepositoryService(logger, _gitHubRepositoryLicenseUrlParser, httpClient);
    }

    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    public void ParserTest(string repositoryUrl)
    {
        var parsed = _gitHubRepositoryLicenseUrlParser.Parse(repositoryUrl, "main");
        Assert.That(parsed, Is.EqualTo("https://raw.githubusercontent.com/SuchByte/Macro-Deck-Twitch-Plugin/main/LICENSE"));
    }

    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    [TestCase("https://github.com/SuchByte/Macro-Deck-Windows-Utils-Plugin")]
    public void LicenseTest(string repositoryUrl)
    {
        var license = _repositoryService.GetLicenseAsync(repositoryUrl).Result;
        Assert.Multiple(() =>
        {
            Assert.That(license.Name, Is.EqualTo("MIT License"));
            Assert.That(license.Url, Is.EqualTo("https://api.github.com/licenses/mit"));
        });
    }

    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    public void DefaultBranchTest(string repositoryUrl)
    {
        var defaultBranch = _repositoryService.GetDefaultBranchName(repositoryUrl).Result;
        Assert.That(defaultBranch, Is.EqualTo("main"));
    }
    
    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    public void ReadMeTest(string repositoryUrl)
    {
        var readMe = _repositoryService.GetReadmeAsync(repositoryUrl).Result;
        Console.WriteLine(readMe);
    }
}