using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components {

	internal class DropdownComponent: SCMControl {
		internal delegate void OptionSelectedEvent(int selected);
		internal event OptionSelectedEvent OptionSelected;

		protected ClickableTextureComponent DropdownBackground = StardewTile.DropDownBackground.ClickableTextureComponent(0, 0, OptionsDropDown.dropDownBGSource.Width, 0);
		protected ClickableTextureComponent DropdownButton = StardewTile.DropDownButton.ClickableTextureComponent(0, 0);
		protected ClickableTextureComponent Dropdown = StardewTile.DropDownBackground.ClickableTextureComponent(0, 0, OptionsDropDown.dropDownBGSource.Width, 11);

		private List<string> _DropdownOptions = new List<string>();
		protected virtual IReadOnlyList<string> DropdownOptions => _DropdownOptions;

		public sealed override int Width {
			get => Dropdown.bounds.Width + DropdownButton.bounds.Width;
			set {
				int width = Math.Max(value - DropdownButton.bounds.Width, 12);
				Dropdown.bounds.Width = width;
				DropdownBackground.bounds.Width = width;
			}
		}
		public sealed override int Height => Dropdown.bounds.Height;
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

		private int hoveredChoice = 0;

		public override bool Enabled {
			get => (DropdownOptions.Count > 0) && _enabled;
			protected set => _enabled = value;
		}

		internal override bool Visible {
			get => Dropdown.visible && DropdownBackground.visible && DropdownButton.visible;
			set {
				Dropdown.visible = value;
				DropdownBackground.visible = value;
				DropdownButton.visible = value;
			}
		}

		private int _SelectedIndex = 0;
		public virtual int SelectedIndex {
			get => _SelectedIndex;
			set {
				if (_DropdownOptions.Count == 0 && value == 0) {
				} else if (value >= _DropdownOptions.Count || value < 0) {
					throw new IndexOutOfRangeException("Selection is out of range of Choices");
				}

				if (_SelectedIndex == value)
					return;

				_SelectedIndex = value;
				OptionSelected?.Invoke(value);
			}
		}

		protected DropdownComponent(string label, int width, bool enabled = true) : this(label, width, 0, 0, enabled) { }

		public DropdownComponent(string label, int width, int x, int y, bool enabled = true) : base(label, enabled) {
			X = x;
			Y = y;
			Width = width;
		}

		public DropdownComponent(List<string> choices, string label, int width, bool enabled = true) : this(choices, label, width, 0, 0, enabled) { }

		public DropdownComponent(List<string> choices, string label, int width, int x, int y, bool enabled = true) : this(label, width, x, y, enabled) {
			_DropdownOptions = choices;
		}

		protected bool containsPoint(int x, int y) {
			return (Dropdown.containsPoint(x, y) || DropdownButton.containsPoint(x, y));
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			if (Enabled && containsPoint(x, y) && IsAvailableForSelection) {
				RegisterAsActiveComponent();
				hoveredChoice = 0;
				LeftClickHeld(x, y);
				if (playSound)
					Game1.playSound("shwip");
			}
		}

		public override void LeftClickHeld(int x, int y) {
			if (Enabled && IsActiveComponent && DropdownBackground.containsPoint(x, y)) {
				DropdownBackground.bounds.Y = Math.Min(DropdownBackground.bounds.Y, Game1.viewport.Height - DropdownBackground.bounds.Height);
				hoveredChoice = (int) Math.Max(Math.Min((float) (y - DropdownBackground.bounds.Y) / (float) Dropdown.bounds.Height, (float) (DropdownOptions.Count - 1)), 0f);
			}
		}

		public override void ReleaseLeftClick(int x, int y) {
			if (IsActiveComponent) {
				UnregisterAsActiveComponent();

				if (DropdownBackground.containsPoint(x, y)) {
					if (Enabled && DropdownOptions.Count > 0) {
						SelectHoveredOption();
					}
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
				b.DrawString(Game1.smallFont, DropdownOptions[SelectedIndex], new Vector2((float) (DropdownBackground.bounds.X + Game1.pixelZoom), 0f), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);

				int drawnPosition = 1;
				for (int i = 0; i < DropdownOptions.Count; i++) {
					if (i == SelectedIndex)
						continue;

					if (drawnPosition == hoveredChoice && DropdownBackground.containsPoint(Game1.getMouseX(), Game1.getMouseY())) {
						b.Draw(Game1.staminaRect, new Rectangle(Dropdown.bounds.X, Dropdown.bounds.Y + drawnPosition * Dropdown.bounds.Height, Dropdown.bounds.Width, Dropdown.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
					}
					b.DrawString(Game1.smallFont, DropdownOptions[i], new Vector2((float) (DropdownBackground.bounds.X + Game1.pixelZoom), (float) (DropdownBackground.bounds.Y + Game1.pixelZoom * 2 + Dropdown.bounds.Height * drawnPosition)), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
					drawnPosition++;
				}
			} else {
				DropdownButton.draw(b, Color.White * buttonAlpha, 0.88f);
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, Dropdown.bounds.X, Dropdown.bounds.Y, Dropdown.bounds.Width, Dropdown.bounds.Height, Color.White * buttonAlpha, (float) Game1.pixelZoom, false);
				b.DrawString(Game1.smallFont, (SelectedIndex >= DropdownOptions.Count) ? string.Empty : DropdownOptions[SelectedIndex], new Vector2((float) (Dropdown.bounds.X + Game1.pixelZoom), (float) (Dropdown.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
			}
		}
	}
}
