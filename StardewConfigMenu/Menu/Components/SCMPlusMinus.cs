using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components {
	using DisplayType = Stepper.DisplayType;

	internal class PlusMinusComponent: SCMControl {
		internal delegate void ValueChangedEvent(decimal Value);

		internal event ValueChangedEvent ValueChanged;

		private readonly ClickableTextureComponent Minus = StardewTile.MinusButton.ClickableTextureComponent(0, 0);
		private readonly ClickableTextureComponent Plus = StardewTile.PlusButton.ClickableTextureComponent(0, 0);

		public sealed override int Width => Plus.bounds.Right - Minus.bounds.X;
		public sealed override int Height => (int) Math.Max(Minus.bounds.Height, MaxLabelSize.Y);
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
		private Vector2 MaxLabelSize = Vector2.Zero;
		private int UnitStringWidth => (int) Game1.dialogueFont.MeasureString(UnitString).X;
		private Vector2 ValueLabelSize => Game1.dialogueFont.MeasureString(Value.ToString());

		private decimal _Min;
		private decimal _Max;
		private decimal _StepSize;
		private DisplayType _Type;
		private decimal _Value;

		public virtual decimal Min { get => _Min; }
		public virtual decimal Max { get => _Max; }
		public virtual decimal StepSize { get => _StepSize; }
		public virtual DisplayType Type => _Type;
		public virtual decimal Value {
			get => _Value;
			protected set {
				var valid = CheckValidInput(Math.Round(value, 3));
				if (valid != _Value) {
					_Value = valid;
					ValueChanged?.Invoke(_Value);
				}
			}
		}
		protected string UnitString {
			get {
				switch (Type) {
					case DisplayType.PERCENT:
						return "%";
					default:
						return "";
				}
			}
		}

		internal PlusMinusComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, DisplayType type = DisplayType.NONE, bool enabled = true) : this(labelText, min, max, stepsize, defaultSelection, 0, 0, type, enabled) { }

		internal PlusMinusComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, int x, int y, DisplayType type = DisplayType.NONE, bool enabled = true) : base(labelText, enabled) {
			_Min = Math.Round(min, 3);
			_Max = Math.Round(max, 3);
			_StepSize = Math.Round(stepsize, 3);
			_Type = type;

			var rounded = Math.Round(defaultSelection, 3);
			var valid = CheckValidInput(rounded);
			_Value = valid;

			CalculateMaxLabelSize();

			X = x;
			Y = y;
		}

		protected void CalculateMaxLabelSize() {
			var maxRect = Game1.dialogueFont.MeasureString((_Max + _StepSize % 1).ToString());
			var minRect = Game1.dialogueFont.MeasureString((_Min - _StepSize % 1).ToString());

			MaxLabelSize = (maxRect.X > minRect.X) ? maxRect : minRect;
		}

		private decimal CheckValidInput(decimal input) {
			if (input > _Max)
				return _Max;

			if (input < _Min)
				return _Min;

			return ((input - _Min) / _StepSize) * _StepSize + _Min;
		}

		protected virtual void StepUp() {
			Value += StepSize;
		}

		protected virtual void StepDown() {
			Value -= StepSize;
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			if (!Enabled)
				return;

			if (Minus.containsPoint(x, y) && IsAvailableForSelection) {
				var prevValue = Value;
				StepDown();
				if (playSound && prevValue != Value)
					Game1.playSound("drumkit6");
			} else if (Plus.containsPoint(x, y) && IsAvailableForSelection) {
				var prevValue = Value;
				StepUp();
				if (playSound && prevValue != Value)
					Game1.playSound("drumkit6");
			}
		}

		public override void Draw(SpriteBatch b) {
			float buttonAlpha = Enabled ? 1f : 0.33f;

			Minus.draw(b, Color.White * ((Enabled && (Value - StepSize >= Min)) ? 1f : 0.33f), 0.88f);
			Plus.draw(b, Color.White * ((Enabled && (Value + StepSize <= Max)) ? 1f : 0.33f), 0.88f);

			b.DrawString(Game1.dialogueFont, Value.ToString() + UnitString, new Vector2((float) (Minus.bounds.Right + (Plus.bounds.X - Minus.bounds.Right - ValueLabelSize.X - UnitStringWidth) / 2), (float) (Minus.bounds.Y + ((Minus.bounds.Height - MaxLabelSize.Y) / 2))), Enabled ? Game1.textColor : (Game1.textColor * 0.33f));

			var labelSize = Game1.dialogueFont.MeasureString(Label);
			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (Plus.bounds.Right + Game1.pixelZoom * 4), (float) (Minus.bounds.Y + ((Minus.bounds.Height - labelSize.Y) / 2))), Game1.textColor * buttonAlpha, 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
