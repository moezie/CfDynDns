using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfDynDns.Cloudflare.DTO
{
    internal class DnsRecord
    {
        public string id { get; set; }
        public string zone_id { get; set; }
        public string zone_name { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public bool proxiable { get; set; }
        public bool proxied { get; set; }
        public int ttl { get; set; }
        public bool locked { get; set; }
        public DnsMeta meta { get; set; }
        public DateTime created_on { get; set; }
        public DateTime modified_on { get; set; }
    }

    public class DnsMeta
    {
        public bool auto_added { get; set; }
        public bool managed_by_apps { get; set; }
        public bool managed_by_argo_tunnel { get; set; }
        public string source { get; set; }
    }

}
