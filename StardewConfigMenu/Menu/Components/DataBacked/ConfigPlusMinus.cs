using StardewConfigFramework.Options;
using StardewValley;
using Microsoft.Xna.Framework;

namespace StardewConfigMenu.Components.DataBacked {
	internal class ModPlusMinusComponent: PlusMinusComponent {
		readonly Stepper Option;

		public override bool Enabled => Option.Enabled;

		public override string Label => Option.Label;

		public override decimal min => Option.Min;
		public override decimal max => Option.Max;
		public override decimal stepSize => Option.StepSize;
		public override decimal Value {
			get {
				return Option.Value;
			}
			protected set {
				Option.Value = value;
			}
		}

		internal ModPlusMinusComponent(Stepper option, int x, int y) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, x, y, option.Type, option.Enabled) {
			this.Option = option;
		}

		internal ModPlusMinusComponent(Stepper option) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, option.Type, option.Enabled) {
			this.Option = option;
		}
	}
}
