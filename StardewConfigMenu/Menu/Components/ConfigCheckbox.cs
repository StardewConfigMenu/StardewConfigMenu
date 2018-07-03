using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components {

	sealed class ConfigCheckbox: SCMControl {
		private readonly IConfigToggle ModData;

		private readonly ClickableTextureComponent Checkbox = StardewTile.CheckboxChecked.ClickableTextureComponent(0, 0);

		internal override bool Visible { get => Checkbox.visible; set => Checkbox.visible = value; }
		public override int X { get => Checkbox.bounds.X; set => Checkbox.bounds.X = value; }
		public override int Y { get => Checkbox.bounds.Y; set => Checkbox.bounds.Y = value; }
		public override int Width => Checkbox.bounds.Width;
		public override int Height => Checkbox.bounds.Height;

		public override string Label => ModData.Label;
		public override bool Enabled => ModData.Enabled;
		public bool IsChecked { get => ModData.IsOn; set => ModData.IsOn = value; }

		internal ConfigCheckbox(IConfigToggle option) : this(option, 0, 0) { }

		internal ConfigCheckbox(IConfigToggle option, int x, int y) : base(option.Label, option.Enabled) {
			ModData = option;
			X = x;
			Y = y;
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			if (!Enabled || !IsAvailableForSelection)
				return;

			if (Checkbox.containsPoint(x, y)) {
				IsChecked = !IsChecked;
				if (playSound)
					Game1.playSound("drumkit6");
			}
		}

		public override void Draw(SpriteBatch b) {
			var colorAlpha = (Enabled) ? 1f : 0.33f;

			if (IsChecked && Checkbox.sourceRect != OptionsCheckbox.sourceRectChecked)
				Checkbox.sourceRect = OptionsCheckbox.sourceRectChecked;
			else if (!IsChecked && Checkbox.sourceRect != OptionsCheckbox.sourceRectUnchecked)
				Checkbox.sourceRect = OptionsCheckbox.sourceRectUnchecked;

			Checkbox.draw(b, Color.White * colorAlpha, 0.88f);

			var labelSize = Game1.dialogueFont.MeasureString(Label);
			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2(Checkbox.bounds.Right + Game1.pixelZoom * 4, Checkbox.bounds.Y + ((Checkbox.bounds.Height - labelSize.Y) / 2)), Game1.textColor * colorAlpha, 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}

