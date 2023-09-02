using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace CfDynDns.Cloudflare
{
    public class PageInfo
    {
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "per_page")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }
    }
}