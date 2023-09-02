using CfDynDns.Cloudflare;
using CfDynDns.IpApi;
using Microsoft.Extensions.Options;

namespace CfDynDns
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly CloudflareConfig _cloudflareConfig;
        private readonly TelegramApi _telegramApi;
        private int _pollFreq = 10;

        public Worker(ILoggerFactory logger, IConfiguration config, IOptions<CloudflareConfig> cloudflareConfig)
        {
            _logger = logger.CreateLogger<Worker>();
            _config = config;
            _cloudflareConfig = cloudflareConfig.Value;

            if (string.IsNullOrEmpty(_cloudflareConfig.Token))
            {
                _logger.LogCritical("No Cloudflare token set. Please configure the Cloudflare API Token in the appsettings.json.");
                Environment.Exit(22);
                return;
            }
            CloudflareAPI.SetToken(_cloudflareConfig.Token);

            _telegramApi = new TelegramApi(config, logger.CreateLogger<TelegramApi>());

            int.TryParse(_config["IpApi:PollFreq"], out _pollFreq);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Polling frequency set to {_pollFreq} minutes.");
            TimeSpan tsFreq = TimeSpan.FromMinutes(_pollFreq);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Start IP check");
                IPAPI ipApi = new IPAPI(_config);
                try
                {
                    var currentIp = ipApi.GetCurrentIpAddress();
                    var zones = CloudflareAPI.GetZonesAsync();
                    var domains = _cloudflareConfig.GetDomains();
                    _logger.LogInformation($"Current ip: {await currentIp}");
                    foreach (var domain in domains)
                    {
                        var zone = (await zones).FirstOrDefault(x => x.name == domain);
                        if (zone == null)
                        {
                            _logger.LogWarning($"No zone information found for domain {domain}. Please check if the API Token has access to this zone in Cloudflare.");
                            continue;
                        }

                        var records = _cloudflareConfig.GetRecordsForDomain(domain);
                        var dnsList = await CloudflareAPI.GetZoneDnsRecordAsync(zone.id);

                        foreach (var record in records)
                        {
                            var dnsRecord = dnsList.FirstOrDefault(x => x.name == record);
                            if (dnsRecord == null)
                            {
                                _logger.LogWarning($"No dns record found for {record} in zone {domain}. Please check if the record exists in Cloudflare DNS.");
                                continue;
                            }

                            if (dnsRecord.type != "A")
                            {
                                _logger.LogWarning($"The dns record {record} is a {dnsRecord.type}-type record. Only A-type records are supported at the moment.");
                                continue;
                            }

                            if (dnsRecord.content != await currentIp)
                            {
                                _logger.LogInformation($"The dns record {record} is pointing at {dnsRecord.content} and our current IP is {await currentIp}. Let's change it...");
                                var updateResult = await CloudflareAPI.UpdateZoneDnsRecordAsync(zone.id, dnsRecord.id, await currentIp);
                                if (updateResult.Success)
                                {
                                    _logger.LogInformation("Done!");
                                    _telegramApi.SendMessage($"The dns record {record} is pointing at {dnsRecord.content} and our current IP is {await currentIp}. The record has been changed.");
                                }
                                else
                                {
                                    _logger.LogError($"Could not update the record {record}. Cloudflare reports the following error(s): ", updateResult.Errors);
                                    _telegramApi.SendMessage($"Could not update the record {record} to point to {await currentIp}. Please check the logs of CfDynDns for more information.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(exception: ex, "Error while retrieving the current IP address.");
                }
                _logger.LogInformation($"Next check at {DateTime.Now.Add(tsFreq)}");
                await Task.Delay((int)tsFreq.TotalMilliseconds, stoppingToken);
            }
        }
    }
}