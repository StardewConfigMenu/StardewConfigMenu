using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewConfigFramework;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Components {

	internal class DropdownComponent: SCMControl {
		internal delegate void OptionSelectedEvent(int selected);
		internal event OptionSelectedEvent OptionSelected;

		protected ClickableTextureComponent DropdownBackground = StardewTile.DropDownBackground.ClickableTextureComponent(0, 0, OptionsDropDown.dropDownBGSource.Width, 0);
		protected ClickableTextureComponent DropdownButton = StardewTile.DropDownButton.ClickableTextureComponent(0, 0);
		protected ClickableTextureComponent Dropdown = StardewTile.DropDownBackground.ClickableTextureComponent(0, 0, OptionsDropDown.dropDownBGSource.Width, 11);

		public override int Width => Dropdown.bounds.Width + DropdownButton.bounds.Width;
		public override int Height => Dropdown.bounds.Height;
		public override int X => Dropdown.bounds.X;
		public override int Y => Dropdown.bounds.Y;

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

		public virtual int SelectionIndex {
			get {
				if (DropdownOptions.Count == 0)
					return -1;

				for (int i = 0; i < DropdownOptions.Count; i++) {
					if (DropdownDisplayOptions[0] == DropdownOptions[i])
						return i;
				}
				return -1;
			}
			set {
				if (SelectionIndex == value)
					return;

				var index = DropdownDisplayOptions.IndexOf(DropdownOptions[value]);

				DropdownDisplayOptions.Insert(0, DropdownOptions[value]);
				DropdownDisplayOptions.RemoveAt(index + 1);
				this.OptionSelected?.Invoke(value);
			}
		}

		private int hoveredChoice = 0;

		// Original List
		private List<string> _DropdownOptions = new List<string>();

		protected virtual IReadOnlyList<string> DropdownOptions {
			get { return _DropdownOptions; }
		}

		// List where order can be changed
		protected List<string> _DropdownDisplayOptions = new List<string>();

		protected virtual List<string> DropdownDisplayOptions {
			get {
				if (_DropdownOptions.Count == 0) {
					_DropdownDisplayOptions.Clear();
				} else {
					var options = DropdownOptions;
					var toRemove = _DropdownDisplayOptions.Except(options).ToList();
					var toAdd = options.Except(_DropdownDisplayOptions).ToList();

					_DropdownDisplayOptions.RemoveAll(x => toRemove.Contains(x));
					_DropdownDisplayOptions.AddRange(toAdd);
				}

				DropdownBackground.bounds.Height = this.Dropdown.bounds.Height * DropdownOptions.Count;
				return _DropdownDisplayOptions;
			}
		}

		protected virtual void SelectDisplayedOption(int DisplayedSelection) {
			if (DisplayedSelection == 0)
				return;
			var selected = DropdownDisplayOptions[DisplayedSelection];
			DropdownDisplayOptions.Insert(0, selected);
			DropdownDisplayOptions.RemoveAt(DisplayedSelection + 1);
			this.OptionSelected?.Invoke(SelectionIndex);
		}

		//
		// Constructors
		//

		public DropdownComponent(string label, int width, int x, int y, bool enabled = true) : base(label, x, y, enabled) {
			updateLocation(x, y, width);

			//this.Dropdown.bounds = new Rectangle(x, y, width + Game1.pixelZoom * 12, 11 * Game1.pixelZoom);
			//this.DropdownBounds = new Rectangle(this.bounds.X, this.bounds.Y, width, this.bounds.Height * this.DropdownOptions.Count);
		}

		public DropdownComponent(List<string> choices, string label, int width, int x, int y, bool enabled = true) : this(label, width, x, y, enabled) {
			this._DropdownOptions.AddRange(choices);
		}


		// These contructors requires Draw(b,x,y) to move the object from origin
		public DropdownComponent(List<string> choices, string label, int width, bool enabled = true) : this(choices, label, width, 0, 0, enabled) { }

		protected DropdownComponent(string label, int width, bool enabled = true) : this(label, width, 0, 0, enabled) { }

		//
		// Methods
		//

		protected void updateLocation(int x, int y, int width) {
			this.Dropdown.bounds.X = x;
			this.Dropdown.bounds.Y = y;
			this.Dropdown.bounds.Width = width;

			this.DropdownButton.bounds.X = this.Dropdown.bounds.Right;
			this.DropdownButton.bounds.Y = this.Dropdown.bounds.Y;

			this.DropdownBackground.bounds.X = x;
			this.DropdownBackground.bounds.Y = y;
			this.DropdownBackground.bounds.Width = width;
		}

		public override void Draw(SpriteBatch b, int x, int y) {
			updateLocation(x, y, this.Dropdown.bounds.Width);
			this.Draw(b);
		}

		public override void Draw(SpriteBatch b) {
			base.Draw(b);

			DropdownBackground.bounds.Height = this.Dropdown.bounds.Height * DropdownDisplayOptions.Count;

			float buttonAlpha = (this.Enabled) ? 1f : 0.33f;

			var labelSize = Game1.dialogueFont.MeasureString(this.Label);

			// Draw Label
			Utility.drawTextWithShadow(b, this.Label, Game1.dialogueFont, new Vector2((float) (this.DropdownButton.bounds.Right + Game1.pixelZoom * 2), (float) (this.Dropdown.bounds.Y + ((this.Dropdown.bounds.Height - labelSize.Y) / 2))), this.Enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

			// If menu is being clicked, and no other components are in use (to prevent click overlap of the dropdown)
			if (this.IsActiveComponent) {
				// Draw Background of dropdown
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, this.DropdownBackground.bounds.X, this.DropdownBackground.bounds.Y, this.DropdownBackground.bounds.Width, this.DropdownBackground.bounds.Height, Color.White * buttonAlpha, (float) Game1.pixelZoom, false);
				//DropdownBackground.draw(b);

				for (int i = 0; i < this.DropdownDisplayOptions.Count; i++) {
					if (i == this.hoveredChoice && DropdownBackground.containsPoint(Game1.getMouseX(), Game1.getMouseY())) {
						b.Draw(Game1.staminaRect, new Rectangle(this.Dropdown.bounds.X, this.Dropdown.bounds.Y + i * this.Dropdown.bounds.Height, this.Dropdown.bounds.Width, this.Dropdown.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
					}
					b.DrawString(Game1.smallFont, this.DropdownDisplayOptions[i], new Vector2((float) (this.DropdownBackground.bounds.X + Game1.pixelZoom), (float) (this.DropdownBackground.bounds.Y + Game1.pixelZoom * 2 + this.Dropdown.bounds.Height * i)), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
				}
				DropdownButton.draw(b);
				//b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float) (this.bounds.Y)), new Rectangle?(OptionsDropdown.DropdownButtonSource), Color.Wheat * scale, 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.981f);
			} else {
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, this.Dropdown.bounds.X, this.Dropdown.bounds.Y, this.Dropdown.bounds.Width, this.Dropdown.bounds.Height, Color.White * buttonAlpha, (float) Game1.pixelZoom, false);
				//Dropdown.draw(b);

				DropdownButton.draw(b, Color.White * buttonAlpha, 0.88f);

				b.DrawString(Game1.smallFont, (this.SelectionIndex >= this.DropdownDisplayOptions.Count || this.SelectionIndex < 0) ? string.Empty : this.DropdownDisplayOptions[0], new Vector2((float) (this.Dropdown.bounds.X + Game1.pixelZoom), (float) (this.Dropdown.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor * buttonAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
				//b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float) (this.bounds.Y)), new Rectangle?(OptionsDropdown.DropdownButtonSource), Color.White * scale, 0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
			}
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			//base.receiveLeftClick(x, y, playSound);

			if (this.Enabled && (this.Dropdown.containsPoint(x, y) || this.DropdownButton.containsPoint(x, y)) && this.IsAvailableForSelection) {
				this.RegisterAsActiveComponent();
				this.hoveredChoice = 0;
				this.LeftClickHeld(x, y);
				Game1.playSound("shwip");
			}
		}

		public override void LeftClickHeld(int x, int y) {
			base.LeftClickHeld(x, y);

			if (this.Enabled && this.IsActiveComponent && this.DropdownBackground.containsPoint(x, y)) {
				this.DropdownBackground.bounds.Y = Math.Min(this.DropdownBackground.bounds.Y, Game1.viewport.Height - this.DropdownBackground.bounds.Height);
				this.hoveredChoice = (int) Math.Max(Math.Min((float) (y - this.DropdownBackground.bounds.Y) / (float) this.Dropdown.bounds.Height, (float) (this.DropdownOptions.Count - 1)), 0f);
			}
		}

		public override void ReleaseLeftClick(int x, int y) {
			base.ReleaseLeftClick(x, y);

			if (this.IsActiveComponent) {
				this.UnregisterAsActiveComponent();

				if (this.DropdownBackground.containsPoint(x, y)) {
					if (this.Enabled && this.DropdownOptions.Count > 0) {
						this.SelectDisplayedOption(this.hoveredChoice);
					}

				}
			}
		}
	}
}

