using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

namespace StardewConfigFramework
{

	internal delegate void ModAddedSettings();

	public class Settings
    {
        void GraphicsEvents_OnPreRenderGuiEvent(object sender, EventArgs e)
        {

        }

        internal event ModAddedSettings ModAdded;

		public static Settings control;

        internal Settings(ModEntry mod)
        {
            Settings.Mod = mod;
            Settings.control = this;

			MenuEvents.MenuChanged += MenuOpened;
			MenuEvents.MenuClosed += MenuClosed;
		}

        static internal ModEntry Mod;

        static internal void Log(string str, LogLevel level = LogLevel.Debug) {
            Mod.Monitor.Log(str, level);
        }

		//internal SettingsPage page;
		internal SettingsTab tab;
        internal ModSettingsPage page;

		internal List<ModOptions> ModOptionsList = new List<ModOptions>();

        public void AddModOptions(ModOptions modOptions) {
            // Only one per mod, remove old one
            foreach (ModOptions mod in this.ModOptionsList) {
                if (mod.modName == modOptions.modName) {
                    ModOptionsList.Remove(mod);
                }
            }

            ModOptionsList.Add(modOptions);
            this.ModAdded();
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

                this.page.RemoveListeners();
                this.page = null;
			}			
		}

		/// <summary>
		/// Attaches the delegates that handle the button click and draw method of the tab
        /// </summary>
		private void MenuOpened(object sender, EventArgsClickableMenuChanged e)
		{
            Log("MenuOpened called");
            if (!(e.NewMenu is GameMenu)) {
                this.tab = null;
                return; 
            }

			Log("is GameMenu");

			GameMenu menu = (GameMenu) e.NewMenu;

            List<IClickableMenu> pages = ModEntry.helper.Reflection.GetPrivateField<List<IClickableMenu>>(menu, "pages").GetValue();
            //         List<ClickableComponent> tabs = ModEntry.helper.Reflection.GetPrivateField<List<ClickableComponent>>(menu, "tabs").GetValue();

            this.page = new ModSettingsPage(this, menu.xPositionOnScreen, menu.yPositionOnScreen, menu.width + ((LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.ru) ? (Game1.tileSize / 2) : (Game1.tileSize * 3 / 2)), menu.height);
            pages.Add(page);

            this.tab = new SettingsTab(this, new Rectangle(menu.xPositionOnScreen + Game1.tileSize * 11, menu.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "mods", "Mod Settings");



			GraphicsEvents.OnPostRenderGuiEvent -= RenderTab;
			GraphicsEvents.OnPostRenderGuiEvent += RenderTab;

		}

        private void RenderTab(object sender, EventArgs e) {

			var b = Game1.spriteBatch;
			this.tab.draw(b);
		}
    }
}
