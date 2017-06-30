﻿using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;

namespace StardewConfigFramework
{
    static internal class Constants {
        internal const int TabNumber = 11; 
    }

    internal class SettingsTab: ClickableComponent
    {

        private Settings controller;
		//
		// Constructors
		//


		internal SettingsTab(Settings controller, Rectangle bounds, string name, string label) : base(bounds, name, label)
		{
            this.controller = controller;
			this.bounds = bounds;
			this.name = name;
			this.label = label;
		}

		internal SettingsTab(Settings controller, Rectangle bounds, Item item): base(bounds, item)
		{
			this.controller = controller;
			this.bounds = bounds;
			this.item = item;
		}

		

		internal SettingsTab(Settings controller, Rectangle bounds, string name): base(bounds, name)
		{
			this.controller = controller;
			this.bounds = bounds;
			this.name = name;
		}

		internal void AddListeners()
		{
		}

		internal void RemoveListeners()
		{

		}

		internal void draw(SpriteBatch b)
		{
            var gameMenu = (GameMenu)Game1.activeClickableMenu;
            b.Draw(Game1.mouseCursors, new Vector2((float)this.bounds.X, (float)(this.bounds.Y + (gameMenu.currentTab == 8 ? 8 : 0))), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);

			// Draw icon
			b.Draw(Game1.mouseCursors, new Vector2((float)this.bounds.X + 8, (float)(this.bounds.Y + (gameMenu.currentTab == 8 ? 8 : 0)) + 14), new Rectangle(128 / 4, 2688 / 4, 64 / 4, 64 / 4), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 1);

			if (bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
			{
				IClickableMenu.drawHoverText(Game1.spriteBatch, "Mod Options", Game1.smallFont);
			}
        }
    }
}
