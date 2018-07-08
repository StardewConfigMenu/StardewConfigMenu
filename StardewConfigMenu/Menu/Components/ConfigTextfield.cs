using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework.Options;
using StardewConfigMenu.UI;

namespace StardewConfigMenu.Components {
	sealed class ConfigTextfield: SCMControl {
		readonly IConfigString ModData;
		readonly SCMTextureBox textfieldBackground = SCMTextureBox.TextfieldBackground;

		public override int X {
			get => textfieldBackground.X;
			set {
				if (textfieldBackground.X == value)
					return;

				textfieldBackground.X = value;
			}
		}
		public override int Y {
			get => textfieldBackground.Y;
			set {
				if (textfieldBackground.Y == value)
					return;

				textfieldBackground.Y = value;
			}
		}
		public override int Height => textfieldBackground.Height;
		public override int Width => textfieldBackground.Bounds.Width;

		public ConfigTextfield(IConfigString option) : base(option.Label, option.Enabled) {
			ModData = option;
		}

		public override void Draw(SpriteBatch b) {
			textfieldBackground.Draw(b);
		}
	}
}
