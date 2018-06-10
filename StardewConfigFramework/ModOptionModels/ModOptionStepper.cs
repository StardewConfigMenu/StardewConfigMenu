using System;

namespace StardewConfigFramework {
	public delegate void ModOptionStepperHandler(string identifier, decimal currentValue);

	public class ModOptionStepper: ModOption {
		public event ModOptionStepperHandler ValueChanged;

		public ModOptionStepper(string identifier, string labelText, decimal min, decimal max, decimal stepsize, decimal defaultValue, DisplayType type = DisplayType.NONE, bool enabled = true) : base(identifier, labelText, enabled) {
			this.min = Math.Round(min, 3);
			this.max = Math.Round(max, 3);
			this.stepSize = Math.Round(stepsize, 3);
			this.type = type;

			var valid = CheckValidInput(Math.Round(defaultValue, 3));
			this.Value = valid - ((valid - min) % stepSize);

		}

		readonly public decimal min;
		readonly public decimal max;
		readonly public decimal stepSize;
		readonly public DisplayType type;

		private decimal _Value;
		public decimal Value {

			get {
				return _Value;
			}

			set {
				var valid = CheckValidInput(Math.Round(value, 3));
				var newVal = (int) ((valid - min) / stepSize) * stepSize + min;
				if (newVal != this._Value) {
					this._Value = newVal;
					this.ValueChanged?.Invoke(this.identifier, this._Value);
				}
			}
		}


		public void StepUp() {
			this.Value = this.Value + this.stepSize;
		}

		public void StepDown() {
			this.Value = this.Value - this.stepSize;
		}

		private decimal CheckValidInput(decimal input) {
			if (input > max)
				return max;

			if (input < min)
				return min;

			return input;
		}

	}

	public enum DisplayType {
		NONE, PERCENT
	}
}