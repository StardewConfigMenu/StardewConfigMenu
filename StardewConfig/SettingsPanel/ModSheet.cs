using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewConfigFramework;
using StardewConfigMenu.Panel.Components;

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
            }
        }

        private int startingOption = 0;

        internal ModSheet(ModOptions modOptions, int x, int y, int width, int height): base(x, y, width, height) {
            for(int i = 0; i < modOptions.list.Count; i++)
            {
                // check type of option
                Type t = modOptions.list[i].GetType();
                if (t.Equals(typeof(ModOptionSelection)))
                {
                    int max = 250;
                    var option = (modOptions.list[i] as ModOptionSelection);
                    option.List.ForEach(optionString => { max = Math.Max( (int) Game1.smallFont.MeasureString(optionString + "     ").X, max); });

                    Options.Add(new ModDropDownComponent((modOptions.list[i] as ModOptionSelection), max));
                }
                if (t.Equals(typeof(ModOptionToggle)))
                    Options.Add(new ModCheckBoxComponent((modOptions.list[i] as ModOptionToggle), (modOptions.list[i] as ModOptionToggle).enabled));

                // create proper component
                // add to Options
            }
            this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
            this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - 12 * Game1.pixelZoom, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
            this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
            this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.downArrow.bounds.Y - this.upArrow.bounds.Y - this.upArrow.bounds.Height - Game1.pixelZoom * 4);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            //drawTextureBox();
#if DEBUG
            if (this.Options.Count >= 0)
#else
            if (this.Options.Count > 6)
#endif
            {
                this.upArrow.draw(b);
                this.downArrow.draw(b);
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
                this.scrollBar.draw(b);
            }

            //b.DrawString(Game1.dialogueFont, this.Options.modManifest.Name, new Vector2(this.xPositionOnScreen, this.yPositionOnScreen), Color.White);

            for (int i = startingOption; i < Math.Min(startingOption + 6, Options.Count); i++)
            {
                Options[i].draw(b, this.xPositionOnScreen, ((this.height / 6) * i) + this.yPositionOnScreen + ((this.height / 6) - Options[i].Height)/2); 
            }

        }
    }
}
