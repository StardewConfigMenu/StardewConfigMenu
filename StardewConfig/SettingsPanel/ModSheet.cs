using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework;
using StardewConfigMenu.Panel.Components.ModOptions;
using StardewConfigMenu.Panel.Components;
using Microsoft.Xna.Framework.Input;


namespace StardewConfigMenu.Panel
{
    internal class ModSheet: IClickableMenu
	{
        private List<OptionComponent> Options = new List<OptionComponent>();

        private ClickableTextureComponent upArrow;
        private ClickableTextureComponent downArrow;
        private ClickableTextureComponent scrollBar;
        private Rectangle scrollBarRunner;

        internal bool invisible
        {
            set
            {
                Options.ForEach(x => { x.invisible = value; });
                _invisible = value;
            }
            get
            {
                return _invisible;
            }
        }

        private bool _invisible = false;

        private int startingOption = 0;

        internal ModSheet(ModOptions modOptions, int x, int y, int width, int height): base(x, y, width, height) {
            for(int i = 0; i < modOptions.list.Count; i++)
            {
                // check type of option
                Type t = modOptions.list[i].GetType();
                if (t.Equals(typeof(ModOptionCategoryLabel)))
                    Options.Add(new OptionCategoryLabel(modOptions.list[i] as ModOptionCategoryLabel));
                else if (t.Equals(typeof(ModOptionSelection)))
                {
                    int max = 350;
                    var option = (modOptions.list[i] as ModOptionSelection);
                    option.List.ForEach(choice => { max = Math.Max( (int) Game1.smallFont.MeasureString(choice.label + "     ").X, max); });

                    Options.Add(new ModDropDownComponent((modOptions.list[i] as ModOptionSelection), max));
                }
                else if (t.Equals(typeof(ModOptionToggle)))
                    Options.Add(new ModCheckBoxComponent((modOptions.list[i] as ModOptionToggle), (modOptions.list[i] as ModOptionToggle).enabled));

                // create proper component
                // add to Options
            }
            this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
            this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - 12 * Game1.pixelZoom, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
            this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
            this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.downArrow.bounds.Y - this.upArrow.bounds.Y - this.upArrow.bounds.Height - Game1.pixelZoom * 3);
            AddListeners();
        }

        public void AddListeners()
        {
            RemoveListeners();
            ControlEvents.MouseChanged += MouseChanged;
            ControlEvents.KeyPressed += KeyPressed;
        }

        public void RemoveListeners(bool children = false)
        {
            if (children)
            {
                Options.ForEach(x => { x.RemoveListeners(); });
            }

            ControlEvents.MouseChanged -= MouseChanged;
            ControlEvents.KeyPressed -= KeyPressed;
            this.scrolling = false;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose) { return; }

