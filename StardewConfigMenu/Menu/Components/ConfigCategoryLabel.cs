using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace StardewConfigMenu.Components.DataBacked {

	internal sealed class ConfigCategoryLabel: SCMControl {
		private ICategoryLabel ModData;
		public sealed override string Label => ModData.Label;

		protected Rectangle Bounds = new Rectangle();
		public sealed override int X { get => Bounds.X; set => Bounds.X = value; }
		public sealed override int Y { get => Bounds.Y; set => Bounds.Y = value; }
		public sealed override int Height { get => Bounds.Height; set => Bounds.Height = value; }
		public sealed override int Width { get => Bounds.Width; set => Bounds.Width = value; }

		public sealed override bool Enabled {
			get { return true; }
			protected set { }
		}

		public ConfigCategoryLabel(ICategoryLabel option) : this(option, 0, 0) { }

		public ConfigCategoryLabel(ICategoryLabel option, int x, int y) : base(option.Label) {
			ModData = option;
			X = x;
			Y = y;
			Height = SpriteText.getHeightOfString(Label);
			Width = SpriteText.getWidthOfString(Label);
		}

		public override void Draw(SpriteBatch b) {
			SpriteText.drawString(b, Label, X, Y - 4 * Game1.pixelZoom);
		}
	}
}
