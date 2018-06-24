using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewConfigFramework.Options;


namespace StardewConfigMenu.Components.DataBacked {
	internal class ConfigCategoryLabel: SCMCategoryLabel {

		private CategoryLabel ModData;

		public override string Label { get => ModData.Label; protected set => ModData.Label = value; }

		public ConfigCategoryLabel(CategoryLabel option) : this(option, 0, 0) { }

		public ConfigCategoryLabel(CategoryLabel option, int x, int y) : base(option.Label, x, y) {
			ModData = option;
		}
	}
}
