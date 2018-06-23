using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewModdingAPI.Events;
using StardewConfigFramework.Options;


namespace StardewConfigMenu.Components.DataBacked {
	internal class ConfigCategoryLabel: SCMControl {

		public override bool Enabled {
			get { return true; }
			protected set { }
		}

		private CategoryLabel Option;

		Rectangle Bounds = new Rectangle();
		public override int X { get => Bounds.X; set => Bounds.X = value; }
		public override int Y { get => Bounds.Y; set => Bounds.Y = value; }
		public override int Height { get => Bounds.Height; set => Bounds.Height = value; }
		public override int Width { get => Bounds.Width; set => Bounds.Width = value; }


		public ConfigCategoryLabel(CategoryLabel option) : base(option.Label) {
			Option = option;
			Bounds.Height = SpriteText.getHeightOfString(Label);
			Bounds.Width = SpriteText.getWidthOfString(Label);
		}

		public ConfigCategoryLabel(string labelText, int x, int y) : base(labelText) {
			Bounds.X = x;
			Bounds.Y = y;
			Bounds.Height = SpriteText.getHeightOfString(Label);
			Bounds.Width = SpriteText.getWidthOfString(Label);
		}

		// static drawing of component
		public override void Draw(SpriteBatch b) {
			SpriteText.drawString(b, (Option != null) ? Option.Label : Label, X, Y - 4 * Game1.pixelZoom);
		}
	}
}
