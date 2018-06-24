using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components {

	internal class SCMCheckbox: SCMControl {
		internal delegate void CheckBoxToggledEvent(bool isOn);

		internal event CheckBoxToggledEvent CheckBoxToggled;

		private readonly ClickableTextureComponent Checkbox = StardewTile.CheckboxChecked.ClickableTextureComponent(0, 0);

		internal sealed override bool Visible => Checkbox.visible;
		public sealed override int Width => Checkbox.bounds.Width;
		public sealed override int Height => Checkbox.bounds.Height;
		public sealed override int X => Checkbox.bounds.X;
		public sealed override int Y => Checkbox.bounds.Y;

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
			X = x;
			Y = y;
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
