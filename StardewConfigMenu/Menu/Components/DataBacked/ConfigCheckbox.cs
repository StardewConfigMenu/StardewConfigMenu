using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigCheckbox: SCMCheckbox {
		readonly private Toggle ModData;

		public override string Label { get => ModData.Label; protected set => ModData.Label = value; }
		public override bool Enabled { get => ModData.Enabled; protected set => ModData.Enabled = value; }
		public override bool IsChecked { get => ModData.IsOn; protected set => ModData.IsOn = value; }

		internal ConfigCheckbox(Toggle option) : this(option, 0, 0) { }

		internal ConfigCheckbox(Toggle option, int x, int y) : base(option.Label, option.IsOn, x, y, option.Enabled) {
			ModData = option;
		}
	}
}

