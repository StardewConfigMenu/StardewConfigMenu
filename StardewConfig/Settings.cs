using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using StardewConfigFramework;
using StardewConfigMenu.Panel;

namespace StardewConfigMenu
{
	internal delegate void ModAddedSettings();

	public class ModSettings: IModSettingsFramework
    {
        public static int? pageIndex = null;
        internal event ModAddedSettings ModAdded;

        internal ModSettings(ModEntry mod)
        {
            ModSettings.Mod = mod;
            ModSettings.Instance = this;

			MenuEvents.MenuChanged += MenuOpened;
			MenuEvents.MenuClosed += MenuClosed;
		}

        static internal ModEntry Mod;

        static internal void Log(string str, LogLevel level = LogLevel.Debug) {
            Mod.Monitor.Log(str, level);
        }

		//internal SettingsPage page;
		internal ModOptionsTab tab;
        internal ModOptionsPage page;

		internal List<ModOptions> ModOptionsList = new List<ModOptions>();

        public override void AddModOptions(ModOptions modOptions) {
            // Only one per mod, remove old one
            foreach (ModOptions mod in this.ModOptionsList) {
                if (mod.modManifest.Name == modOptions.modManifest.Name) {
                    ModOptionsList.Remove(mod);
                }
            }

            Mod.Monitor.Log($"{modOptions.modManifest.Name} has been added their mod options");

            ModOptionsList.Add(modOptions);
            this.ModAdded?.Invoke();
        }

		/// <summary>
		/// Removes the delegates that handle the button click and draw method of the tab
		/// </summary>
		private void MenuClosed(object sender, EventArgsClickableMenuClosed e)
		{
            GraphicsEvents.OnPostRenderGuiEvent -= RenderTab;

			if (this.tab != null) {
				this.tab.RemoveListeners();
				this.tab = null;
            }

            if (this.page != null) {
                if (e.PriorMenu is GameMenu) {
                    List<IClickableMenu> pages = ModEntry.helper.Reflection.GetPrivateField<List<IClickableMenu>>((e.PriorMenu as GameMenu), "pages").GetValue();
                    pages.Remove(this.page);
                }

                this.page.RemoveListeners(true);
                this.page = null;
                ModSettings.pageIndex = null;
            }
        }

		/// <summary>
		/// Attaches the delegates that handle the button click and draw method of the tab
        /// </summary>
		private void MenuOpened(object sender, EventArgsClickableMenuChanged e)
		{
            if (!(e.NewMenu is GameMenu)) {
                this.tab = null;
                this.page = null;
                ModSettings.pageIndex = null;
                return; 
            }

            GameMenu menu = (GameMenu) e.NewMenu;
            List<IClickableMenu> pages = ModEntry.helper.Reflection.GetPrivateField<List<IClickableMenu>>(menu, "pages").GetValue();

            var options = pages.Find(x => { return x is OptionsPage; });
            int width = options.width;
            
            //List<ClickableComponent> tabs = ModEntry.helper.Reflection.GetPrivateField<List<ClickableComponent>>(menu, "tabs").GetValue();

            this.page = new ModOptionsPage(this, menu.xPositionOnScreen, menu.yPositionOnScreen, width, menu.height);
            ModSettings.pageIndex = pages.Count;
            pages.Add(page);

            this.tab = new ModOptionsTab(this, new Rectangle(menu.xPositionOnScreen + Game1.tileSize * 11, menu.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize));

			GraphicsEvents.OnPostRenderGuiEvent -= RenderTab;
			GraphicsEvents.OnPostRenderGuiEvent += RenderTab;

		}

        private void RenderTab(object sender, EventArgs e) {

            if (!(Game1.activeClickableMenu is GameMenu))
                return;

            var gameMenu = (GameMenu)Game1.activeClickableMenu;

            if (gameMenu.currentTab == GameMenu.mapTab) { return; }

            if (tab != null)
                tab.draw(Game1.spriteBatch);

            //var b = Game1.spriteBatch;
			//this.tab.draw(b);
		}
    }
}
