using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CfDynDns.Cloudflare
{
    internal static class CloudflareAPI
    {
        private static string _baseUrl = "https://api.cloudflare.com/client/v4/";
        private static string _token = null;

        public static void SetToken(string token)
        {
            _token = token;
        }

        public async static Task<List<DTO.Zone>> GetZonesAsync()
        {
            var data = await GetData("zones");

            return JArray.FromObject(data).ToObject<List<DTO.Zone>>() ?? new List<DTO.Zone>();
        }

        public async static Task<List<DTO.DnsRecord>> GetZoneDnsRecordAsync(string zoneId)
        {
            var data = await GetData($"zones/{zoneId}/dns_records");

            return JArray.FromObject(data).ToObject<List<DTO.DnsRecord>>() ?? new List<DTO.DnsRecord>();
        }

        public async static Task<HttpResponse> UpdateZoneDnsRecordAsync(string zoneId, string dnsId, string ip)
        {
            return await GetData($"zones/{zoneId}/dns_records/{dnsId}", new { content = ip }, HttpMethods.PATCH);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="bodyParams"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static async Task<HttpResponse> GetData(string endPoint, object bodyParams, HttpMethods method)
        {
            //Dictionary<string, string> bodyDict = new Dictionary<string, string>();

            //var properties = bodyParams.GetType().GetProperties();
            //foreach (var property in properties)
            //{
            //    if (property.GetValue(bodyParams) != null)
            //        bodyDict.Add(property.Name, property.GetValue(bodyParams).ToString());
            //}

            string bodyJson = JsonConvert.SerializeObject(bodyParams);

            return await GetData(endPoint, bodyJson, method);
        }

        /// <summary>
        /// Ophalen van data uit de Tuinconfigurator doormiddel van een GET request. De baseUrl hoeft hierbij niet opgegeven te worden.
        /// </summary>
        /// <param name="endPoint">Het endpoint/pad welke opgehaald moet worden. De eerste / hoeft niet opgegeven te worden. Query params mogen hierbij ook opgegeven worden.</param>
        /// <returns></returns>
        private static async Task<object> GetData(string endPoint)
        {
            var result = await GetData(endPoint, method: HttpMethods.GET);
            if (!result.Success)
                return null;

            return result.Result;
        }

        /// <summary>
        /// Interne functie voor het uitvoeren van de HTTP call. De baseUrl hoeft hierbij niet opgegeven te worden.
        /// </summary>
        /// <param name="endPoint">Het endpoint/pad welke opgehaald moet worden. De eerste / hoeft niet opgegeven te worden. Query params mogen hierbij ook opgegeven worden.</param>
        /// <param name="bodyParams">Eventuele parameters die in het geval van een POST, PUT, PATCH of DELETE meegegeven moeten worden.</param>
        /// <param name="method">De gewenste HTTP method</param>
        /// <returns></returns>
        private static async Task<HttpResponse> GetData(string endPoint, string bodyParams = null, HttpMethods method = HttpMethods.GET)
        {
            //string apikey = Settings.GetSetting(Settings.Setting.ScoretraceAPIKey);
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 3, 0);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = new HttpMethod(method.ToString());
            request.RequestUri = new Uri($"{_baseUrl}{endPoint}");
            if (!string.IsNullOrEmpty(bodyParams))
                request.Content = new StringContent(bodyParams, Encoding.UTF8, "application/json");

            //var responseMessage = await client.GetAsync($"{baseUrl}{endPoint}");
            var responseMessage = await client.SendAsync(request);
            try
            {
                return JsonConvert.DeserializeObject<HttpResponse>(await responseMessage.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }

        }

        private enum HttpMethods
        {
            GET,
            POST,
            PUT,
            DELETE,
            HEAD,
            OPTIONS,
            PATCH
        }
    }
}
