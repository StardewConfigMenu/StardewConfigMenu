using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace StardewConfigMenu.Components {

	internal delegate void SliderValueChanged(decimal Value);

	internal class SliderComponent: SCMControl {

		internal event SliderValueChanged SliderValueChanged;

		Rectangle bounds = new Rectangle();

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool showValue, int x, int y, bool enabled = true) : base(labelText, enabled) {
			this._min = Math.Round(min, 3);
			this._max = Math.Round(max, 3);
			this._stepSize = Math.Round(stepsize, 3);
			this._showValue = showValue;

			var valid = CheckValidInput(Math.Round(defaultSelection, 3));
			var newVal = (int) ((valid - _min) / _stepSize) * _stepSize + _min;
			this._Value = newVal;

			this.bounds = new Rectangle(x, y, 48 * Game1.pixelZoom, 6 * Game1.pixelZoom);
		}

		internal SliderComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, bool showValue, bool enabled = true) : this(labelText, min, max, stepsize, defaultSelection, showValue, 0, 0, enabled) { }

		protected int valueLabelWidth {
			get {
				if (showValue)
					return (int) Math.Max(Game1.dialogueFont.MeasureString($"{min}").X, Game1.dialogueFont.MeasureString($"{max}").X);
				else
					return 0;
			}
		}

		private decimal _min;
		protected virtual decimal min {
			get {
				return _min;
			}
			private set {
				_min = value;
			}
		}

		private decimal _max;
		protected virtual decimal max {
			get {
				return _max;
			}
			private set {
				_max = value;
			}
		}

		private decimal _stepSize;
		protected virtual decimal stepSize {
			get {
				return _stepSize;
			}
			private set {
				_stepSize = value;
			}
		}

		private decimal _Value;
		protected virtual decimal Value {
			get {
				return _Value;
			}
			set {
				var valid = CheckValidInput(Math.Round(value, 3));
				var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
				if (newVal != this._Value) {
					this._Value = newVal;
					this.SliderValueChanged?.Invoke(this._Value);
				}
			}
		}

		private bool _showValue;
		protected virtual bool showValue {
			get {
				return _showValue;
			}
			private set {
				_showValue = value;
			}
		}

		protected decimal CheckValidInput(decimal input) {
			if (input > _max)
				return _max;

			if (input < _min)
				return _min;

			return input;
		}

		private bool scrolling = false;

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			base.ReceiveLeftClick(x, y, playSound);

			if (this.bounds.Contains(x, y) && Enabled && this.IsAvailableForSelection) {
				scrolling = true;
				// TODO 
			}
		}

		public override void LeftClickHeld(int x, int y) {
			base.LeftClickHeld(x, y);

			if (scrolling) {
				if (x < this.bounds.X) {
					this.Value = min;
				} else if (x > this.bounds.Right) {
					this.Value = max;
				} else {
					// TODO
					//this.bounds.X + (float) (this.bounds.Width - 10 * Game1.pixelZoom) * ((float) this.Value / (float) ((max - min) / stepSize)
					var sectionSize = (this.bounds.Width - 10 * Game1.pixelZoom) / ((max - min) / stepSize);
					var sectionNum = (x - this.bounds.X) / sectionSize;
					this.Value = min + sectionNum * stepSize;
				}
			}
		}

		public override void ReleaseLeftClick(int x, int y) {
			base.ReleaseLeftClick(x, y);
			scrolling = false;
		}

		public override void Draw(SpriteBatch b, int x, int y) {
			base.Draw(b, x, y);

			bounds.X = x;
			bounds.Y = y;

			if (showValue)
				bounds.X += valueLabelWidth + 4 * Game1.pixelZoom;


			this.Draw(b);
		}



		public override void Draw(SpriteBatch b) {
			base.Draw(b);

			var labelSize = Game1.dialogueFont.MeasureString(this.Label);

			if (showValue)
				b.DrawString(Game1.dialogueFont, $"{Value}", new Vector2(bounds.X - ((valueLabelWidth - Game1.dialogueFont.MeasureString($"{Value}").X) / 2 + Game1.dialogueFont.MeasureString($"{Value}").X + 4 * Game1.pixelZoom), (float) (this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.Enabled ? Game1.textColor : (Game1.textColor * 0.33f));

			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsSlider.sliderBGSource, this.bounds.X, this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float) Game1.pixelZoom, false);

			b.Draw(Game1.mouseCursors, new Vector2(this.bounds.X + (float) ((this.bounds.Width - 10 * Game1.pixelZoom) * (((this.Value - min) / stepSize) / ((max - min) / stepSize))), (float) (this.bounds.Y)), new Rectangle?(OptionsSlider.sliderButtonRect), Color.White, 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.9f);

			Utility.drawTextWithShadow(b, this.Label, Game1.dialogueFont, new Vector2((float) (this.bounds.Right + Game1.pixelZoom * 4), (float) (this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.Enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
