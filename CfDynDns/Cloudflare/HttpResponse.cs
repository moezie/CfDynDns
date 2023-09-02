using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CfDynDns.Cloudflare
{
    internal class HttpResponse
    {
        public bool Success { get; set; }
        public List<CloudflareError> Errors { get; set; }
        public List<string> Messages { get; set; }
        public object Result { get; set; }
        public PageInfo ResultInfo { get; set; }
    }

}
