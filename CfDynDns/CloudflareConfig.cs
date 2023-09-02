using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfDynDns
{
    public class CloudflareConfig
    {
        public const string Cloudflare = "Cloudflare";
        public string Url { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public List<string> Records { get; set; } = new List<string>();

        public List<string> GetDomains()
        {
            var hosts = Records.Select(x => x.Substring(x.LastIndexOf('.', x.LastIndexOf('.') - 1) + 1));

            return hosts.Distinct().ToList();
        }

        public List<string> GetRecordsForDomain(string domain)
        {
            return Records.Where(x => x.EndsWith(domain, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
    }
}
