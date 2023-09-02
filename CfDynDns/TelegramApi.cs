using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CfDynDns
{
    internal class TelegramApi
    {
        private readonly ILogger<TelegramApi> _logger;
        private readonly bool useTelegram = false;
        private readonly string apiToken;
        private readonly int? chatId;
        private readonly string baseUrl = "https://api.telegram.org/bot";

        public TelegramApi(IConfiguration config, ILogger<TelegramApi> logger)
        {
            _logger = logger;
            apiToken = config.GetValue<string>("Telegram:ApiToken");
            chatId = config.GetValue<int?>("Telegram:ChatId");
            useTelegram = !string.IsNullOrEmpty(apiToken) && chatId.HasValue && chatId.Value > 0;
        }

        public async void SendMessage(string message)
        {
            if (useTelegram)
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{baseUrl}{apiToken}/sendMessage?chat_id={chatId}&parse_mode=markdown&text={Uri.EscapeDataString(message)}";
                    var result = await client.GetAsync(url);
                    if (!result.IsSuccessStatusCode)
                    {
                        _logger.LogWarning($"Could not send Telegram notification. Error: {result.StatusCode} - {result.ReasonPhrase}");
                    }
                }
            }
        }
    }
}
