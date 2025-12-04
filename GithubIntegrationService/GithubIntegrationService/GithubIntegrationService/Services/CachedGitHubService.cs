using GithubIntegrationService.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GithubIntegrationService.Services
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _inner;
        private readonly IMemoryCache _cache;

        public CachedGitHubService(IGitHubService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<IEnumerable<RepoDto>> GetPortfolioAsync()
        {
            if (_cache.TryGetValue("portfolio", out IEnumerable<RepoDto> cached))
                return cached;

            var data = await _inner.GetPortfolioAsync();

            _cache.Set("portfolio", data, TimeSpan.FromMinutes(5));

            return data;
        }

        public async Task<IEnumerable<RepoDto>> SearchAsync(string? name, string? language, string? user)
        {
            string key = $"search-{name}-{language}-{user}";

            if (_cache.TryGetValue(key, out IEnumerable<RepoDto> cached))
                return cached;

            var data = await _inner.SearchAsync(name, language, user);

            _cache.Set(key, data, TimeSpan.FromMinutes(5));

            return data;
        }
    }
}
