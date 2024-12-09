using System.Net;
using System.Text;
using StardewFixes.Utilities;
using StardewModdingAPI;
using StardewValley;

namespace StardewFixes.HttpServer
{
    public static class RequestProcessor
    {
        public static void ProcessRequest(HttpListenerContext context, DateTime startTime)
        {
            string action = context.Request.QueryString["action"];

            if (string.IsNullOrEmpty(action))
            {
                WriteErrorResponse(context, 400, "Bad Request", "Missing required 'action' parameter.");
                return;
            }

            switch (action.ToLowerInvariant())
            {
                case "queryinfo":
                    HandleStatus(context, startTime);
                    break;
                case "newcode":
                    HandleNewInviteCode(context);
                    break;
                default:
                    WriteErrorResponse(context, 400, "Bad Request", $"Unknown action '{action}'.");
                    break;
            }
        }
        private static void HandleStatus(HttpListenerContext context, DateTime startTime)
        {
            bool isWorldReady = Context.IsWorldReady;

            var responseData = new
            {
                IsWorldReady = isWorldReady,
                Farm = isWorldReady ? new
                {
                    FarmName = Game1.player.farmName.Value,
                    Date = $"{Game1.dayOfMonth}/{Game1.currentSeason}/{Game1.year}",
                    DayOfWeek = GameTime.GetDayOfWeek(Game1.dayOfMonth, Game1.currentSeason),
                    Time = GameTime.ConvertGameTimeToString(Game1.timeOfDay),
                    Uptime = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss"),
                    Players = $"{Game1.getOnlineFarmers().Count}/8",
                    InviteCode = Game1.server?.getInviteCode()
                } : null,
                System = isWorldReady ? new
                {
                    CpuUsage = $"{Performance.GetCpuUsage():F2}%",
                    MemoryUsage = $"{Performance.GetMemoryUsage():F2} MB"
                } : null
            };
            WriteJsonResponse(context, responseData, isWorldReady ? 200 : 503);
        }
        private static void HandleNewInviteCode(HttpListenerContext context)
        {
            try
            {
                Game1.Multiplayer.Disconnect(Multiplayer.DisconnectType.ServerOfflineMode);

                WriteJsonResponse(context, new { StatusCode = 200, Message = $"{Game1.server?.getInviteCode()}" }, 200);
            }
            catch (Exception ex)
            {
                WriteErrorResponse(context, 500, "Failed to restart server.", ex.Message);
            }
        }
        public static void WriteJsonResponse(HttpListenerContext context, object data, int statusCode)
        {
            string jsonResponse = System.Text.Json.JsonSerializer.Serialize(data);
            byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse);

            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = responseBytes.Length;
            context.Response.StatusCode = statusCode;

            using (var output = context.Response.OutputStream)
            {
                output.Write(responseBytes, 0, responseBytes.Length);
            }
        }
        private static void WriteErrorResponse(HttpListenerContext context, int statusCode, string errorMessage, string details = null)
        {
            var errorResponse = new
            {
                StatusCode = statusCode,
                Message = errorMessage,
                Error = details
            };

            WriteJsonResponse(context, errorResponse, statusCode);
        }
    }
}