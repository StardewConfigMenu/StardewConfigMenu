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

namespace StardewConfigMenu.Components {

	internal class SCMCheckbox: SCMControl {
		internal delegate void CheckBoxToggledEvent(bool isOn);

		internal event CheckBoxToggledEvent CheckBoxToggled;

		private ClickableTextureComponent Checkbox = StardewTile.CheckboxChecked.ClickableTextureComponent(0, 0);

		public override int Width => Checkbox.bounds.Width;
		public override int Height => Checkbox.bounds.Height;
		public override int X => Checkbox.bounds.X;
		public override int Y => Checkbox.bounds.Y;

		private bool _IsChecked;
		public virtual bool IsChecked {
			get {
				return _IsChecked;
			}
			protected set {
				_IsChecked = value;
				this.CheckBoxToggled?.Invoke(value);
			}
		}

		internal override bool Visible {
			set {
				base.Visible = value;
				Checkbox.visible = value;
			}
		}

		internal SCMCheckbox(string label, bool isChecked, int x, int y, bool enabled = true) : base(label, enabled) {
			this.Checkbox.bounds.X = x;
			this.Checkbox.bounds.Y = y;
			this._IsChecked = isChecked;
		}

		internal SCMCheckbox(string label, bool isChecked, bool enabled = true) : base(label, enabled) {
			this._IsChecked = isChecked;
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			base.ReceiveLeftClick(x, y, playSound);

			if (this.Checkbox.containsPoint(x, y) && Enabled && IsAvailableForSelection) {
				IsChecked = !IsChecked;
				if (playSound)
					Game1.playSound("drumkit6");
			}
		}

		public override void Draw(SpriteBatch b) {
			base.Draw(b);

			if (IsChecked && Checkbox.sourceRect != OptionsCheckbox.sourceRectChecked)
				Checkbox.sourceRect = OptionsCheckbox.sourceRectChecked;
			else if (!IsChecked && Checkbox.sourceRect != OptionsCheckbox.sourceRectUnchecked)
				Checkbox.sourceRect = OptionsCheckbox.sourceRectUnchecked;

			Checkbox.draw(b, Color.White * ((this.Enabled) ? 1f : 0.33f), 0.88f);

			var labelSize = Game1.dialogueFont.MeasureString(this.Label);
			Utility.drawTextWithShadow(b, this.Label, Game1.dialogueFont, new Vector2((float) (this.Checkbox.bounds.Right + Game1.pixelZoom * 4), (float) (this.Checkbox.bounds.Y + ((this.Checkbox.bounds.Height - labelSize.Y) / 2))), this.Enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

		}


	}
}
