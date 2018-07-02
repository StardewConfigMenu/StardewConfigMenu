using Type = System.Type;
using Math = System.Math;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework;
using StardewConfigFramework.Options;
using StardewConfigMenu.Components.DataBacked;
using StardewConfigMenu.Components;
using Microsoft.Xna.Framework.Input;


namespace StardewConfigMenu {
	internal class ModSheet: IClickableMenu {
		private readonly List<SCMControl> Options = new List<SCMControl>();

		private ClickableTextureComponent upArrow;
		private ClickableTextureComponent downArrow;
		private ClickableTextureComponent scrollBar;
		private Rectangle scrollBarRunner;

		internal bool Visible {
			set {
				if (!value)
					Options.ForEach(x => {
						x.Visible = value;
					});
				else
					setVisibleOptions();

				_visible = value;
			}
			get => _visible;
		}

		private bool _visible = false;

		private int startingOption = 0;

		internal ModSheet(IOptionsPackage package, int x, int y, int width, int height) : base(x, y, width, height) {
			foreach (IConfigOption option in package.Tabs[0].Options) {
				Type optionType = option.GetType();
				if (typeof(IConfigHeader).IsAssignableFrom(optionType))
					Options.Add(new ConfigCategoryLabel(option as IConfigHeader));
				else if (typeof(IConfigSelection).IsAssignableFrom(optionType)) {
					int minWidth = 350;
					var selection = (option as IConfigSelection);
					foreach (ISelectionChoice choice in selection.Choices) {
						minWidth = Math.Max((int) Game1.smallFont.MeasureString(choice.Label + "     ").X, minWidth);
					}
					Options.Add(new ConfigDropdown(selection, minWidth));
				} else if (typeof(IConfigToggle).IsAssignableFrom(optionType))
					Options.Add(new ConfigCheckbox(option as IConfigToggle));
				else if (typeof(IConfigAction).IsAssignableFrom(optionType))
					Options.Add(new ConfigButton(option as IConfigAction));
				else if (typeof(IConfigStepper).IsAssignableFrom(optionType))
					Options.Add(new ConfigPlusMinus(option as IConfigStepper));
				else if (typeof(IConfigRange).IsAssignableFrom(optionType))
					Options.Add(new ConfigSlider(option as IConfigRange));
				else
					throw new System.Exception("Unknown Component");
			}

			upArrow = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width + Game1.tileSize / 4, yPositionOnScreen, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
			downArrow = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width + Game1.tileSize / 4, yPositionOnScreen + height - 12 * Game1.pixelZoom, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
			scrollBar = new ClickableTextureComponent(new Rectangle(upArrow.bounds.X + Game1.pixelZoom * 3, upArrow.bounds.Y + upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
			scrollBarRunner = new Rectangle(scrollBar.bounds.X, upArrow.bounds.Y + upArrow.bounds.Height + Game1.pixelZoom, scrollBar.bounds.Width, downArrow.bounds.Y - upArrow.bounds.Y - upArrow.bounds.Height - Game1.pixelZoom * 3);
			AddListeners();
		}

		public void AddListeners() {
			RemoveListeners();
			ControlEvents.KeyPressed += KeyPressed;
		}

		public void RemoveListeners(bool children = false) {
			if (children) {
				Options.ForEach(x => { x.Visible = false; });
			}

			ControlEvents.KeyPressed -= KeyPressed;
			scrolling = false;
		}

		public override void receiveRightClick(int x, int y, bool playSound = true) {

			if (GameMenu.forcePreventClose) { return; }

			Options.ForEach(z => {
				if (z.Visible)
					z.ReceiveRightClick(x, y, playSound);
			});
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true) {
			if (GameMenu.forcePreventClose || !Visible) { return; }

			Options.ForEach(z => {
				if (z.Visible)
					z.ReceiveLeftClick(x, y, playSound);
			});

			if (downArrow.containsPoint(x, y) && startingOption < Math.Max(0, Options.Count - 6)) {
				if (playSound && Options.Count > 6 && startingOption != Options.Count - 6)
					Game1.playSound("shwip");
				downArrowPressed();
			} else if (upArrow.containsPoint(x, y) && startingOption > 0) {
				if (playSound && Options.Count > 6 && startingOption != 0)
					Game1.playSound("shwip");

				upArrowPressed();

			} else if (scrollBar.containsPoint(x, y) && Options.Count > 6) {
				scrolling = true;
			}
			// failsafe
			startingOption = Math.Max(0, Math.Min(Options.Count - 6, startingOption));
		}

		private bool scrolling = false;

		public override void leftClickHeld(int x, int y) {
			if (GameMenu.forcePreventClose) { return; }

			Options.ForEach(z => {
				if (z.Visible)
					z.LeftClickHeld(x, y);
			});

			if (scrolling) {
				int oldPosition = startingOption;

				if (y < scrollBarRunner.Y) {
					startingOption = 0;
					setScrollBarToCurrentIndex();
				} else if (y > scrollBarRunner.Bottom) {
					startingOption = Options.Count - 6;
					setScrollBarToCurrentIndex();
				} else {
					float num = (float) (y - scrollBarRunner.Y) / (float) scrollBarRunner.Height;
					startingOption = (int) Math.Round(Math.Max(0, num * (Options.Count - 6)));
				}

				setScrollBarToCurrentIndex();
				if (oldPosition != startingOption) {
					Game1.playSound("shiny4");
				}
			}
		}

		public override void releaseLeftClick(int x, int y) {
			Options.ForEach(z => {
				if (z.Visible)
					z.ReleaseLeftClick(x, y);
			});

			scrolling = false;
		}

		public override void receiveScrollWheelAction(int direction) {
			Options.ForEach(z => {
				if (z.Visible)
					z.ReceiveScrollWheelAction(direction);
			});

			if (direction > 0) {
				if (Options.Count > 6 && startingOption != 0)
					Game1.playSound("shiny4");

				upArrowPressed();

			} else if (direction < 0) {
				if (Options.Count > 6 && startingOption != Options.Count - 6)
					Game1.playSound("shiny4");
				downArrowPressed();
			}
		}

		private void KeyPressed(object sender, EventArgsKeyPressed e) {
			Options.ForEach(z => {
				if (z.Visible)
					z.ReceiveKeyPress(e.KeyPressed);
			});

			if (e.KeyPressed == Keys.Up)
				upArrowPressed();
			if (e.KeyPressed == Keys.Down)
				downArrowPressed();
		}

		public void upArrowPressed() {
			upArrow.scale = upArrow.baseScale;
			if (startingOption > 0)
				startingOption--;
			setScrollBarToCurrentIndex();
			//setVisibleOptions();
		}

		public void downArrowPressed() {
			downArrow.scale = downArrow.baseScale;
			if (startingOption < Options.Count - 6)
				startingOption++;
			setScrollBarToCurrentIndex();
			//setVisibleOptions();
		}

		public void setVisibleOptions() {
			for (int i = 0; i < Options.Count; i++) {
				if (i >= startingOption && i < startingOption + 6) {
					Options[i].Visible = true;
				} else {
					Options[i].Visible = false;
				}
			}
		}

		public void setScrollBarToCurrentIndex() {
			setVisibleOptions();
			if (Options.Count > 0) {
				scrollBar.bounds.Y = scrollBarRunner.Height / Math.Max(1, Options.Count - 6 + 1) * startingOption + upArrow.bounds.Bottom + (Game1.pixelZoom);
				if (startingOption == Options.Count - 6) {
					scrollBar.bounds.Y = downArrow.bounds.Y - scrollBar.bounds.Height - Game1.pixelZoom * 2;
				}
			}
		}

		public override void draw(SpriteBatch b) {

			//drawTextureBox();
			if (Options.Count > 6) {
				upArrow.draw(b);
				downArrow.draw(b);
				drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), scrollBarRunner.X, scrollBarRunner.Y, scrollBarRunner.Width, scrollBarRunner.Height, Color.White, (float) Game1.pixelZoom, false);
				scrollBar.draw(b);
			}

			//b.DrawString(Game1.dialogueFont, Options.modManifest.Name, new Vector2(xPositionOnScreen, yPositionOnScreen), Color.White);

			for (int i = startingOption; i < Options.Count; i++) {
				if (!(Options[i] is ConfigDropdown) && Options[i].Visible)
					Options[i].Draw(b, xPositionOnScreen, ((height / 6) * (i - startingOption)) + yPositionOnScreen + ((height / 6) - Options[i].Height) / 2);
			}

			// Draw Dropdowns last, they must be on top; must draw from bottom to top
			for (int i = Math.Min(startingOption + 5, Options.Count - 1); i >= startingOption; i--) {
				if (Options[i] is ConfigDropdown && Options[i].Visible)
					Options[i].Draw(b, xPositionOnScreen, ((height / 6) * (i - startingOption)) + yPositionOnScreen + ((height / 6) - Options[i].Height) / 2);
			}
		}
	}
}
