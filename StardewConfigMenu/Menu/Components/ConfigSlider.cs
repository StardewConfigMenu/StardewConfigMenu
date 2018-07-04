using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewConfigMenu.UI;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components {

	sealed class ConfigSlider: SCMControl {
		readonly IConfigRange ModData;

		private SCMTextureBox SliderBackground = SCMTextureBox.SliderBackground;
		private ClickableTextureComponent SliderBar = StardewTile.SliderBar.ClickableTextureComponent(0, 0);

		private Vector2 MaxLabelSize = Vector2.Zero;
		private Point Origin = new Point();
		public sealed override int X {
			get => Origin.X;
			set {
				if (Origin.X == value)
					return;

				Origin.X = value;
				SliderBackground.X = value;
				if (ShowValue)
					SliderBackground.X += (int) MaxLabelSize.X + (4 * Game1.pixelZoom);

				UpdateSliderLocation(value, Min, Max, StepSize);
			}
		}
		public sealed override int Y {
			get => Origin.Y;
			set {
				if (Origin.Y == value)
					return;

				Origin.Y = value;
				SliderBackground.Y = value;
				SliderBar.bounds.Y = value;
			}
		}
		public sealed override int Height => Math.Max(SliderBackground.Height, (int) MaxLabelSize.Y);
		public sealed override int Width => SliderBackground.Bounds.Right - Origin.X;

		public sealed override bool Enabled => ModData.Enabled;
		public sealed override string Label => ModData.Label;
		public decimal Min => ModData.Min;
		public decimal Max => ModData.Max;
		public decimal StepSize => ModData.StepSize;
		public bool ShowValue => ModData.ShowValue;
		public decimal Value { get => ModData.Value; set => ModData.Value = value; }

		internal ConfigSlider(IConfigRange option) : this(option, 0, 0) { }

		internal ConfigSlider(IConfigRange option, int x, int y) : base(option.Label, option.Enabled) {
			ModData = option;
			CalculateMaxLabelSize();
			X = x;
			Y = y;
			SliderBackground.Width = OptionsSlider.pixelsWide * Game1.pixelZoom;
			SliderBackground.Height = OptionsSlider.pixelsHigh * Game1.pixelZoom;
			SliderBar.bounds.Width = OptionsSlider.sliderButtonWidth * Game1.pixelZoom;
			SliderBar.bounds.Height = OptionsSlider.pixelsHigh * Game1.pixelZoom;
		}

		private void CalculateMaxLabelSize() {
			var maxRect = Game1.dialogueFont.MeasureString((Max + StepSize % 1).ToString());
			var minRect = Game1.dialogueFont.MeasureString((Min - StepSize % 1).ToString());

			MaxLabelSize = (maxRect.X > minRect.X) ? maxRect : minRect;
		}

		private void UpdateSliderLocation(decimal value, decimal min, decimal max, decimal stepSize) {
			var sectionNum = ((value - max) / stepSize);
			var totalSections = ((max - min) / stepSize);
			var sectionWidth = SliderBackground.Width - SliderBar.bounds.Width / totalSections;
			SliderBar.bounds.X = SliderBackground.X + (SliderBar.bounds.Width / 2) + (int) (sectionWidth * sectionNum);
		}

		private decimal CheckValidInput(decimal input) {
			if (input > Max)
				return Max;

			if (input < Min)
				return Min;

			return ((input - Min) / StepSize) * StepSize + Min;
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			if (!Enabled || !IsAvailableForSelection)
				return;

			if (SliderBackground.Bounds.Contains(x, y)) {
				LeftClickHeld(x, y);
				RegisterAsActiveComponent();
			}
		}

		public override void LeftClickHeld(int x, int y) {
			if (IsActiveComponent) {
				var halfButtonWidth = SliderBar.bounds.Width / 2;
				if (x < SliderBackground.X + halfButtonWidth) {
					Value = Min;
				} else if (x > SliderBackground.Bounds.Right - halfButtonWidth) {
					Value = Max;
				} else {
					var sectionCount = ((Max - Min) / StepSize);
					var sectionWidth = (SliderBackground.Width - SliderBar.bounds.Width) / sectionCount;
					var sectionNum = (x - (SliderBackground.X + halfButtonWidth)) / sectionWidth;
					Value = Min + sectionNum * StepSize;
				}
			}
		}

		public override void ReleaseLeftClick(int x, int y) {
			UnregisterAsActiveComponent();
		}

		public override void Draw(SpriteBatch b) {
			var labelSize = Game1.dialogueFont.MeasureString(Label);
			var valueLabelSize = Game1.dialogueFont.MeasureString($"{Value}");
			float buttonAlpha = Enabled ? 1f : 0.33f;

			if (ShowValue)
				b.DrawString(Game1.dialogueFont, $"{Value}", new Vector2(SliderBackground.X - ((MaxLabelSize.X - valueLabelSize.X) / 2 + valueLabelSize.X + 4 * Game1.pixelZoom), SliderBackground.Y + ((SliderBackground.Height - labelSize.Y) / 2)), Game1.textColor * buttonAlpha);

			SliderBackground.Color = Color.White * buttonAlpha;
			SliderBackground.Draw(b);
			//.draw(b, Color.White * buttonAlpha, 0.88f);
			SliderBar.draw(b, Color.White * buttonAlpha, 1f);

			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2(SliderBackground.Bounds.Right + Game1.pixelZoom * 4, SliderBackground.Y + ((SliderBackground.Height - labelSize.Y) / 2)), Game1.textColor * buttonAlpha, 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
