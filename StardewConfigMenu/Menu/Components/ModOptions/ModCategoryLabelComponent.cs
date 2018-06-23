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


namespace StardewConfigMenu.Components.ModOptions {
	class ModCategoryLabelComponent: OptionComponent {

		public override bool Enabled {
			get { return true; }
			protected set { }
		}

		private CategoryLabel option;

		Rectangle bounds = new Rectangle();

		public ModCategoryLabelComponent(CategoryLabel option) : base(option.Label) {
			this.option = option;
			this.bounds.Height = SpriteText.getHeightOfString(this.Label);
			this.bounds.Width = SpriteText.getWidthOfString(this.Label);
		}

		public ModCategoryLabelComponent(string labelText, int x, int y) : base(labelText) {
			this.bounds.X = x;
			this.bounds.Y = y;
			this.bounds.Height = SpriteText.getHeightOfString(this.Label);
			this.bounds.Width = SpriteText.getWidthOfString(this.Label);
		}

		public override void draw(SpriteBatch b, int x, int y) {
			base.draw(b, x, y);
			this.bounds.X = x;
			this.bounds.Y = y;
			this.draw(b);
		}

		// static drawing of component
		public override void draw(SpriteBatch b) {
			SpriteText.drawString(b, (option != null) ? option.Label : this.Label, this.bounds.X, this.bounds.Y - 4 * Game1.pixelZoom);
		}
	}
}
