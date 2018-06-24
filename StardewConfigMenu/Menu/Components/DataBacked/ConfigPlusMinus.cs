using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigPlusMinus: PlusMinusComponent {
		readonly Stepper ModData;

		public override bool Enabled => ModData.Enabled;

		public override string Label => ModData.Label;

		public override decimal min => ModData.Min;
		public override decimal max => ModData.Max;
		public override decimal stepSize => ModData.StepSize;
		public override decimal Value {
			get {
				return ModData.Value;
			}
			protected set {
				ModData.Value = value;
			}
		}

		internal ConfigPlusMinus(Stepper option, int x, int y) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, x, y, option.Type, option.Enabled) {
			this.ModData = option;
		}

		internal ConfigPlusMinus(Stepper option) : base(option.Label, option.Min, option.Max, option.StepSize, option.Value, option.Type, option.Enabled) {
			this.ModData = option;
		}
	}
}
