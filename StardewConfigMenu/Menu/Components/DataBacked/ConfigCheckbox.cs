using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigCheckbox: SCMCheckbox {
		readonly private Toggle ModData;

		public sealed override string Label => ModData.Label;
		public sealed override bool Enabled => ModData.Enabled;
		public sealed override bool IsChecked => ModData.IsOn;

		internal ConfigCheckbox(Toggle option) : this(option, 0, 0) { }

		internal ConfigCheckbox(Toggle option, int x, int y) : base(option.Label, option.IsOn, x, y, option.Enabled) {
			ModData = option;
		}
	}
}

