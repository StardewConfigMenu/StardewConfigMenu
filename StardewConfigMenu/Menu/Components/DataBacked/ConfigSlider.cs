using StardewConfigFramework.Options;
using StardewValley;
using Microsoft.Xna.Framework;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigSlider: SliderComponent {
		readonly Range ModData;

		public override bool Enabled => ModData.Enabled;

		public override string Label => ModData.Label;

		protected override decimal min => ModData.Min;
		protected override decimal max => ModData.Max;
		protected override decimal stepSize => ModData.StepSize;
		protected override bool showValue => ModData.ShowValue;
		protected override decimal Value {
			get {
				return ModData.Value;
			}
			set {
				ModData.Value = value;
			}
		}

		internal ConfigSlider(Range option, int x, int y) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, option.ShowValue, x, y, option.Enabled) {
			this.ModData = option;
		}

		internal ConfigSlider(Range option) : this(option, 0, 0) { }

	}
}
