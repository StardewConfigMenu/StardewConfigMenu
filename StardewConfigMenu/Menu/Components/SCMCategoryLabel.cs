using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace StardewConfigMenu.Components {

	internal class SCMCategoryLabel: SCMControl {

		public sealed override bool Enabled {
			get { return true; }
			protected set { }
		}

		protected Rectangle Bounds = new Rectangle();
		public sealed override int X { get => Bounds.X; set => Bounds.X = value; }
		public sealed override int Y { get => Bounds.Y; set => Bounds.Y = value; }
		public sealed override int Height { get => Bounds.Height; set => Bounds.Height = value; }
		public sealed override int Width { get => Bounds.Width; set => Bounds.Width = value; }

		public SCMCategoryLabel(string label) : this(label, 0, 0) { }

		public SCMCategoryLabel(string labelText, int x, int y) : base(labelText) {
			X = x;
			Y = y;
			Height = SpriteText.getHeightOfString(Label);
			Width = SpriteText.getWidthOfString(Label);
		}

		// static drawing of component
		public override void Draw(SpriteBatch b) {
			SpriteText.drawString(b, Label, X, Y - 4 * Game1.pixelZoom);
		}
	}
}
