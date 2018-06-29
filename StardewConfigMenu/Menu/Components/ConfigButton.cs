using StardewConfigFramework.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace StardewConfigMenu.Components {

	sealed class ConfigButton: SCMControl {
		private readonly IAction ModData;

		private ClickableTextureComponent Button;
		public sealed override int X { get => Button.bounds.X; set => Button.bounds.X = value; }
		public sealed override int Y { get => Button.bounds.Y; set => Button.bounds.Y = value; }
		public sealed override int Width => Button.bounds.Width;
		public sealed override int Height => Button.bounds.Height;

		public sealed override string Label { get => ModData.Label; }
		public sealed override bool Enabled { get => ModData.Enabled; }
		internal sealed override bool Visible { get => Button.visible; set => Button.visible = value; }
		public ButtonType ButtonType => ModData.ButtonType;
		private ButtonType PreviousButtonType = ButtonType.OK;

		internal ConfigButton(IAction option) : this(option, 0, 0) { }

		internal ConfigButton(IAction option, int x, int y) : base(option.Label, option.Enabled) {
			ModData = option;

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
			if (!Enabled || !IsAvailableForSelection)
				return;

			if (Button.containsPoint(x, y)) {
				if (playSound)
					Game1.playSound("breathin");
				ModData.Trigger();
			}
		}

		private void CheckForButtonUpdate() {
			if (PreviousButtonType == ButtonType)
				return;

			var newButton = GetButtonTile().ClickableTextureComponent(X, Y);
			newButton.visible = Button.visible;
			Button = newButton;
			Button.drawShadow = true;
			PreviousButtonType = ButtonType;
		}

		public override void Draw(SpriteBatch b) {
			var labelSize = Game1.dialogueFont.MeasureString(Label);
			var colorAlpha = (Enabled) ? 1f : 0.33f;

			CheckForButtonUpdate();
			Button.draw(b, Color.White * colorAlpha, 0.88f);

			Utility.drawTextWithShadow(b, Label, Game1.dialogueFont, new Vector2((float) (Button.bounds.Right + Game1.pixelZoom * 4), (float) (Y + ((Height - labelSize.Y) / 2))), Game1.textColor * colorAlpha, 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
