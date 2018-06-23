using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewConfigMenu {
	public struct StardewTile {
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

		public ClickableTextureComponent clickableTextureComponent(int x, int y) {
			return new ClickableTextureComponent(new Rectangle(x, y, (int) (Scale * Source.Width), (int) (Scale * Source.Height)), TileSheet, Source, Scale);
		}
	}
}
