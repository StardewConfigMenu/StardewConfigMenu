using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewConfigMenu.Panel.Components;

namespace StardewConfigMenu.Panel
{
    public class ModOptionsPage: IClickableMenu
	{

		private ModSettings controller;
        private List<ModSheet> Sheets = new List<ModSheet>();
        private DropDownComponent modSelected;

		internal ModOptionsPage(ModSettings controller, int x, int y, int width, int height) : base (x, y, width, height, false)
		{
            this.controller = controller;

            var modChoices = new List<string>();

			for (int i = 0; i < this.controller.ModOptionsList.Count; i++)
			{

				// Create mod page and add it
				this.Sheets.Add(new ModSheet(this.controller.ModOptionsList[i], (int)(this.xPositionOnScreen + Game1.pixelZoom * 15), (int)(this.yPositionOnScreen + Game1.pixelZoom * 55), this.width - (Game1.pixelZoom * 15), this.height - Game1.pixelZoom * 65));

                // Add names to mod selector dropdown
                modChoices.Add(this.controller.ModOptionsList[i].modManifest.Name);
			}

            modSelected = new DropDownComponent(modChoices, "", (int)Game1.smallFont.MeasureString("Stardew Configuration Menu Framework").X, (int)(this.xPositionOnScreen + Game1.pixelZoom * 15), (int)(this.yPositionOnScreen + Game1.pixelZoom * 30));
			AddListeners();

		}

        static public void SetActive()
        {
            var gameMenu = (GameMenu)Game1.activeClickableMenu;
            if (ModSettings.pageIndex != null)
                gameMenu.currentTab = (int)ModSettings.pageIndex;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        internal void AddListeners()
        {
            RemoveListeners();

            modSelected.DropDownOptionSelected += DisableBackgroundSheets;
        }

        private void DisableBackgroundSheets(int selected)
        {
            for (int i = 0; i < Sheets.Count; i++)
            {
                 Sheets[i].invisible = (i != modSelected.selectedOption);
            }
        }

        internal void RemoveListeners(bool children = false)
        {
            if (children)
            {
                modSelected.RemoveListeners();
                Sheets.ForEach(x => { x.RemoveListeners(true); });
            }

            modSelected.DropDownOptionSelected -= DisableBackgroundSheets;
        }

        //private OptionComponent tester = new PlusMinusComponent("Test", -10, 100, 5, 20);

        public override void draw(SpriteBatch b)
        {
            //base.draw(b);
            //tester.draw(b);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);

            base.drawHorizontalPartition(b, (int)(this.yPositionOnScreen + Game1.pixelZoom * 40));

            if (Sheets.Count > 0)
            {
                Sheets[modSelected.selectedOption].draw(b);

                if ((Game1.getMouseX() > this.modSelected.X && Game1.getMouseX() < this.modSelected.X + this.modSelected.Width) && (Game1.getMouseY() > this.modSelected.Y && Game1.getMouseY() < this.modSelected.Y + this.modSelected.Height) && !modSelected.IsActiveComponent())
                {
                    IClickableMenu.drawHoverText(Game1.spriteBatch, this.controller.ModOptionsList[modSelected.selectedOption].modManifest.Description, Game1.smallFont);
                }
            }

            // draw mod select dropdown last, should cover mod settings
            modSelected.draw(b);
            SpriteText.drawString(b, "Mod Options", modSelected.X + modSelected.Width + Game1.pixelZoom * 5, modSelected.Y);


        }

    }
}
