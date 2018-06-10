using System;

namespace StardewConfigFramework {
	public delegate void ModOptionRangeHandler(string identifier, decimal currentValue);

	public class ModOptionRange: ModOption {
		public event ModOptionRangeHandler ValueChanged;

		public ModOptionRange(string identifier, string label, decimal min, decimal max, decimal stepSize, decimal defaultSelection, bool showValue, bool enabled = true) : base(identifier, label, enabled) {
			this.showValue = showValue;
			this.stepSize = stepSize;
			this.min = min;
			this.max = max;
			this.Value = defaultSelection;
		}

		readonly public decimal min;
		readonly public decimal max;
		readonly public decimal stepSize;
		readonly public bool showValue;

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

		private decimal CheckValidInput(decimal input) {
			if (input > max)
				return max;

			if (input < min)
				return min;

			return input;
		}

	}
}