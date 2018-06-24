using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace StardewConfigMenu.Components {

	internal class SliderComponent: SCMControl {
		internal delegate void ValueChangedEvent(decimal Value);

		internal event ValueChangedEvent ValueChanged;

		private readonly ClickableTextureComponent SliderBackground = StardewTile.SliderBackground.ClickableTextureComponent(0, 0, 48, 6);
		private readonly ClickableTextureComponent SliderButton = StardewTile.SliderButton.ClickableTextureComponent(0, 0);

		private Vector2 MaxLabelSize = Vector2.Zero;
		private Point Origin = new Point();
		public sealed override int X {
			get => Origin.X;
			set {
				if (Origin.X == value)
					return;

				Origin.X = value;
				SliderBackground.bounds.X = value;
				if (ShowValue)
					SliderBackground.bounds.X += (int) MaxLabelSize.X + (4 * Game1.pixelZoom);

				UpdateSliderLocation();
			}
		}
		public sealed override int Y {
			get => Origin.Y;
			set {
				if (Origin.Y == value)
					return;

				Origin.Y = value;
				SliderBackground.bounds.Y = value;
				SliderButton.bounds.Y = value;
			}
		}
		public sealed override int Height => Math.Max(SliderBackground.bounds.Height, (int) MaxLabelSize.Y);
		public sealed override int Width => SliderBackground.bounds.Right - Origin.X;

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool showValue, bool enabled = true) : this(labelText, min, max, stepsize, defaultSelection, showValue, 0, 0, enabled) { }

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool showValue, int x, int y, bool enabled = true) : base(labelText, enabled) {
			_min = Math.Round(min, 3);
			_max = Math.Round(max, 3);
			_stepSize = Math.Round(stepsize, 3);
			_showValue = showValue;

			var valid = CheckValidInput(Math.Round(defaultSelection, 3));
			_Value = valid;

			RecalculateMaxLabelSize();

			X = x;
			Y = y;
		}

		protected void RecalculateMaxLabelSize() {
			var maxRect = Game1.dialogueFont.MeasureString((Max + StepSize % 1).ToString());
			var minRect = Game1.dialogueFont.MeasureString((Min - StepSize % 1).ToString());

			MaxLabelSize = (maxRect.X > minRect.X) ? maxRect : minRect;
		}

		protected void UpdateSliderLocation() {
			var sectionNum = ((Value - Min) / StepSize);
			var totalSections = ((Max - Min) / StepSize);
			var sectionWidth = SliderBackground.bounds.Width - SliderButton.bounds.Width / totalSections;
			SliderButton.bounds.X = SliderBackground.bounds.X + (SliderButton.bounds.Width / 2) + (int) (sectionWidth * sectionNum);
		}

		private decimal _min;
		public virtual decimal Min { get => _min; }

		private decimal _max;
		public virtual decimal Max { get => _max; }

		private decimal _stepSize;
		public virtual decimal StepSize { get => _stepSize; }

		private bool _showValue;
		public virtual bool ShowValue { get => _showValue; }

		private decimal _Value;
		public virtual decimal Value {
			get => _Value;
			set {
				var valid = CheckValidInput(Math.Round(value, 3));
				if (valid != _Value) {
					_Value = valid;
					ValueChanged?.Invoke(_Value);
				}
			}
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

			if (SliderBackground.bounds.Contains(x, y)) {
				LeftClickHeld(x, y);
				RegisterAsActiveComponent();
			}
		}

		public override void LeftClickHeld(int x, int y) {
			if (IsActiveComponent) {
				var halfButtonWidth = SliderButton.bounds.Width / 2;
				if (x < SliderBackground.bounds.X + halfButtonWidth) {
					Value = Min;
				} else if (x > SliderBackground.bounds.Right - halfButtonWidth) {
					Value = Max;
				} else {
					var sectionCount = ((Max - Min) / StepSize);
					var sectionWidth = (SliderBackground.bounds.Width - SliderBackground.bounds.Width) / sectionCount;
					var sectionNum = (x - SliderBackground.bounds.X) / sectionWidth;
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
				b.DrawString(Game1.dialogueFont, $"{Value}", new Vector2(SliderBackground.bounds.X - ((MaxLabelSize.X - valueLabelSize.X) / 2 + valueLabelSize.X + 4 * Game1.pixelZoom), (float) (SliderBackground.bounds.Y + ((SliderBackground.bounds.Height - labelSize.Y) / 2))), Game1.textColor * buttonAlpha);

			SliderBackground.draw(b, Color.White * buttonAlpha, 0.88f);

			SliderButton.draw(b, Color.White * buttonAlpha, 0.88f);

			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (SliderBackground.bounds.Right + Game1.pixelZoom * 4), (float) (SliderBackground.bounds.Y + ((SliderBackground.bounds.Height - labelSize.Y) / 2))), Game1.textColor * buttonAlpha, 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
