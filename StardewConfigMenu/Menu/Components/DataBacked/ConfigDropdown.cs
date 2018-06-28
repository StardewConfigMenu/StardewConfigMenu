using System.Collections.Generic;
using StardewConfigFramework.Options;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components.DataBacked {
	using SelectionTuple = System.Tuple<string, string, string>;

	internal class ConfigDropdown: DropdownComponent {
		readonly private ISelection ModData;

		public sealed override bool Enabled => (DropdownOptions.Count > 0) && ModData.Enabled;
		public sealed override string Label => ModData.Label;
		public sealed override int SelectedIndex => ModData.SelectedIndex;
		protected sealed override IList<SelectionTuple> DropdownOptions => ModData as IList<SelectionTuple>;

		public ConfigDropdown(ISelection option, int width) : this(option, width, 0, 0) { }

		public ConfigDropdown(ISelection option, int width, int x, int y) : base(option.Label, width, x, y) {
			ModData = option;
		}
	}
}
