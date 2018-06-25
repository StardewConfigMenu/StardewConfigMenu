using System.Collections.Generic;
using StardewConfigFramework.Options;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components.DataBacked {
	using SelectionTuple = System.Tuple<string, string, string>;

	internal class ConfigDropdown: DropdownComponent {
		readonly private Selection ModData;

		public sealed override bool Enabled => (DropdownOptions.Count > 0) && ModData.Enabled;
		public sealed override string Label => ModData.Label;
		public sealed override int SelectedIndex => ModData.SelectionIndex;
		protected sealed override IList<SelectionTuple> DropdownOptions => ModData.Choices as IList<SelectionTuple>;

		public ConfigDropdown(Selection option, int width) : this(option, width, 0, 0) { }

		public ConfigDropdown(Selection option, int width, int x, int y) : base(option.Label, width, x, y) {
			ModData = option;
		}
	}
}
