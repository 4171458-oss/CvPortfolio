using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubIntegrationService.Models
{
    public class RepoDto
    {
        public string Name { get; set; }
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
        public DateTime? LastCommit { get; set; }
        public IEnumerable<string> Languages { get; set; }
    }
}

