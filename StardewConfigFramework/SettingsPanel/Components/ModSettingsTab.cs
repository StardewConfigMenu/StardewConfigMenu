using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;

namespace StardewConfigFramework
{
    static internal class Constants
    {
        internal const int TabNumber = 11;
    }

    internal class ModSettingsTab : IClickableMenu
    {

        private Settings controller;

        //
        // Constructors
        //
        internal ModSettingsTab(Settings controller, Rectangle bounds) : base(bounds.X, bounds.Y, bounds.Width, bounds.Height)
        {
            this.controller = controller;
            AddListeners();
        }

        internal void AddListeners()
        {
            RemoveListeners();
            ControlEvents.MouseChanged += receiveLeftClick;
        }

        internal void RemoveListeners()
        {
            ControlEvents.MouseChanged -= receiveLeftClick;
        }

        public void receiveLeftClick(object sender, EventArgsMouseStateChanged e)
        {

            if (e.NewState.LeftButton != ButtonState.Pressed)
                return;

            // Prevents repeating sound while holding on button
            if (e.PriorState.LeftButton == ButtonState.Pressed)
                return;

            if (!(Game1.activeClickableMenu is GameMenu)) 
                return;

            var menu = Game1.activeClickableMenu as GameMenu;

            if (menu.currentTab == GameMenu.mapTab)
                return;

            if (isWithinBounds(e.NewPosition.X, e.NewPosition.Y))
            {
                base.receiveLeftClick(e.NewPosition.X, e.NewPosition.Y, true);
                ModSettingsPage.SetActive();
                Game1.playSound("smallSelect");
            }
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            var gameMenu = (GameMenu) Game1.activeClickableMenu;

            b.Draw(Game1.mouseCursors, new Vector2((float)this.xPositionOnScreen, (float)(this.yPositionOnScreen + (gameMenu.currentTab == 8 ? 8 : 0))), new Rectangle(16, 368, 16, 16), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);

            // Draw icon
            b.Draw(Game1.mouseCursors, new Vector2((float)this.xPositionOnScreen + 8, (float)(this.yPositionOnScreen + (gameMenu.currentTab == 8 ? 8 : 0)) + 14), new Rectangle(32, 672, 16, 16), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 1);
            if (this.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
            {
                IClickableMenu.drawHoverText(Game1.spriteBatch, "Mod Options", Game1.smallFont);
            }

            drawMouse(b);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }
    }
}
