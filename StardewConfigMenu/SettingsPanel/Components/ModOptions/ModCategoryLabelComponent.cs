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
using StardewConfigFramework;


namespace StardewConfigMenu.Panel.Components.ModOptions
{
	class ModCategoryLabelComponent: OptionComponent
	{

		public override bool enabled
		{
			get { return true; }
			protected set { }
		}

		private ModOptionCategoryLabel option;

		public ModCategoryLabelComponent(ModOptionCategoryLabel option) : base(option.LabelText)
		{
			this.option = option;
			this.bounds.Height = SpriteText.getHeightOfString(this.label);
			this.bounds.Width = SpriteText.getWidthOfString(this.label);
		}

		public ModCategoryLabelComponent(string labelText, int x, int y) : base(labelText)
		{
			this.bounds.X = x;
			this.bounds.Y = y;
			this.bounds.Height = SpriteText.getHeightOfString(this.label);
			this.bounds.Width = SpriteText.getWidthOfString(this.label);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true) {
			//throw new NotImplementedException();
		}

		// static drawing of component
		public override void draw(SpriteBatch b)
		{
			SpriteText.drawString(b, this.label, this.bounds.X, this.bounds.Y);
		}
	}
}
