using GithubIntegrationService.Services;
using Microsoft.AspNetCore.Mvc;
using GithubIntegrationService;
using GithubIntegrationService.Models;





namespace CvPortfolio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubService _service;

        public PortfolioController(IGitHubService service)
        {
            _service = service;
        }

        // GET: api/portfolio/mine
        [HttpGet("mine")]
        public async Task<IActionResult> GetPortfolio()
        {
            var data = await _service?.GetPortfolioAsync();
            return Ok(data);
        }

        // GET: api/portfolio/search?name=xxx&language=CSharp&user=someone
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            string? name,
            string? language,
            string? user)
        {
            var results = await _service?.SearchAsync(name, language, user);
            return Ok(results);
        }
    }
}
