using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Services;
using Moq;

namespace MacroDeckExtensionStoreTests;

public class GitHubServiceTest
{
    private readonly IGitHubRepositoryService _repositoryService;

    public GitHubServiceTest()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var httpClientFactory = mockFactory.Object;
        _repositoryService = new GitHubRepositoryService(httpClientFactory);
    }

    [Test]
    public async Task DescriptionTest()
    {
        const string repositoryUrl = "https://github.com/Macro-Deck-org/Macro-Deck";
        var description = await _repositoryService.GetDescriptionAsync(repositoryUrl);
        Assert.That(description, Is.EqualTo("Macro Deck converts your phone, tablet or any other device with an up-to-date internet browser into an powerful remote macro pad to perform single actions or even multiple actions with just one tap."));
    }

    [TestCase("https://github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("https://www.github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("www.github.com/Macro-Deck-org/Macro-Deck")]
    [TestCase("github.com/Macro-Deck-org/Macro-Deck")]
    public void TestGetRepositoryNameFromUrl(string url)
    {
        var repositoryName = GitHubRepositoryService.GetRepositoryNameFromUrl(url);
        Assert.That(repositoryName, Is.EqualTo("Macro-Deck-org/Macro-Deck"));
    }

    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    [TestCase("https://github.com/SuchByte/Macro-Deck-Windows-Utils-Plugin")]
    public void LicenseTest(string? repositoryUrl)
    {
        var license = _repositoryService.GetLicenseAsync(repositoryUrl).Result;
        Assert.Multiple(() =>
        {
            Assert.That(license.Name, Is.EqualTo("MIT License"));
            Assert.That(license.Url, Is.EqualTo("https://api.github.com/licenses/mit"));
        });
    }

    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    public void DefaultBranchTest(string? repositoryUrl)
    {
        var defaultBranch = _repositoryService.GetDefaultBranchName(repositoryUrl).Result;
        Assert.That(defaultBranch, Is.EqualTo("main"));
    }
    
    [TestCase("https://github.com/SuchByte/Macro-Deck-Twitch-Plugin")]
    public void ReadMeTest(string? repositoryUrl)
    {
        var readMe = _repositoryService.GetReadmeAsync(repositoryUrl).Result;
        Console.WriteLine(readMe);
    }
}