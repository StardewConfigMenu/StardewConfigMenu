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

        protected Rectangle bounds;

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

        // For moving the component
        public virtual void draw(SpriteBatch b, int x, int y)
        {
            this.bounds.X = x;
            this.bounds.Y = y;
            this.draw(b);
        }

        // static drawing of component
        public virtual void draw(SpriteBatch b) { }
    }
}
