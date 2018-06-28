using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigSlider: SliderComponent {
		readonly Range ModData;

		public sealed override bool Enabled => ModData.Enabled;
		public sealed override string Label => ModData.Label;
		public sealed override decimal Min => ModData.Min;
		public sealed override decimal Max => ModData.Max;
		public sealed override decimal StepSize => ModData.StepSize;
		public sealed override bool ShowValue => ModData.ShowValue;
		public sealed override decimal Value => ModData.Value;

		internal ConfigSlider(Range option) : this(option, 0, 0) { }

		internal ConfigSlider(Range option, int x, int y) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, option.ShowValue, x, y, option.Enabled) {
			ModData = option;
		}
	}
}
