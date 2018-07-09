using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewConfigMenu.UI;
using StardewValley;

namespace StardewConfigMenu.Components {
	sealed class ConfigTextfield: SCMControl {
		readonly IConfigString ModData;
		readonly SCMSprite textfieldBackgroundLeft = SCMSprite.TextfieldBackgroundLeft;
		SCMSprite textfieldBackgroundMiddle = SCMSprite.TextfieldBackgroundMiddle;
		readonly SCMSprite textfieldBackgroundRight = SCMSprite.TextfieldBackgroundRight;

		public override int X {
			get => textfieldBackgroundLeft.X;
			set {
				if (textfieldBackgroundLeft.X == value)
					return;

				textfieldBackgroundLeft.X = value;
				textfieldBackgroundMiddle.X = textfieldBackgroundLeft.Bounds.Right;
				textfieldBackgroundRight.X = textfieldBackgroundMiddle.Bounds.Right;
			}
		}
		public override int Y {
			get => textfieldBackgroundLeft.Y;
			set {
				if (textfieldBackgroundLeft.Y == value)
					return;

				textfieldBackgroundLeft.Y = value;
				textfieldBackgroundMiddle.Y = value;
				textfieldBackgroundRight.Y = value;
			}
		}
		public override int Height {
			get => textfieldBackgroundLeft.Height;
			set {
				textfieldBackgroundLeft.Height = value;
				textfieldBackgroundRight.Height = value;
				textfieldBackgroundMiddle.Height = value;
			}
		}
		public override int Width => textfieldBackgroundRight.Bounds.Right - textfieldBackgroundLeft.X;

		public ConfigTextfield(IConfigString option) : base(option.Label, option.Enabled) {
			ModData = option;
			textfieldBackgroundMiddle.Width = 60 * Game1.pixelZoom;
			Height = 15 * Game1.pixelZoom;
		}

		public override void Draw(SpriteBatch b) {
			textfieldBackgroundLeft.DrawStretched(b);
			textfieldBackgroundMiddle.DrawStretched(b);
			textfieldBackgroundRight.DrawStretched(b);
		}
	}
}
