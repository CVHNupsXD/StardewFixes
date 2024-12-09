using StardewModdingAPI;
using System.Text;

namespace StardewFixes.Utilities
{
    public static class DiscordWebhook
    {
        private static ModConfig _config;
        private static IMonitor _monitor;

        private static readonly HttpClient _httpClient = new HttpClient();

        public static async void SendEmbed(string description, int color)
        {
            _config = new ModConfig();

            if (string.IsNullOrEmpty(_config.WebHook))
                throw new InvalidOperationException("Webhook URL is not configured.");

            var embed = new
            {
                username = _config.WebHook_Username,
                avatar_url = _config.WebHook_Avatar,

                embeds = new[]
                {
                    new
                    {
                        description,
                        color,
                        timestamp = DateTime.UtcNow.ToString("o"),
                    }
                }
            };

            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(embed);

            try
            {
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(_config.WebHook, content);

                if (!response.IsSuccessStatusCode)
                {
                    _monitor.Log($"Failed to send webhook: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _monitor.Log($"Error sending webhook: {ex.Message}");
            }
        }
    }
}
