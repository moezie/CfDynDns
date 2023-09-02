using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfDynDns.Cloudflare.DTO
{
    internal class Zone
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public bool paused { get; set; }
        public string type { get; set; }
        public int development_mode { get; set; }
        public string[] name_servers { get; set; }
        public string[] original_name_servers { get; set; }
        public string original_registrar { get; set; }
        public object original_dnshost { get; set; }
        public DateTime modified_on { get; set; }
        public DateTime created_on { get; set; }
        public DateTime activated_on { get; set; }
        public ZoneMeta meta { get; set; }
        public Owner owner { get; set; }
        public Account account { get; set; }
        public Tenant tenant { get; set; }
        public Tenant_Unit tenant_unit { get; set; }
        public string[] permissions { get; set; }
        public Plan plan { get; set; }
    }

    public class ZoneMeta
    {
        public int step { get; set; }
        public int custom_certificate_quota { get; set; }
        public int page_rule_quota { get; set; }
        public bool phishing_detected { get; set; }
        public bool multiple_railguns_allowed { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string type { get; set; }
        public string email { get; set; }
    }

    public class Account
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Tenant
    {
        public object id { get; set; }
        public object name { get; set; }
    }

    public class Tenant_Unit
    {
        public object id { get; set; }
    }

    public class Plan
    {
        public string id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string currency { get; set; }
        public string frequency { get; set; }
        public bool is_subscribed { get; set; }
        public bool can_subscribe { get; set; }
        public string legacy_id { get; set; }
        public bool legacy_discount { get; set; }
        public bool externally_managed { get; set; }
    }

}
