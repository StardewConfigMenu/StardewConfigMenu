using Newtonsoft.Json;

namespace StardewConfigFramework
{
	public delegate void ModOptionRangeHandler(string identifier, decimal currentValue);

	public class ModOptionRange: ModOption
	{
		public event ModOptionRangeHandler ValueChanged;

		[JsonConstructor]
		public ModOptionRange(string identifier, string label, decimal min, decimal max, decimal defaultSelection, bool showValue, bool enabled = true) : base(identifier, label, enabled)
		{
			this.Value = defaultSelection;
			this.showValue = showValue;
		}

		public decimal min { get; private set; }
		public decimal max { get; private set; }
		public decimal Value { get; private set; }
		public bool showValue { get; private set; }

		public void SetValue(decimal selection)
		{
			this.Value = CheckValidInput(selection);
			this.ValueChanged(this.identifier, this.Value);
		}

		public void SetValueByPercentage(decimal percent)
		{
			decimal newValue = ((max - min) * percent) + min;
			this.Value = CheckValidInput(newValue);
			this.ValueChanged(this.identifier, this.Value);
		}

		private decimal CheckValidInput(decimal input)
		{
			if (input > max)
				return max;

			if (input < min)
				return min;

			return input;
		}

	}
}