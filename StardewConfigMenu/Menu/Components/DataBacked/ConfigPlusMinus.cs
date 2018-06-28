using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigPlusMinus: PlusMinusComponent {
		private readonly IStepper ModData;

		public sealed override bool Enabled => ModData.Enabled;
		public sealed override string Label => ModData.Label;
		public sealed override decimal Min => ModData.Min;
		public sealed override decimal Max => ModData.Max;
		public sealed override decimal StepSize => ModData.StepSize;
		public sealed override decimal Value => ModData.Value;

		internal ConfigPlusMinus(IStepper option) : this(option, 0, 0) { }

		internal ConfigPlusMinus(IStepper option, int x, int y) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, x, y, option.DisplayType, option.Enabled) {
			ModData = option;
		}
	}
}
