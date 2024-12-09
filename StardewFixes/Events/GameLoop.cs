using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewModdingAPI;
using StardewFixes.Utilities;
using StardewValley.Menus;

namespace StardewFixes.GameHandlers
{
    public static class GameLoop
    {
        private static IMonitor _monitor;
        public static void OnTimeChanged(object sender, System.EventArgs e)
        {
            int currentTime = Game1.timeOfDay;

            if (currentTime == 650) //Always On Server = 640.
            {
                if (Game1.currentLocation != null && Game1.currentLocation is Farm)
                {
                    Farmer Host = Game1.MasterPlayer;

                    var farm = Game1.getLocationFromName("Farm") as Farm;
                    var warp = new Warp(64, 15, farm.NameOrUniqueName, 64, 10, false);

                    Host.warpFarmer(warp);
                }
            }
        }

        public static void OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (Context.IsWorldReady)
            {
                Game1.player.temporarilyInvincible = true;
                Game1.player.temporaryInvincibilityTimer = -999;

                if (Game1.activeClickableMenu is ItemGrabMenu iMenu)
                {
                    var count = iMenu.ItemsToGrabMenu.actualInventory.Count();

                    for (int i = Game1.player.Items.Count - 1; i >= 0; i--)
                    {
                        var item = Game1.player.Items[i];
                        if (null == item)
                        {
                            count--;
                        }
                        else
                        {
                            if (item.canBeTrashed())
                            {
                                Game1.player.removeItemFromInventory(item);
                                count--;
                            }
                        }
                    }
                }

                if (Game1.activeClickableMenu is not LevelUpMenu lMenu)
                {
                    return;
                }
                else
                {
                    lMenu.isActive = false;
                    lMenu.informationUp = false;
                    lMenu.isProfessionChooser = false;
                    lMenu.RemoveLevelFromLevelList();
                }

                var owner = ((Cabin)Game1.getFarm().buildings.First(building => building.isCabin).indoors.Value).owner;

                if (owner.HouseUpgradeLevel != Game1.player.HouseUpgradeLevel)
                {
                    Game1.player.HouseUpgradeLevel = owner.HouseUpgradeLevel;
                }
            }
        }

        public static void OnDayStarted(object sender, DayStartedEventArgs e)
        {

        }

        public static void OnTick(object sender, System.EventArgs e)
        {

        } 
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.options.setMoveBuildingPermissions("on");
        }

        public static void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Game1.options.fullscreen = false;
            Game1.options.pauseWhenOutOfFocus = false;
        }

        public static void OnPeerConnected(object sender, PeerConnectedEventArgs e)
        {
            string description = "";
            Farmer player = Game1.GetPlayer(e.Peer.PlayerID, true);

            if (player.Name == "")
            {
                description = $"New player has joined the game!";
            } else
            {
                description = $"{player.Name} has joined the game!";
            }

            DiscordWebhook.SendEmbed(description, 3066993);
        }

        public static void OnPeerDisconnected(object sender, PeerDisconnectedEventArgs e)
        {
            Farmer player = Game1.GetPlayer(e.Peer.PlayerID, true);

            string description = $"{player.Name} has left the game.";
            DiscordWebhook.SendEmbed(description, 15158332);
        }
    }
}
