using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigCategoryLabel: SCMCategoryLabel {
		private ICategoryLabel ModData;

		public sealed override string Label => ModData.Label;

		public ConfigCategoryLabel(ICategoryLabel option) : this(option, 0, 0) { }

		public ConfigCategoryLabel(ICategoryLabel option, int x, int y) : base(option.Label, x, y) {
			ModData = option;
		}
	}
}