            if (this.downArrow.containsPoint(x, y) && this.startingOption < Math.Max(0, this.Options.Count - 6))
            {
                this.downArrowPressed();
                Game1.playSound("shwip");
            }
            else if (this.upArrow.containsPoint(x, y) && this.startingOption > 0)
            {
                this.upArrowPressed();
                Game1.playSound("shwip");
            }
            else if (this.scrollBar.containsPoint(x, y))
            {
                this.scrolling = true;
            } 
            // failsafe
            this.startingOption = Math.Max(0, Math.Min(this.Options.Count - 6, this.startingOption));
        }

        private bool scrolling = false;

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose) { return; }
            base.leftClickHeld(x, y);

            if (this.scrolling || this.scrollBarRunner.Contains(x, y))
            {
                int oldPosition = this.startingOption;

                if (y < this.scrollBarRunner.Y)
                {
                    this.startingOption = 0;
                    this.setScrollBarToCurrentIndex();
                } else if (y > this.scrollBarRunner.Bottom)
                {
                    this.startingOption = this.Options.Count - 6;
                    this.setScrollBarToCurrentIndex();
                } else
                {
                    float num = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
                    this.startingOption = (int) Math.Round(Math.Max(0, num * (Options.Count - 6)));
                }

                this.setScrollBarToCurrentIndex();
                if (oldPosition != this.startingOption)
                {
                    Game1.playSound("shiny4");
                }
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);

            this.scrolling = false;
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);

            if (direction > 0)
            {
                this.upArrowPressed();
                Game1.playSound("shiny4");
            } else if (direction < 0)
            {
                this.downArrowPressed();
                Game1.playSound("shiny4");
            }
        }

        private void KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.Up)
                upArrowPressed();
            if (e.KeyPressed == Keys.Down)
                downArrowPressed();
        }

        public void upArrowPressed()
        {
            this.upArrow.scale = this.upArrow.baseScale;
            if (startingOption > 0)
                this.startingOption--;
            this.setScrollBarToCurrentIndex();
        }

        public void downArrowPressed()
        {
            this.downArrow.scale = this.downArrow.baseScale;
            if (startingOption < Options.Count - 6)
                this.startingOption++;
            this.setScrollBarToCurrentIndex();
        }

        public void setScrollBarToCurrentIndex()
        {
            if (this.Options.Count > 0)
            {
                this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.Options.Count - 6 + 1) * this.startingOption + this.upArrow.bounds.Bottom + (Game1.pixelZoom);
                if (this.startingOption == this.Options.Count - 6)
                {
                    this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom * 2;
                }
            }
        }

        protected virtual void MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            if (GameMenu.forcePreventClose) { return; }
            if (!(Game1.activeClickableMenu is GameMenu)) { return; }

            // only allow one component to be interacted with at a time, and must be on settings tab
            if ((Game1.activeClickableMenu as GameMenu).currentTab != ModSettings.pageIndex || invisible) { return; }

            if (e.NewState.ScrollWheelValue > e.PriorState.ScrollWheelValue)
                receiveScrollWheelAction(1);
            else if (e.NewState.ScrollWheelValue < e.PriorState.ScrollWheelValue)
                receiveScrollWheelAction(-1);

            if (e.PriorState.LeftButton == ButtonState.Released)
            {
                if (e.NewState.LeftButton == ButtonState.Pressed)
                {
                    // clicked
                    receiveLeftClick(e.NewState.X, e.NewState.Y);
                }
            }
            else if (e.PriorState.LeftButton == ButtonState.Pressed)
            {
                if (e.NewState.LeftButton == ButtonState.Pressed)
                {
                    leftClickHeld(e.NewState.X, e.NewState.Y);
                }
                else if (e.NewState.LeftButton == ButtonState.Released)
                {
                    releaseLeftClick(e.NewState.X, e.NewState.Y);
                }
            }
            
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            //drawTextureBox();
            if (this.Options.Count > 6)
            {
                this.upArrow.draw(b);
                this.downArrow.draw(b);
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
                this.scrollBar.draw(b);
            }

            //b.DrawString(Game1.dialogueFont, this.Options.modManifest.Name, new Vector2(this.xPositionOnScreen, this.yPositionOnScreen), Color.White);
            List<ModDropDownComponent> dropDowns = new List<ModDropDownComponent>();

            for (int i = 0; i < Options.Count; i++)
            {
                if (i >= startingOption && i < startingOption + 6)
                {
                    Options[i].invisible = false;
                    if (!(Options[i] is ModDropDownComponent))
                        Options[i].draw(b, this.xPositionOnScreen, ((this.height / 6) * (i - startingOption)) + this.yPositionOnScreen + ((this.height / 6) - Options[i].Height) / 2);
                } else
                {
                    Options[i].invisible = true;
                }
            }

            // Draw Dropdowns last, they must be on top; must draw from bottom to top
            for (int i = Math.Min(startingOption + 5, Options.Count - 1); i >= startingOption; i--)
            {
                if (Options[i] is ModDropDownComponent)
                    Options[i].draw(b, this.xPositionOnScreen, ((this.height / 6) * (i - startingOption)) + this.yPositionOnScreen + ((this.height / 6) - Options[i].Height) / 2);
            }

        }
    }
}
