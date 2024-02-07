using ExtensionStoreAPI.Core.Interfaces;
using ExtensionStoreAPI.Core.Services;
using NSubstitute;
using NUnit.Framework;

namespace ExtensionStoreAPI.Test.UnitTests;

public class GitHubServiceTest
{
    private readonly IGitHubRepositoryService _repositoryService;

    public GitHubServiceTest()
    {
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
        httpClientFactoryMock.CreateClient(Arg.Any<string>()).Returns(new HttpClient());
        _repositoryService = new GitHubRepositoryService(httpClientFactoryMock);
    }
    
    [TestCase("github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("www.github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("https://www.github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("http://github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("https://github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("github.com/Macro-Deck-org/Macro-Deck.git")]
    [TestCase("www.github.com/Macro-Deck-org/Macro-Deck.git")]
    [TestCase("https://www.github.com/Macro-Deck-org/Macro-Deck.git")]
    [TestCase("http://github.com/Macro-Deck-org/Macro-Deck.git")]
    [TestCase("https://github.com/Macro-Deck-org/Macro-Deck.git")]
    [TestCase("github.com/Macro-Deck-org/Macro-Deck/unnecessary_parameter")]
    [TestCase("www.github.com/Macro-Deck-org/Macro-Deck/unnecessary_parameter")]
    [TestCase("https://www.github.com/Macro-Deck-org/Macro-Deck/unnecessary_parameter")]
    [TestCase("http://github.com/Macro-Deck-org/Macro-Deck/unnecessary_parameter")]
    [TestCase("https://github.com/Macro-Deck-org/Macro-Deck/unnecessary_parameter")]
    [TestCase("github.com/Macro-Deck-org/Macro-Deck.git/unnecessary_parameter")]
    [TestCase("www.github.com/Macro-Deck-org/Macro-Deck.git/unnecessary_parameter")]
    [TestCase("https://www.github.com/Macro-Deck-org/Macro-Deck.git/unnecessary_parameter")]
    [TestCase("http://github.com/Macro-Deck-org/Macro-Deck.git/unnecessary_parameter")]
    [TestCase("https://github.com/Macro-Deck-org/Macro-Deck.git/unnecessary_parameter")]
    public void TestGetRepositoryNameFromUrl(string url)
    {
        var repositoryName = GitHubRepositoryService.GetRepositoryNameFromUrl(url);
        Assert.That(repositoryName, Is.EqualTo("Macro-Deck-org/Macro-Deck"));
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