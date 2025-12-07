using GithubIntegrationService.Models;
using GithubIntegrationService.Options;
using Microsoft.Extensions.Options;
using Octokit;

namespace GithubIntegrationService.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubOptions _options;

        public GitHubService(IOptions<GitHubOptions> options)
        {
            _options = options.Value;

            _client = new GitHubClient(new ProductHeaderValue("CvPortfolioApp"))
            {
                Credentials = new Credentials(_options.Token)
            };
        }
        public async Task<DateTime?> GetLastUserActivityAsync()
        {
            var events = await _client.Activity.Events.GetAllUserPerformed(_options.UserName);

            return events.FirstOrDefault()?.CreatedAt.UtcDateTime;
        }

        public async Task<IEnumerable<RepoDto>> GetPortfolioAsync()
        {
            var repos = await _client.Repository.GetAllForUser(_options.UserName);

            var list = new List<RepoDto>();

            foreach (var repo in repos)
            {
                IReadOnlyList<GitHubCommit>? commits = null;

                try
                {
                    
                    commits = await _client.Repository.Commit.GetAll(repo.Owner.Login, repo.Name);
                }
                catch (Octokit.ApiException ex) when (ex.Message.Contains("Git Repository is empty"))
                {
                    // מתעלמים וממשיכים — commits יישאר null
                }
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
                var pulls = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);

                list.Add(new RepoDto
                {
                    Name = repo.Name,
                    HtmlUrl = repo.HtmlUrl,
                    Description = repo.Description,
                    Stars = repo.StargazersCount,
                    PullRequests = pulls.Count,
                    LastCommit = commits?.FirstOrDefault()?.Commit.Author.Date.UtcDateTime,
                    Languages = languages.Select(l => l.Name).ToList()
                });
            }

            return list;
        }


        public async Task<IEnumerable<RepoDto>> SearchAsync(string? name, string? language, string? user)
        {
            var repos = await _client.Repository.GetAllForUser(user);

            if (!string.IsNullOrWhiteSpace(name))
                repos = repos.Where(r => r.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(language))
            {
                repos = repos.Where(r =>
                {
                    var langs = _client.Repository.GetAllLanguages(r.Owner.Login, r.Name).Result;
                    return langs.Any(l => l.Name.Contains(language, StringComparison.OrdinalIgnoreCase)
);
                }).ToList();
            }

            return repos.Select(r => new RepoDto
            {
                Name = r.Name,
                HtmlUrl = r.HtmlUrl,
                Description = r.Description,
                Stars = r.StargazersCount
            });
        }
    }
}
