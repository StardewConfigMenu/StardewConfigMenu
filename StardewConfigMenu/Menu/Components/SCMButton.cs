using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewConfigFramework.Options;

namespace StardewConfigMenu.Components {

	internal class SCMButton: SCMControl {
		internal delegate void ButtonPressedEvent();
		internal event ButtonPressedEvent ButtonPressed;

		private ButtonType _ButtonType;
		private ButtonType PreviousButtonType = ButtonType.OK;
		public virtual ButtonType ButtonType => _ButtonType;
		internal override bool Visible => Button.visible;

		protected ClickableTextureComponent Button;
		public override int Width => Button.bounds.Width;
		public override int Height => Button.bounds.Height;
		public override int X => Button.bounds.X;
		public override int Y => Button.bounds.Y;

		internal SCMButton(string label, ButtonType type, bool enabled = true) : this(label, type, 0, 0, enabled) { }

		internal SCMButton(string label, ButtonType type, int x, int y, bool enabled = true) : base(label, enabled) {
			_ButtonType = type;

			Button = GetButtonTile().ClickableTextureComponent(x, y);
			Button.drawShadow = true;
		}

		protected StardewTile GetButtonTile() {
			switch (ButtonType) {
				case ButtonType.DONE:
					return StardewTile.DoneButton;
				case ButtonType.CLEAR:
					return StardewTile.ClearButton;
				case ButtonType.OK:
					return StardewTile.OKButton;
				case ButtonType.SET:
					return StardewTile.SetButton;
				case ButtonType.GIFT:
					return StardewTile.GiftButton;
				default:
					return StardewTile.OKButton;
			}
		}

		public override void ReceiveLeftClick(int x, int y, bool playSound = true) {
			base.ReceiveLeftClick(x, y, playSound);

			if (Button.containsPoint(x, y) && Enabled && IsAvailableForSelection) {
				if (playSound)
					Game1.playSound("breathin");
				ButtonPressed?.Invoke();
			}
		}

		private void CheckForButtonUpdate() {
			if (PreviousButtonType != ButtonType) {
				Button = GetButtonTile().ClickableTextureComponent(Button.bounds.X, Button.bounds.Y);
				Button.drawShadow = true;
				PreviousButtonType = ButtonType;
			}
		}

		public override void Draw(SpriteBatch b) {
			base.Draw(b);

			// draw button
			var labelSize = Game1.dialogueFont.MeasureString(Label);

			CheckForButtonUpdate();
			Button.draw(b, Color.White * ((Enabled) ? 1f : 0.33f), 0.88f);

			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (Button.bounds.Right + Game1.pixelZoom * 4), (float) (Button.bounds.Y + ((Button.bounds.Height - labelSize.Y) / 2))), Enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
