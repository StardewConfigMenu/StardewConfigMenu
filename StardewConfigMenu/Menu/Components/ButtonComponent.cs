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

	internal class ButtonComponent: OptionComponent {
		internal delegate void ButtonPressedEvent();
		internal event ButtonPressedEvent ButtonPressed;

		private ActionType _ActionType;
		private ActionType PreviousActionType = ActionType.OK;
		public virtual ActionType ActionType {
			get => _ActionType;
			set {
				_ActionType = value;
			}
		}

		protected ClickableTextureComponent Button;
		public override int Width => Button.bounds.Width;
		public override int Height => Button.bounds.Height;
		public override int X => Button.bounds.X;
		public override int Y => Button.bounds.Y;

		internal ButtonComponent(string label, ActionType type, int x, int y, bool enabled = true) : base(label, enabled) {
			this._ActionType = type;

			Button = GetButtonTile().ClickableTextureComponent(x, y);
			Button.drawShadow = true;
		}

		internal ButtonComponent(string label, ActionType type, bool enabled = true) : this(label, type, 0, 0, enabled) { }

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

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			base.receiveLeftClick(x, y, playSound);

			if (this.Button.containsPoint(x, y) && Enabled && this.IsAvailableForSelection()) {
				this.ButtonPressed?.Invoke();
			}

		}

		private void CheckForButtonUpdate() {
			if (PreviousActionType != ActionType) {
				Button = GetButtonTile().ClickableTextureComponent(Button.bounds.X, Button.bounds.Y);
				Button.drawShadow = true;
				PreviousActionType = ActionType;
			}
		}

		public override void draw(SpriteBatch b, int x, int y) {
			Button.bounds.X = x;
			Button.bounds.Y = y;
			this.draw(b);
		}

		public override void draw(SpriteBatch b) {
			base.draw(b);

			// draw button
			var labelSize = Game1.dialogueFont.MeasureString(this.Label);

			CheckForButtonUpdate();
			Button.draw(b, Color.White * ((this.Enabled) ? 1f : 0.33f), 0.88f);
			//Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.bounds.X), (float) (this.bounds.Y)), this.buttonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, this.buttonScale, false, 0.15f, -1, -1, 0.35f);

			Utility.drawTextWithShadow(b, this.Label, Game1.dialogueFont, new Vector2((float) (this.Button.bounds.Right + Game1.pixelZoom * 4), (float) (this.Button.bounds.Y + ((this.Button.bounds.Height - labelSize.Y) / 2))), this.Enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
