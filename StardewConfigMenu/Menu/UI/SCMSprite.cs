using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewConfigMenu.UI {
	public class SCMSprite {

		public static SCMSprite OKButton => new SCMSprite(Game1.mouseCursors, 46);
		public static SCMSprite ClearButton => new SCMSprite(Game1.mouseCursors, 47);
		public static SCMSprite SetButton => new SCMSprite(Game1.mouseCursors, OptionsInputListener.setButtonSource, Game1.pixelZoom);
		public static SCMSprite DoneButton => new SCMSprite(Game1.mouseCursors, new Rectangle(441, 411, 24, 13), Game1.pixelZoom);
		public static SCMSprite GiftButton => new SCMSprite(Game1.mouseCursors, new Rectangle(229, 410, 14, 14), Game1.pixelZoom);
		public static SCMSprite CheckboxChecked => new SCMSprite(Game1.mouseCursors, OptionsCheckbox.sourceRectChecked, Game1.pixelZoom);
		public static SCMSprite CheckboxUnchecked => new SCMSprite(Game1.mouseCursors, OptionsCheckbox.sourceRectUnchecked, Game1.pixelZoom);

		public static SCMSprite DropDownButton => new SCMSprite(Game1.mouseCursors, OptionsDropDown.dropDownButtonSource, Game1.pixelZoom);

		public static SCMSprite MinusButton => new SCMSprite(Game1.mouseCursors, OptionsPlusMinus.minusButtonSource, Game1.pixelZoom);
		public static SCMSprite PlusButton => new SCMSprite(Game1.mouseCursors, OptionsPlusMinus.plusButtonSource, Game1.pixelZoom);

		public static SCMSprite SliderBar => new SCMSprite(Game1.mouseCursors, OptionsSlider.sliderButtonRect, Game1.pixelZoom);

		public static SCMSprite TextfieldBackgroundLeft {
			get {
				// full box: new Rectangle(293, 360, 24, 24)
				//var texture = Game1.content.Load<Texture2D>("LooseSprites\\textBox");

				return new SCMSprite(Game1.mouseCursors, new Rectangle(293, 360, 4, 24), Game1.pixelZoom);
			}
		}

		public static SCMSprite TextfieldBackgroundMiddle {
			get {
				//var texture = Game1.content.Load<Texture2D>("LooseSprites\\textBox");
				//return new SCMSprite(texture, new Rectangle(4 * Game1.pixelZoom, 0, Game1.pixelZoom, texture.Height));
				return new SCMSprite(Game1.mouseCursors, new Rectangle(297, 360, 4, 24), Game1.pixelZoom);
			}
		}

		public static SCMSprite TextfieldBackgroundRight {
			get {
				//var texture = Game1.content.Load<Texture2D>("LooseSprites\\textBox");
				//return new SCMSprite(texture, new Rectangle(texture.Width - (3 * Game1.pixelZoom), 0, (4 * Game1.pixelZoom), texture.Height));
				return new SCMSprite(Game1.mouseCursors, new Rectangle(313, 360, 4, 24), Game1.pixelZoom);
			}
		}

		public SCMSprite(Texture2D texture, Rectangle source, float scale = 1f, Color color = default(Color), float transparency = 1f) {
			SourceRect = source;
			Scale = scale;
			Texture = texture;
			Color = (color == default(Color)) ? Color.White : color;
			Transparency = transparency;
			_Bounds.Width = (int) (SourceRect.Width * Scale);
			_Bounds.Height = (int) (SourceRect.Height * Scale);
		}

		public SCMSprite(Texture2D tileSheet, int tilePosition, int width = -1, int height = -1, float scale = 1f) {
			SourceRect = Game1.getSourceRectForStandardTileSheet(tileSheet, tilePosition, width, height);
			Scale = scale;
			Texture = tileSheet;
			Color = Color.White;
			Transparency = 1f;
			_Bounds.Width = (int) (SourceRect.Width * Scale);
			_Bounds.Height = (int) (SourceRect.Height * Scale);
		}

		public int X { get => _Bounds.X; set => _Bounds.X = value; }
		public int Y { get => _Bounds.Y; set => _Bounds.Y = value; }
		public int Width { get => _Bounds.Width; set => _Bounds.Width = value; }
		public int Height { get => _Bounds.Height; set => _Bounds.Height = value; }

		public Rectangle Bounds => _Bounds;

		Rectangle _Bounds;

		readonly Texture2D Texture;
		public Rectangle SourceRect;
		public float Scale;
		public Color Color;
		public float Transparency;

		public void Draw(SpriteBatch b) {
			var origin = new Vector2(Bounds.X, Bounds.Y);
			b.Draw(Texture, origin, SourceRect, Color * Transparency, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.9f);
		}

		public void DrawStretched(SpriteBatch b) {
			b.Draw(Texture, Bounds, SourceRect, Color);
		}
	}
}
