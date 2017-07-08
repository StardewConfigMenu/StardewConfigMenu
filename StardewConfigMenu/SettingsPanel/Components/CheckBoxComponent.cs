using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewConfigFramework;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Panel.Components
{
	internal delegate void CheckBoxToggled(bool isOn);

	internal class CheckBoxComponent: OptionComponent
	{

		internal event CheckBoxToggled CheckBoxToggled;
		//
		// Static Fields
		//

		public virtual bool IsChecked
		{
			get {
				return _IsChecked;
			}
			protected set {
				_IsChecked = value;
				this.CheckBoxToggled?.Invoke(value);
			}
		}

		private bool _IsChecked;


		//
		// Fields
		//

		internal CheckBoxComponent(string label, bool isChecked, int x, int y, bool enabled = true) : base(label, enabled)
		{
			this.bounds = new Rectangle(x, y, Game1.pixelZoom * OptionsCheckbox.sourceRectChecked.Width, Game1.pixelZoom * OptionsCheckbox.sourceRectChecked.Height);
			this._IsChecked = isChecked;
		}

		internal CheckBoxComponent(string label, bool isChecked, bool enabled = true) : base(label, enabled)
		{
			this.bounds = new Rectangle(0, 0, Game1.pixelZoom * OptionsCheckbox.sourceRectChecked.Width, Game1.pixelZoom * OptionsCheckbox.sourceRectChecked.Height);
			this._IsChecked = isChecked;
		}

		protected override void leftClicked(int x, int y)
		{
			base.leftClicked(x, y);

			if (this.bounds.Contains(x, y) && enabled)
			{
				IsChecked = !IsChecked;
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);

			b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X), (float) (this.bounds.Y)), new Rectangle?((this.IsChecked) ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked), Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.4f);

			var labelSize = Game1.dialogueFont.MeasureString(this.label);

			Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float) (this.bounds.Right + Game1.pixelZoom * 4), (float) (this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

		}
	}
}
