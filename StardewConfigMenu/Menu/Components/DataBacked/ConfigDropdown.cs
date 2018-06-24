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

		public override int SelectionIndex {
			get {
				if (DropdownOptions.Count > 0 && _DropdownDisplayOptions[0] != DropdownOptions[this.ModData.SelectionIndex]) {
					_DropdownDisplayOptions.Insert(0, DropdownOptions[this.ModData.SelectionIndex]);
					_DropdownDisplayOptions.RemoveAt(this.ModData.SelectionIndex + 1);
				}

				return this.ModData.SelectionIndex;
			}
			set {
				if (SelectionIndex == value)
					return;

				_DropdownDisplayOptions.Insert(0, DropdownOptions[value]);
				_DropdownDisplayOptions.RemoveAt(value + 1);
				this.ModData.SelectionIndex = value;
			}
		}

		protected override IReadOnlyList<string> DropdownOptions {
			get {
				if (this.ModData == null) // for base intialization
					return new List<string>();
				else
					return ModData.GetLabels();
			}
		}

		protected override List<string> DropdownDisplayOptions {
			get {
				if (ModData.Choices.Count == 0) {
					_DropdownDisplayOptions.Clear();
				} else {
					var options = DropdownOptions;
					var toRemove = _DropdownDisplayOptions.Except(options).ToList();
					var toAdd = options.Except(_DropdownDisplayOptions).ToList();

					_DropdownDisplayOptions.RemoveAll(x => toRemove.Contains(x));
					_DropdownDisplayOptions.AddRange(toAdd);
				}

				DropdownBackground.bounds.Height = this.Dropdown.bounds.Height * this.ModData.Choices.Count;
				return _DropdownDisplayOptions;
			}
		}

		public ConfigDropdown(Selection option, int width) : this(option, width, 0, 0) { }

		public ConfigDropdown(Selection option, int width, int x, int y) : base(option.Label, width, x, y) {
			this.ModData = option;
		}

		protected override void SelectDisplayedOption(int DisplayedSelection) {
			if (this.ModData.Choices.Count == 0)
				return;

			var selected = DropdownDisplayOptions[DisplayedSelection];
			ModData.SelectionIndex = this.ModData.IndexOfLabel(selected);
			base.SelectDisplayedOption(DisplayedSelection);
		}

		public override void Draw(SpriteBatch b) {
			base.Draw(b);

			if (this.ModData.Choices.Count == 0)
				return;

			if (!this.IsActiveComponent && (Game1.getMouseX() > this.X) && (Game1.getMouseX() < this.Width + this.X) && (Game1.getMouseY() > this.Y) && (Game1.getMouseY() < this.Height + this.Y)) {
				if (this.ModData.SelectedChoice.HoverText != null) {
					string optionDescription = Utilities.GetWordWrappedString(ModData.SelectedChoice.HoverText);
					IClickableMenu.drawHoverText(b, optionDescription, Game1.smallFont);
				}
			}
		}
	}
}
