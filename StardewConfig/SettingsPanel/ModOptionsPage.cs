using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;


namespace StardewConfigMenu
{
    public class ModOptionsPage: IClickableMenu
	{
		private ModSettings controller;
        private List<ModSheet> Pages = new List<ModSheet>();
        private DropDownComponent modSelected;

		internal ModOptionsPage(ModSettings controller, int x, int y, int width, int height) : base (x, y, width, height, false)
		{
            this.controller = controller;
            modSelected = new DropDownComponent("", (int)Game1.smallFont.MeasureString("Stardew Configuration Menu Framework").X, (int)(this.xPositionOnScreen + Game1.pixelZoom * 15), (int)(this.yPositionOnScreen + Game1.pixelZoom * 30));
            AddListeners();
			ReloadMenu();
        }

        static public void SetActive()
        {
            var gameMenu = (GameMenu)Game1.activeClickableMenu;
            gameMenu.currentTab = 8;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        internal void AddListeners()
        {
            controller.ModAdded += ReloadMenu;
            RemoveListeners();
        }

        internal void RemoveListeners(bool children = false)
        {
            if (children)
            {
                modSelected.RemoveListeners();
            }

            controller.ModAdded -= ReloadMenu;
        }

        private void ReloadMenu() {
            // Reset Menu and pages
            this.Pages.Clear();
            this.modSelected.ClearOptions();

            for (int i = 0; i < this.controller.ModOptionsList.Count; i++) {
                
                // Create mod page and add it
				this.Pages.Add(new ModSheet(this.controller.ModOptionsList[i], (int)(this.xPositionOnScreen + Game1.pixelZoom * 15), (int)(this.yPositionOnScreen + Game1.pixelZoom * 55), this.width - (Game1.pixelZoom * 15), this.height - Game1.pixelZoom * 65));

                // Add names to mod selector dropdown
                this.modSelected.AddOption(this.controller.ModOptionsList[i].modManifest.Name);
            }

        }

        public override void draw(SpriteBatch b)
        {
            //base.draw(b);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);

            base.drawHorizontalPartition(b, (int)(this.yPositionOnScreen + Game1.pixelZoom * 40));

            //Game1.draw

            if (Pages.Count > 0)
                Pages[modSelected.selectedOption].draw(b);

            // draw mod select dropdown last, should cover mod settings
            modSelected.draw(b);
            SpriteText.drawString(b, "Mod Options", modSelected.X + modSelected.Width + Game1.pixelZoom * 5, modSelected.Y);

        }

    }
}
