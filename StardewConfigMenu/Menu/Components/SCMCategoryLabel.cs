using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace StardewConfigMenu.Components {

	internal class SCMCategoryLabel: SCMControl {

		public override bool Enabled {
			get { return true; }
			protected set { }
		}

		protected Rectangle Bounds = new Rectangle();
		public override int X { get => Bounds.X; set => Bounds.X = value; }
		public override int Y { get => Bounds.Y; set => Bounds.Y = value; }
		public override int Height { get => Bounds.Height; set => Bounds.Height = value; }
		public override int Width { get => Bounds.Width; set => Bounds.Width = value; }

		public SCMCategoryLabel(string label) : this(label, 0, 0) { }

		public SCMCategoryLabel(string labelText, int x, int y) : base(labelText) {
			Bounds.X = x;
			Bounds.Y = y;
			Bounds.Height = SpriteText.getHeightOfString(Label);
			Bounds.Width = SpriteText.getWidthOfString(Label);
		}

		// static drawing of component
		public override void Draw(SpriteBatch b) {
			SpriteText.drawString(b, Label, X, Y - 4 * Game1.pixelZoom);
		}
	}
}
