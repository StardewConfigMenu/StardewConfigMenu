using StardewConfigFramework;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System;
using Microsoft.Xna.Framework.Graphics;




namespace StardewConfigMenu.Panel.Components.ModOptions
{
	internal class SliderComponent: OptionComponent
	{

		internal event PlusMinusValueChanged PlusMinusValueChanged;

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, int x, int y, bool enabled = true) : base(labelText, enabled)
		{
			this.min = Math.Round(min, 3);
			this.max = Math.Round(max, 3);
			this.stepSize = Math.Round(stepsize, 3);

			var valid = CheckValidInput(Math.Round(defaultSelection, 3));
			var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
			this._Value = newVal;

			this.bounds = new Rectangle(x, y, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Height);
			plusButtonbounds = new Rectangle(bounds.X + 10 * Game1.pixelZoom, bounds.Y, Game1.pixelZoom * OptionsPlusMinus.plusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.plusButtonSource.Height);
		}

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool enabled = true) : base(labelText, enabled)
		{
			this.min = Math.Round(min, 3);
			this.max = Math.Round(max, 3);
			this.stepSize = Math.Round(stepsize, 3);

			var valid = CheckValidInput(Math.Round(defaultSelection, 3));
			var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
			this._Value = newVal;

			this.bounds = new Rectangle(0, 0, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Height);

			plusButtonbounds = new Rectangle(bounds.X + (int) valueLabelSize.X + 8 * Game1.pixelZoom, bounds.Y, Game1.pixelZoom * OptionsPlusMinus.plusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.plusButtonSource.Height);
		}

		protected Rectangle plusButtonbounds;
		protected Vector2 valueLabelSize
		{
			get {
				return Game1.smallFont.MeasureString($"{Value}");
			}
		}

		private decimal _min;
		public decimal min
		{
			get {
				return _min;
			}
			private set {
				_min = value;
			}
		}

		private decimal _max;
		public decimal max
		{
			get {
				return _max;
			}
			private set {
				_max = value;
			}
		}

		private decimal _stepSize;
		public decimal stepSize
		{
			get {
				return _stepSize;
			}
			private set {
				_stepSize = value;
			}
		}

		private decimal _Value;
		protected decimal Value
		{
			get {
				return _Value;
			}
			private set {
				var valid = CheckValidInput(Math.Round(value, 3));
				var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
				if (newVal != this._Value)
				{
					this._Value = newVal;
					this.PlusMinusValueChanged?.Invoke(this._Value);
				}
			}
		}

		public void StepUp()
		{
			this.Value = this.Value + this.stepSize;
		}

		public void StepDown()
		{
			this.Value = this.Value - this.stepSize;
		}

		private decimal CheckValidInput(decimal input)
		{
			if (input > max)
				return max;

			if (input < min)
				return min;

			return input;
		}

		protected override void leftClicked(int x, int y)
		{
			base.leftClicked(x, y);

			if (this.bounds.Contains(x, y) && enabled)
			{
				this.StepDown();
			} else if (this.plusButtonbounds.Contains(x, y) && enabled)
			{
				this.StepUp();
			}
		}

		public override void draw(SpriteBatch b, int x, int y)
		{
			this.plusButtonbounds.X = x + (int) this.valueLabelSize.X + 4 * Game1.pixelZoom;
			this.plusButtonbounds.Y = y;
			base.draw(b, x, y);
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);

			b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X), (float) (this.bounds.Y)), OptionsPlusMinus.minusButtonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.4f);

			Utility.drawBoldText(b, $"{Value}", Game1.smallFont, new Vector2((float) (this.bounds.Right + Game1.pixelZoom * 2), (float) (this.bounds.Y + ((this.bounds.Height - valueLabelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f));

			b.Draw(Game1.mouseCursors, new Vector2((float) (this.plusButtonbounds.X), (float) (this.plusButtonbounds.Y)), OptionsPlusMinus.plusButtonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.4f);


			var labelSize = Game1.dialogueFont.MeasureString(this.label);

			Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float) (this.plusButtonbounds.Right + Game1.pixelZoom * 4), (float) (this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

		}
	}
}
