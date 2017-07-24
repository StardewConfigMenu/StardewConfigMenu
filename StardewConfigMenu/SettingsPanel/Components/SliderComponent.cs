using StardewConfigFramework;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System;
using Microsoft.Xna.Framework.Graphics;




namespace StardewConfigMenu.Panel.Components {

	internal delegate void SliderValueChanged(decimal Value);

	internal class SliderComponent: OptionComponent {

		internal event SliderValueChanged SliderValueChanged;


		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool showValue, int x, int y, bool enabled = true) : base(labelText, enabled) {
			this.min = Math.Round(min, 3);
			this.max = Math.Round(max, 3);
			this.stepSize = Math.Round(stepsize, 3);
			this.showValue = showValue;

			var valid = CheckValidInput(Math.Round(defaultSelection, 3));
			var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
			this._Value = newVal;

			this.bounds = new Rectangle(x, y, 48 * Game1.pixelZoom, 6 * Game1.pixelZoom);
		}

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool showValue, bool enabled = true) : base(labelText, enabled) {
			this.min = Math.Round(min, 3);
			this.max = Math.Round(max, 3);
			this.stepSize = Math.Round(stepsize, 3);
			this.showValue = showValue;

			var valid = CheckValidInput(Math.Round(defaultSelection, 3));
			var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
			this._Value = newVal;

			this.bounds = new Rectangle(0, 0, 48 * Game1.pixelZoom, 6 * Game1.pixelZoom);
		}

		protected Vector2 valueLabelSize {
			get {
				return Game1.smallFont.MeasureString($"{Value}");
			}
		}

		private decimal _min;
		public decimal min {
			get {
				return _min;
			}
			private set {
				_min = value;
			}
		}

		private decimal _max;
		public decimal max {
			get {
				return _max;
			}
			private set {
				_max = value;
			}
		}

		private decimal _stepSize;
		public decimal stepSize {
			get {
				return _stepSize;
			}
			private set {
				_stepSize = value;
			}
		}

		private decimal _Value;
		protected decimal Value {
			get {
				return _Value;
			}
			private set {
				var valid = CheckValidInput(Math.Round(value, 3));
				var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
				if (newVal != this._Value) {
					this._Value = newVal;
					this.SliderValueChanged?.Invoke(this._Value);
				}
			}
		}

		private bool _showValue;
		protected bool showValue {
			get {
				return _showValue;
			}
			private set {
				_showValue = value;
			}
		}

		private decimal CheckValidInput(decimal input) {
			if (input > max)
				return max;

			if (input < min)
				return min;

			return input;
		}

		private bool scrolling = false;

		public override void receiveRightClick(int x, int y, bool playSound = true) {
			//throw new NotImplementedException();
		}

		/*
		protected override void leftClicked(int x, int y) {
			base.leftClicked(x, y);

			if (this.bounds.Contains(x, y) && enabled && this.IsAvailableForSelection()) {
				scrolling = true;
				// TODO 
			}
		}

		protected override void leftClickHeld(int x, int y) {
			base.leftClickHeld(x, y);

			if (scrolling) {
				if (x < this.bounds.X) {
					this.Value = min;
				} else if (x > this.bounds.Right) {
					this.Value = max;
				} else {
					// TODO
				}
			}
		}

		protected override void leftClickReleased(int x, int y) {
			base.leftClickReleased(x, y);

			scrolling = false;
		} */

		public override void draw(SpriteBatch b) {
			base.draw(b);


			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsSlider.sliderBGSource, this.bounds.X, this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float) Game1.pixelZoom, false);

			b.Draw(Game1.mouseCursors, new Vector2(this.bounds.X + (float) (this.bounds.Width - 10 * Game1.pixelZoom) * ((float) this.Value / (float)((max - min) / stepSize)), (float) (this.bounds.Y)), new Rectangle?(OptionsSlider.sliderButtonRect), Color.White, 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.9f);

			var labelSize = Game1.dialogueFont.MeasureString(this.label);

			Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float) (this.bounds.Right + Game1.pixelZoom * 4), (float) (this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
