using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewConfigFramework.Options;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Components {
	using ActionType = Action.ActionType;
	internal delegate void ButtonPressed();

	internal class ButtonComponent: OptionComponent {

		internal event ButtonPressed ButtonPressed;

		protected StardewTile GetButtonTile() {
			switch (this.ActionType) {
				case ActionType.DONE:
					return StardewTile.DoneButton;
				case ActionType.CLEAR:
					return StardewTile.ClearButton;
				case ActionType.OK:
					return StardewTile.OKButton;
				case ActionType.SET:
					return StardewTile.SetButton;
				case ActionType.GIFT:
					return StardewTile.GiftButton;
				default:
					return StardewTile.OKButton;
			}
		}

		public virtual ActionType ActionType {
			get => _ActionType;
			set {
				_ActionType = value;
			}
		}

		protected ActionType _ActionType;

		//
		// Static Fields
		//

		//
		// Fields
		//

		public override int Width => button.bounds.Width;
		public override int Height => button.bounds.Height;
		public override int X => button.bounds.X;
		public override int Y => button.bounds.Y;

		protected ClickableTextureComponent button;

		internal ButtonComponent(string label, ActionType type, int x, int y, bool enabled = true) : base(label, enabled) {
			this._ActionType = type;

			button = GetButtonTile().ClickableTextureComponent(x, y);
			button.drawShadow = true;
		}

		internal ButtonComponent(string label, ActionType type, bool enabled = true) : this(label, type, 0, 0, enabled) { }

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			base.receiveLeftClick(x, y, playSound);

			if (this.button.containsPoint(x, y) && enabled && this.IsAvailableForSelection()) {
				this.ButtonPressed?.Invoke();
			}

		}

		private ActionType PreviousActionType = ActionType.OK;

		public void updateButton() {
			if (PreviousActionType != ActionType) {
				button = GetButtonTile().ClickableTextureComponent(button.bounds.X, button.bounds.Y);
				button.drawShadow = true;
				PreviousActionType = ActionType;
			}
		}

		public override void draw(SpriteBatch b, int x, int y) {
			button.bounds.X = x;
			button.bounds.Y = y;
			this.draw(b);
		}

		public override void draw(SpriteBatch b) {
			base.draw(b);

			// draw button
			var labelSize = Game1.dialogueFont.MeasureString(this.label);

			updateButton();
			button.draw(b, Color.White * ((this.enabled) ? 1f : 0.33f), 0.88f);
			//Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.bounds.X), (float) (this.bounds.Y)), this.buttonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, this.buttonScale, false, 0.15f, -1, -1, 0.35f);

			Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float) (this.button.bounds.Right + Game1.pixelZoom * 4), (float) (this.button.bounds.Y + ((this.button.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
