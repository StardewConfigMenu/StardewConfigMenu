using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigButton: SCMButton {
		readonly IAction ModData;

		public sealed override string Label => ModData.Label;
		public sealed override bool Enabled => ModData.Enabled;
		public sealed override ButtonType ButtonType => ModData.ButtonType;

		internal ConfigButton(IAction option) : this(option, 0, 0) { }

		internal ConfigButton(IAction option, int x, int y) : base(option.Label, option.ButtonType, x, y, option.Enabled) {
			ModData = option;
		}
	}
}
