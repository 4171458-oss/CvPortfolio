using GithubIntegrationService.Models;
using Octokit;

namespace GithubIntegrationService.Services
{
    public interface IGitHubService
    {
        Task<IEnumerable<RepoDto>> GetPortfolioAsync();
        Task<IEnumerable<RepoDto>> SearchAsync(string? name, string? language, string? user);

        Task<DateTime?> GetLastUserActivityAsync();
    }

}
