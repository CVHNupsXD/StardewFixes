using StardewModdingAPI;

namespace StardewFixes
{
    public class ModEntry : Mod
    {
        private HttpServer.HttpServer _httpServer;

        public override void Entry(IModHelper helper)
        {

            _httpServer = new HttpServer.HttpServer(helper.ReadConfig<ModConfig>());
            _httpServer.Start();

            helper.Events.GameLoop.DayStarted += GameHandlers.GameLoop.OnTick;
            helper.Events.GameLoop.SaveLoaded += GameHandlers.GameLoop.OnSaveLoaded;
            helper.Events.GameLoop.TimeChanged += GameHandlers.GameLoop.OnTimeChanged;
            helper.Events.GameLoop.DayStarted += GameHandlers.GameLoop.OnDayStarted;
            helper.Events.GameLoop.GameLaunched += GameHandlers.GameLoop.OnGameLaunched;
            helper.Events.GameLoop.OneSecondUpdateTicked -= GameHandlers.GameLoop.OneSecondUpdateTicked;

            helper.Events.Multiplayer.PeerConnected += GameHandlers.GameLoop.OnPeerConnected;
            helper.Events.Multiplayer.PeerDisconnected += GameHandlers.GameLoop.OnPeerDisconnected;
        }

        
    }
}
