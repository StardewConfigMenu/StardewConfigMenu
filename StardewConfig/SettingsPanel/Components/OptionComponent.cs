using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewValley;



namespace StardewConfigMenu
{
    internal class OptionComponent
    {
        static protected OptionComponent selected;

        protected Vector2 location = new Vector2();

        protected Rectangle bounds;

        protected Vector2 MousePositionRelativeToComponent()
        {
            var position = new Vector2();

            position.Y = Game1.getMouseY() - (int)location.Y - this.bounds.Y;
            position.X = Game1.getMouseX() - (int)location.X - this.bounds.X;

            return position;
        }

        protected bool IsAvailableForSelection()
        {
            if (OptionComponent.selected == this || OptionComponent.selected == null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        protected void RegisterAsActiveComponent()
        {
            OptionComponent.selected = this;
        }

        protected void UnregisterAsActiveComponent()
        {
            if (OptionComponent.selected == this)
            {
                OptionComponent.selected = null;
            }
        }

        protected bool IsActiveComponent()
        {
            return OptionComponent.selected == this;
        }

        public virtual void draw(SpriteBatch b, int x, int y)
        {
            this.location.X = x;
            this.location.Y = y;
        }
    }
}
