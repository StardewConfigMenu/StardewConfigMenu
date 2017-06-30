using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;


namespace StardewConfigFramework
{
    public class ModSettingsPage: IClickableMenu
	{
		private const int WIDTH = 800;

		private ClickableTextureComponent upArrow;
		private ClickableTextureComponent downArrow;
		private ClickableTextureComponent scrollBar;
		private Rectangle scrollBarRunner;

		private Settings controller;
        private List<ModPage> Pages = new List<ModPage>();
        private OptionsDropDown modSelected = new OptionsDropDown("Mod", 0);

		internal ModSettingsPage(Settings controller, int x, int y, int width, int height) : base (x, y, width, height, false)
		{
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upArrow.bounds.Height - Game1.pixelZoom * 2);
			//for (int i = 0; i < 7; i++)
			//{
			//	this.optionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((height - Game1.tileSize * 2) / 7), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom), string.Empty + i)
			//	{
			//		myID = i,
			//		downNeighborID = ((i >= 6) ? -7777 : (i + 1)),
			//		upNeighborID = ((i <= 0) ? -7777 : (i - 1)),
			//		fullyImmutable = true
			//	});
			//}

            this.controller = controller;
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
			this.RemoveListeners();

			this.controller.ModAdded += ReloadMenu; 
		}

		internal void RemoveListeners()
		{
			this.controller.ModAdded -= ReloadMenu;
		}

        private void ReloadMenu() {
            // Reset Menu and pages
            this.Pages.Clear();
            this.modSelected.dropDownOptions.Clear();

            for (int i = 0; i < this.controller.ModOptionsList.Count; i++) {
                
                // Create mod page and add it
				this.Pages.Add(new ModPage(this.controller.ModOptionsList[i]));

                // Add names to mod selector dropdown
                this.modSelected.dropDownOptions.Add(this.controller.ModOptionsList[i].modName);
			}

        }

        public override void draw(SpriteBatch b)
        {
            //base.draw(b);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
        }

   	}
}
