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

		internal override bool Visible { get => Checkbox.visible; set => Checkbox.visible = value; }

		public override int Width => Checkbox.bounds.Width;
		public override int Height => Checkbox.bounds.Height;
		public override int X { get => Checkbox.bounds.X; set => Checkbox.bounds.X = value; }
		public override int Y { get => Checkbox.bounds.Y; set => Checkbox.bounds.Y = value; }

		private bool _IsChecked;
		public virtual bool IsChecked {
			get => _IsChecked;
			protected set {
				if (_IsChecked != value)
					return;

				_IsChecked = value;
				CheckBoxToggled?.Invoke(value);
			}
		}

		internal SCMCheckbox(string label, bool isChecked, bool enabled = true) : this(label, isChecked, 0, 0, enabled) { }

		internal SCMCheckbox(string label, bool isChecked, int x, int y, bool enabled = true) : base(label, enabled) {
			Checkbox.bounds.X = x;
			Checkbox.bounds.Y = y;
			_IsChecked = isChecked;
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			base.ReceiveLeftClick(x, y, playSound);

			if (Checkbox.containsPoint(x, y) && Enabled && IsAvailableForSelection) {
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

			Checkbox.draw(b, Color.White * ((Enabled) ? 1f : 0.33f), 0.88f);

			var labelSize = Game1.dialogueFont.MeasureString(Label);
			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (Checkbox.bounds.Right + Game1.pixelZoom * 4), (float) (Checkbox.bounds.Y + ((Checkbox.bounds.Height - labelSize.Y) / 2))), Enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

		}
	}
}
