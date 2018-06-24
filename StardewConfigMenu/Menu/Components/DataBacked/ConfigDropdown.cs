using System.Collections.Generic;
using System.Linq;
using StardewConfigFramework.Options;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components.DataBacked {

	internal class ConfigDropdown: DropdownComponent {
		readonly private Selection ModData;

		public override bool Enabled {
			get => (DropdownOptions.Count > 0) && ModData.Enabled;
			protected set => ModData.Enabled = value;
		}

		public override string Label { get => ModData.Label; protected set => ModData.Label = value; }

		public override int SelectedIndex { get => ModData.SelectionIndex; set => ModData.SelectionIndex = value; }

		protected override IReadOnlyList<string> DropdownOptions => ModData.GetLabels();

		public ConfigDropdown(Selection option, int width) : this(option, width, 0, 0) { }

		public ConfigDropdown(Selection option, int width, int x, int y) : base(option.Label, width, x, y) {
			ModData = option;
		}

		public override void Draw(SpriteBatch b) {
			base.Draw(b);

			if (ModData.Choices.Count == 0)
				return;

			if (!IsActiveComponent && Dropdown.containsPoint(Game1.getMouseX(), Game1.getMouseY())) {
				if (ModData.SelectedChoice.HoverText != null) {
					string optionDescription = Utilities.GetWordWrappedString(ModData.SelectedChoice.HoverText);
					IClickableMenu.drawHoverText(b, optionDescription, Game1.smallFont);
				}
			}
		}
	}
}
