﻿using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewConfigMenu {
	public struct StardewTile {

		public static StardewTile OKButton = new StardewTile(Game1.mouseCursors, 46);
		public static StardewTile ClearButton = new StardewTile(Game1.mouseCursors, 47);
		public static StardewTile SetButton = new StardewTile(Game1.mouseCursors, OptionsInputListener.setButtonSource, Game1.pixelZoom);
		public static StardewTile DoneButton = new StardewTile(Game1.mouseCursors, new Rectangle(441, 411, 24, 13), Game1.pixelZoom);
		public static StardewTile GiftButton = new StardewTile(Game1.mouseCursors, new Rectangle(229, 410, 14, 14), Game1.pixelZoom);

		public StardewTile(Texture2D tileSheet, Rectangle source, float scale = 1f) {
			Source = source;
			Scale = scale;
			TileSheet = tileSheet;
		}

		public StardewTile(Texture2D tileSheet, int tilePosition, int width = -1, int height = -1, float scale = 1f) {
			Source = Game1.getSourceRectForStandardTileSheet(tileSheet, tilePosition, width, height);
			Scale = scale;
			TileSheet = tileSheet;
		}

		Rectangle Source;
		float Scale;
		Texture2D TileSheet;

		public ClickableTextureComponent ClickableTextureComponent(int x, int y) {
			return new ClickableTextureComponent(new Rectangle(x, y, (int) (Scale * Source.Width), (int) (Scale * Source.Height)), TileSheet, Source, Scale);
		}
	}
}