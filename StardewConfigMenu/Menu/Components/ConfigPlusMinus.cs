using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components {

	sealed class ConfigPlusMinus: SCMControl {
		private readonly IConfigStepper ModData;

		private readonly ClickableTextureComponent Minus = StardewTile.MinusButton.ClickableTextureComponent(0, 0);
		private readonly ClickableTextureComponent Plus = StardewTile.PlusButton.ClickableTextureComponent(0, 0);

		public sealed override int X {
			get => Minus.bounds.X;
			set {
				if (X == value)
					return;

				Minus.bounds.X = value;
				Plus.bounds.X = Minus.bounds.Right + (int) MaxLabelSize.X + UnitStringWidth + 4 * Game1.pixelZoom;
			}
		}
		public sealed override int Y {
			get => Minus.bounds.Y;
			set {
				if (Y == value)
					return;

				Minus.bounds.Y = value;
				Plus.bounds.Y = value;
			}
		}
		public sealed override int Width => Plus.bounds.Right - Minus.bounds.X;
		public sealed override int Height => (int) Math.Max(Minus.bounds.Height, MaxLabelSize.Y);

		private Vector2 MaxLabelSize = Vector2.Zero;
		private int UnitStringWidth => (int) Game1.dialogueFont.MeasureString(UnitString).X;
		private Vector2 ValueLabelSize => Game1.dialogueFont.MeasureString(Value.ToString());

		public sealed override bool Enabled => ModData.Enabled;
		public sealed override string Label => ModData.Label;
		public decimal Min => ModData.Min;
		public decimal Max => ModData.Max;
		public decimal StepSize => ModData.StepSize;
		public decimal Value { get => ModData.Value; set => ModData.Value = value; }
		public RangeDisplayType DisplayType => ModData.DisplayType;

		private string UnitString {
			get {
				switch (DisplayType) {
					case RangeDisplayType.PERCENT:
						return "%";
					default:
						return "";
				}
			}
		}

		internal ConfigPlusMinus(IConfigStepper option) : this(option, 0, 0) { }

		internal ConfigPlusMinus(IConfigStepper option, int x, int y) : base(option.Label, option.Enabled) {
			ModData = option;

			CalculateMaxLabelSize();

			X = x;
			Y = y;
		}

		private void CalculateMaxLabelSize() {
			var maxRect = Game1.dialogueFont.MeasureString((Max + StepSize % 1).ToString());
			var minRect = Game1.dialogueFont.MeasureString((Min - StepSize % 1).ToString());

			MaxLabelSize = (maxRect.X > minRect.X) ? maxRect : minRect;
		}

		private decimal CheckValidInput(decimal input) {
			if (input > Max)
				return Max;

			if (input < Min)
				return Min;

			return ((input - Min) / StepSize) * StepSize + Min;
		}

		private void StepUp() {
			Value += StepSize;
		}

		private void StepDown() {
			Value -= StepSize;
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			if (!Enabled)
				return;

			var prevValue = Value;
			if (Minus.containsPoint(x, y) && IsAvailableForSelection) {
				StepDown();
				if (playSound && prevValue != Value)
					Game1.playSound("drumkit6");
			} else if (Plus.containsPoint(x, y) && IsAvailableForSelection) {
				StepUp();
				if (playSound && prevValue != Value)
					Game1.playSound("drumkit6");
			}
		}

		public override void Draw(SpriteBatch b) {
			float buttonAlpha = Enabled ? 1f : 0.33f;

			Minus.draw(b, Color.White * ((Enabled && ModData.CanStepDown) ? 1f : 0.33f), 0.88f);
			Plus.draw(b, Color.White * ((Enabled && ModData.CanStepUp) ? 1f : 0.33f), 0.88f);

			b.DrawString(Game1.dialogueFont, Value.ToString() + UnitString, new Vector2((float) (Minus.bounds.Right + (Plus.bounds.X - Minus.bounds.Right - ValueLabelSize.X - UnitStringWidth) / 2), (float) (Minus.bounds.Y + ((Minus.bounds.Height - MaxLabelSize.Y) / 2))), Enabled ? Game1.textColor : (Game1.textColor * 0.33f));

			var labelSize = Game1.dialogueFont.MeasureString(Label);
			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (Plus.bounds.Right + Game1.pixelZoom * 4), (float) (Minus.bounds.Y + ((Minus.bounds.Height - labelSize.Y) / 2))), Game1.textColor * buttonAlpha, 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
