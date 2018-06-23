using StardewConfigFramework.Options;
using StardewValley;
using Microsoft.Xna.Framework;

namespace StardewConfigMenu.Components.ModData {
	internal class ModSliderComponent: SliderComponent {

		readonly Range Option;

		public override bool Enabled => Option.Enabled;

		public override string Label => Option.Label;

		protected override decimal min => Option.Min;
		protected override decimal max => Option.Max;
		protected override decimal stepSize => Option.StepSize;
		protected override bool showValue => Option.ShowValue;
		protected override decimal Value {
			get {
				return Option.Value;
			}
			set {
				Option.Value = value;
			}
		}

		internal ModSliderComponent(Range option, int x, int y) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, option.ShowValue, x, y, option.Enabled) {
			this.Option = option;
		}

		internal ModSliderComponent(Range option) : this(option, 0, 0) { }

	}
}
