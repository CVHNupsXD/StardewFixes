using System.Net;

namespace StardewFixes.HttpServer
{
    public class HttpServer
    {
        private readonly HttpListener _httpListener;
        private readonly ModConfig _config;

        public DateTime _startTime;

        public HttpServer(ModConfig config)
        {
            _config = config;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(_config.ApiUrl);
        }

        public void Start()
        {
            _startTime = DateTime.Now;

            _httpListener.Start();
            Task.Run(() => Listen());
        }

        private async Task Listen()
        {
            while (_httpListener.IsListening)
            {
                var context = await _httpListener.GetContextAsync();
                RequestProcessor.ProcessRequest(context, _startTime);
            }
        }
        public void Stop()
        {
            _httpListener.Stop();
        }
    }
}