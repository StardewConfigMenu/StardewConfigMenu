using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {
	using ActionType = Action.ActionType;

	internal class ConfigButton: SCMButton {
		readonly Action ModData;

		public sealed override string Label => ModData.Label;
		public sealed override bool Enabled => ModData.Enabled;
		public sealed override ActionType ActionType => ModData.Type;

		internal ConfigButton(Action option) : this(option, 0, 0) { }

		internal ConfigButton(Action option, int x, int y) : base(option.Label, option.Type, x, y, option.Enabled) {
			ModData = option;
		}
	}
}
