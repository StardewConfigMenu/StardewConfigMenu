using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewConfigFramework;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Panel.Components
{
	internal delegate void ButtonPressed();

	internal class ButtonComponent: OptionComponent
	{

		internal event ButtonPressed ButtonPressed;

		private Rectangle setButtonSource => OptionsInputListener.setButtonSource;
		private Rectangle okButtonSource => Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1);
		private Rectangle clearButtonSource => Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1);
		private Rectangle doneButtonSource = new Rectangle(441, 411, 24, 13);
		private Rectangle giftButtonSource = new Rectangle(229, 410, 14, 14);

		protected Rectangle buttonSource
		{
			get {
				switch (this.ActionType)
				{
				case OptionActionType.DONE:
					return this.doneButtonSource;
				case OptionActionType.CLEAR:
					return this.clearButtonSource;
				case OptionActionType.OK:
					return this.okButtonSource;
				case OptionActionType.SET:
					return this.setButtonSource;
				case OptionActionType.GIFT:
					return this.giftButtonSource;
				default:
					return new Rectangle();
				}
			}
		}

		protected float buttonScale
		{
			get {
				switch (this.ActionType)
				{
				case OptionActionType.DONE:
					return (float) Game1.pixelZoom;
				case OptionActionType.CLEAR:
					return 1f;
				case OptionActionType.OK:
					return 1f;
				case OptionActionType.SET:
					return (float) Game1.pixelZoom;
				case OptionActionType.GIFT:
					return (float) Game1.pixelZoom;
				default:
					return (float) Game1.pixelZoom;
				}
			}
		}

		public virtual OptionActionType ActionType
		{
			get {
				return _ActionType;
			}
			set {
				_ActionType = value;
				this.bounds.Width = (int) buttonScale * this.buttonSource.Width;
				this.bounds.Height = (int) buttonScale * this.buttonSource.Height;
			}
		}

		protected OptionActionType _ActionType;

		//
		// Static Fields
		//

		//
		// Fields
		//

		internal ButtonComponent(string label, OptionActionType type, int x, int y, bool enabled = true) : base(label, enabled)
		{
			this._ActionType = type;

			this.bounds = new Rectangle(x, y, (int) buttonScale * this.buttonSource.Width, (int) buttonScale * this.buttonSource.Height);
		}

		internal ButtonComponent(string label, OptionActionType type, bool enabled = true) : base(label, enabled)
		{
			this._ActionType = type;

			this.bounds = new Rectangle(0, 0, (int) buttonScale * this.buttonSource.Width, (int) buttonScale * this.buttonSource.Height);
		}

		public override void receiveRightClick(int x, int y, bool playSound = true) { }

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			base.receiveLeftClick(x, y, playSound);

			if (this.bounds.Contains(x, y) && enabled && this.IsAvailableForSelection()) {
				this.ButtonPressed?.Invoke();
			}

		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);

			// draw button
			var labelSize = Game1.dialogueFont.MeasureString(this.label);
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.bounds.X), (float) (this.bounds.Y)), this.buttonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, this.buttonScale, false, 0.15f, -1, -1, 0.35f);

			Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float) (this.bounds.Right + Game1.pixelZoom * 4), (float) (this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);
		}
	}
}
