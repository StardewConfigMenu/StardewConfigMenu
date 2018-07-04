using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.UI {
	public struct SCMTextureBox {

		public SCMTextureBox(Texture2D texture, Rectangle sourceRect, Rectangle bounds = new Rectangle()) {
			_Bounds = bounds;
			Texture = texture;
			SourceRect = sourceRect;
		}

		public int X { get => _Bounds.X; set => _Bounds.X = value; }
		public int Y { get => _Bounds.Y; set => _Bounds.Y = value; }
		public int Width { get => _Bounds.Width; set => _Bounds.Width = value; }
		public int Height { get => _Bounds.Height; set => _Bounds.Height = value; }

		public Rectangle Bounds => _Bounds;

		Rectangle _Bounds;

		public Texture2D Texture;
		public Rectangle SourceRect;

		public bool Contains(int x, int y) {
			return Bounds.Contains(x, y);
		}

		public void DrawAt(SpriteBatch b, int x, int y) {
			X = x;
			Y = y;
			Draw(b);
		}

		public void Draw(SpriteBatch b) {
			var origin = new Vector2(Bounds.X, Bounds.Y);
			IClickableMenu.drawTextureBox(b, Texture, SourceRect, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, Color.White, 1f, true);
		}
	}
}