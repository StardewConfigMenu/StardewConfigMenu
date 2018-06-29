using System.Collections.Generic;
using StardewConfigFramework.Options;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using Microsoft.Xna.Framework;

namespace StardewConfigMenu.Components {
	using SelectionTuple = System.Tuple<string, string, string>;

	sealed class ConfigDropdown: SCMControl {
		readonly private ISelection ModData;

		private int hoveredChoice = 0;

		readonly ClickableTextureComponent DropdownBackground = StardewTile.DropDownBackground.ClickableTextureComponent(0, 0, OptionsDropDown.dropDownBGSource.Width, 0);
		readonly ClickableTextureComponent DropdownButton = StardewTile.DropDownButton.ClickableTextureComponent(0, 0);
		readonly ClickableTextureComponent Dropdown = StardewTile.DropDownBackground.ClickableTextureComponent(0, 0, OptionsDropDown.dropDownBGSource.Width, 11);

		public sealed override int X {
			get => Dropdown.bounds.X;
			set {
				Dropdown.bounds.X = value;
				DropdownBackground.bounds.X = value;
				DropdownButton.bounds.X = Dropdown.bounds.Right;
			}
		}
		public sealed override int Y {
			get => Dropdown.bounds.Y;
			set {
				Dropdown.bounds.Y = value;
				DropdownBackground.bounds.Y = value;
				DropdownButton.bounds.Y = value;
			}
		}
		public sealed override int Height => Dropdown.bounds.Height;
		public sealed override int Width {
			get => Dropdown.bounds.Width + DropdownButton.bounds.Width;
			set {
				int width = Math.Max(value - DropdownButton.bounds.Width, 12);
				Dropdown.bounds.Width = width;
				DropdownBackground.bounds.Width = width;
			}
		}

		public override string Label => ModData.Label;
		public override bool Enabled => (DropdownOptions.Count > 0) && ModData.Enabled;
		internal override bool Visible {
			get => Dropdown.visible && DropdownBackground.visible && DropdownButton.visible;
			set {
				Dropdown.visible = value;
				DropdownBackground.visible = value;
				DropdownButton.visible = value;
			}
		}

		public int SelectedIndex { get => ModData.SelectedIndex; set => ModData.SelectedIndex = value; }
		private IList<SelectionTuple> DropdownOptions => ModData.Choices as IList<SelectionTuple>;

		public ConfigDropdown(ISelection option, int width) : this(option, width, 0, 0) { }

		public ConfigDropdown(ISelection option, int width, int x, int y) : base(option.Label, option.Enabled) {
			ModData = option;
			X = x;
			Y = y;
			Width = width;
		}

		private bool containsPoint(int x, int y) {
			return (Dropdown.containsPoint(x, y) || DropdownButton.containsPoint(x, y));
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			if (!Enabled || !IsAvailableForSelection)
				return;

			if (containsPoint(x, y)) {
				RegisterAsActiveComponent();
				hoveredChoice = 0;
				LeftClickHeld(x, y);
				if (playSound)
					Game1.playSound("shwip");
			}
		}

		public override void LeftClickHeld(int x, int y) {
			if (!Enabled || !IsActiveComponent)
				return;

			if (DropdownBackground.containsPoint(x, y)) {
				DropdownBackground.bounds.Y = Math.Min(DropdownBackground.bounds.Y, Game1.viewport.Height - DropdownBackground.bounds.Height);
				hoveredChoice = (int) Math.Max(Math.Min((float) (y - DropdownBackground.bounds.Y) / (float) Dropdown.bounds.Height, (float) (DropdownOptions.Count - 1)), 0f);
			}
		}

		public override void ReleaseLeftClick(int x, int y) {
			if (!IsActiveComponent)
				return;

			UnregisterAsActiveComponent();

			if (DropdownBackground.containsPoint(x, y)) {
				if (Enabled && DropdownOptions.Count > 0) {
					SelectHoveredOption();
				}
			}
		}

		private void SelectHoveredOption() {
			if (hoveredChoice == 0)
				return; // 0 is the already selected option

			SelectedIndex = hoveredChoice > SelectedIndex ? hoveredChoice : hoveredChoice - 1;
		}

		public override void Draw(SpriteBatch b) {

			float buttonAlpha = (Enabled) ? 1f : 0.33f;
			var labelSize = Game1.dialogueFont.MeasureString(Label);

			// Draw Label
			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (DropdownButton.bounds.Right + Game1.pixelZoom * 2), (float) (Dropdown.bounds.Y + ((Dropdown.bounds.Height - labelSize.Y) / 2))), Game1.textColor * buttonAlpha, 1f, 0.1f, -1, -1, 1f, 3);

			if (IsActiveComponent) {
				DropdownBackground.bounds.Height = Dropdown.bounds.Height * DropdownOptions.Count;

				DropdownButton.draw(b);

				// Draw Background of dropdown
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, DropdownBackground.bounds.X, DropdownBackground.bounds.Y, DropdownBackground.bounds.Width, DropdownBackground.bounds.Height, Color.White * buttonAlpha, (float) Game1.pixelZoom, false);

				if (0 == hoveredChoice && DropdownBackground.containsPoint(Game1.getMouseX(), Game1.getMouseY())) {
					b.Draw(Game1.staminaRect, new Rectangle(Dropdown.bounds.X, Dropdown.bounds.Y + SelectedIndex * Dropdown.bounds.Height, Dropdown.bounds.Width, Dropdown.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
				}
				b.DrawString(Game1.smallFont, DropdownOptions[SelectedIndex].Item2, new Vector2((float) (DropdownBackground.bounds.X + Game1.pixelZoom), 0f), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);

				int drawnPosition = 1;
				for (int i = 0; i < DropdownOptions.Count; i++) {
					if (i == SelectedIndex)
						continue;

					if (drawnPosition == hoveredChoice && DropdownBackground.containsPoint(Game1.getMouseX(), Game1.getMouseY())) {
						b.Draw(Game1.staminaRect, new Rectangle(Dropdown.bounds.X, Dropdown.bounds.Y + drawnPosition * Dropdown.bounds.Height, Dropdown.bounds.Width, Dropdown.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
					}
					b.DrawString(Game1.smallFont, DropdownOptions[i].Item2, new Vector2((float) (DropdownBackground.bounds.X + Game1.pixelZoom), (float) (DropdownBackground.bounds.Y + Game1.pixelZoom * 2 + Dropdown.bounds.Height * drawnPosition)), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
					drawnPosition++;
				}
			} else {
				DropdownButton.draw(b, Color.White * buttonAlpha, 0.88f);
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, Dropdown.bounds.X, Dropdown.bounds.Y, Dropdown.bounds.Width, Dropdown.bounds.Height, Color.White * buttonAlpha, (float) Game1.pixelZoom, false);
				b.DrawString(Game1.smallFont, (SelectedIndex >= DropdownOptions.Count) ? string.Empty : DropdownOptions[SelectedIndex].Item2, new Vector2((float) (Dropdown.bounds.X + Game1.pixelZoom), (float) (Dropdown.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);

				if (Dropdown.containsPoint(Game1.getMouseX(), Game1.getMouseY())) {
					if (DropdownOptions[SelectedIndex].Item3 != null) {
						string optionDescription = Utilities.GetWordWrappedString(DropdownOptions[SelectedIndex].Item3);
						IClickableMenu.drawHoverText(b, optionDescription, Game1.smallFont);
					}
				}
			}
		}
	}
}
