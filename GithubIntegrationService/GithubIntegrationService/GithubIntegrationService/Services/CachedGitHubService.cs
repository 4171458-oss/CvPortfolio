using GithubIntegrationService.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GithubIntegrationService.Services
{
   
        public class CachedGitHubService : IGitHubService
        {
            private readonly IGitHubService _inner;
            private readonly IMemoryCache _cache;

            private const string PortfolioKey = "portfolio";
            private const string PortfolioActivityKey = "portfolio-last-activity";

            public CachedGitHubService(IGitHubService inner, IMemoryCache cache)
            {
                _inner = inner;
                _cache = cache;
            }

            public async Task<IEnumerable<RepoDto>> GetPortfolioAsync()
            {
                var lastActivity = await _inner.GetLastUserActivityAsync();

              
                if (_cache.TryGetValue(PortfolioKey, out IEnumerable<RepoDto> cachedData) &&
                    _cache.TryGetValue(PortfolioActivityKey, out DateTime cachedActivity))
                {
                
                    if (lastActivity != null && lastActivity <= cachedActivity)
                    {
                        return cachedData;
                    }
                }

            
                var freshData = await _inner.GetPortfolioAsync();

                _cache.Set(PortfolioKey, freshData);
                _cache.Set(PortfolioActivityKey, lastActivity ?? DateTime.UtcNow);

                return freshData;
            }

            public async Task<IEnumerable<RepoDto>> SearchAsync(string? name, string? language, string? user)
            {
                string key = $"search-{name}-{language}-{user}";

                if (_cache.TryGetValue(key, out IEnumerable<RepoDto> cached))
                    return cached;

                var data = await _inner.SearchAsync(name, language, user);

                _cache.Set(key, data);

                return data;
            }

            public Task<DateTime?> GetLastUserActivityAsync()
            {
             
                return _inner.GetLastUserActivityAsync();
            }
        }
    }


