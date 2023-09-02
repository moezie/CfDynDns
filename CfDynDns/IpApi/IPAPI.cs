using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CfDynDns.IpApi
{
    internal class IPAPI
    {
        private static IPAPI ipApi;
        private readonly IConfiguration _config;
        private readonly string _ipEndpoint = null;

        public IPAPI(IConfiguration config)
        {
            _config = config;
            _ipEndpoint = config["IpApi:UrlIpApi"];
            if (!Uri.IsWellFormedUriString(_ipEndpoint, UriKind.Absolute))
                throw new Exception($"{_ipEndpoint} is an invalid URL. Please use a valid URL.");
        }

        public async Task<string> GetCurrentIpAddress()
        {
            using(HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(_ipEndpoint);
            }
        }
    }
}
