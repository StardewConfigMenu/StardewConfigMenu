using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI.Events;



namespace StardewConfigMenu.Panel.Components
{
    abstract class OptionComponent
    {
        static protected OptionComponent selectedComponent;

        protected Rectangle bounds;
        public abstract bool enabled { get; set; }
        protected string label;

        public int Height => this.bounds.Height;
        public int Width => this.bounds.Width;
        public int X => this.bounds.X;
        public int Y => this.bounds.Y;

        public OptionComponent(string label, bool enabled = true)
        {
            this.label = label;
            this.enabled = enabled;
        }

        protected bool IsAvailableForSelection()
        {
            if (OptionComponent.selectedComponent == this || OptionComponent.selectedComponent == null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        protected void RegisterAsActiveComponent()
        {
            OptionComponent.selectedComponent = this;
        }

        protected void UnregisterAsActiveComponent()
        {
            if (OptionComponent.selectedComponent == this)
            {
                OptionComponent.selectedComponent = null;
            }
        }

        protected bool IsActiveComponent()
        {
            return OptionComponent.selectedComponent == this;
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

        internal virtual void AddListeners()
        {
            RemoveListeners();
            ControlEvents.MouseChanged += MouseChanged;
        }

        internal virtual void RemoveListeners()
        {
            ControlEvents.MouseChanged -= MouseChanged;
            this.UnregisterAsActiveComponent();
        }

        internal bool invisible = false;

        protected virtual void MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            // only allow one component to be interacted with at a time, and must be on settings tab
            if (!this.IsAvailableForSelection() || (Game1.activeClickableMenu as GameMenu).currentTab != ModSettings.pageIndex || invisible) { return; }

            if (e.PriorState.LeftButton == ButtonState.Released)
            {
                if (e.NewState.LeftButton == ButtonState.Pressed)
                {
                    // clicked
                    leftClicked(e.NewState.X, e.NewState.Y);
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
                    leftClickReleased(e.NewState.X, e.NewState.Y);
                }
            }
            else
            {
                this.UnregisterAsActiveComponent();
            }
        }

        protected virtual void leftClicked(int x, int y) { }

        protected virtual void leftClickHeld(int x, int y) { }

        protected virtual void leftClickReleased(int x, int y) { }
    }
}
