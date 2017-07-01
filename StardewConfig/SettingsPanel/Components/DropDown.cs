using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewConfigFramework;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu
{

    internal class ModOptionDropDown: OptionComponent
    {
        //
        // Static Fields
        //
        //public const int pixelsHigh = 11;

        public static Rectangle dropDownButtonSource = new Rectangle(437, 450, 10, 11);

        public static Rectangle dropDownBGSource = new Rectangle(433, 451, 3, 3);

        //
        // Fields
        //
        private Rectangle dropDownBounds;

        private Rectangle selectorBounds;

        private Vector2 location = new Vector2();

        private bool clicked;

        public int startingSelected;

        public int recentSlotY;

        public int selectedOption = 0;

        public bool disabled = true;

        public string label = "";

        private List<string> dropDownOptions = new List<string>();

        public void ClearOptions()
        {
            dropDownOptions.Clear();
            dropDownBounds.Height = this.selectorBounds.Height * this.dropDownOptions.Count;
            this.selectedOption = 0;
            this.disabled = true;
        }

        public void AddOption(string item)
        {
            dropDownOptions.Add(item);
            dropDownBounds.Height = this.selectorBounds.Height * this.dropDownOptions.Count;
            this.disabled = false;
        }

        //
        // Constructors
        //
        public ModOptionDropDown(string label, int width) //: base(label, x, y, (int)Game1.smallFont.MeasureString("Stardew Configuration Menu Framework     ").X + Game1.pixelZoom * 12, 11 * Game1.pixelZoom, whichOption)
        {
            AddListeners();
            this.label = label;
            this.selectorBounds = new Rectangle(0, 0, width + Game1.pixelZoom * 12, 11 * Game1.pixelZoom);
            this.dropDownBounds = new Rectangle(this.selectorBounds.X, this.selectorBounds.Y, width, this.selectorBounds.Height * this.dropDownOptions.Count);
        }

        internal void AddListeners()
        {
            RemoveListeners();
            ControlEvents.MouseChanged += MouseChanged;
        }

        internal void RemoveListeners()
        {
            ControlEvents.MouseChanged -= MouseChanged;
            if (OptionComponent.selected == this)
            {
                OptionComponent.selected = null;
            }
        }

        //
        // Methods
        //
        public void draw(SpriteBatch b, int x, int y)
        {
            this.location.X = x;
            this.location.Y = y;
            this.recentSlotY = y;
            float scale = (!this.disabled) ? 1f : 0.33f;

            var labelSize = Game1.dialogueFont.MeasureString(this.label);

            b.DrawString(Game1.dialogueFont, this.label, new Vector2((float)(x + this.selectorBounds.X + this.selectorBounds.Width + Game1.pixelZoom), (float)(y + this.selectorBounds.Y + ((this.selectorBounds.Height - labelSize.Y) / 2))), Game1.textColor);

            if (this.clicked)
            {
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, x + this.dropDownBounds.X, y + this.dropDownBounds.Y, this.dropDownBounds.Width, this.dropDownBounds.Height, Color.White * scale, (float)Game1.pixelZoom, false);
                for (int i = 0; i < this.dropDownOptions.Count; i++)
                {
                    if (i == this.selectedOption)
                    {
                        b.Draw(Game1.staminaRect, new Rectangle(x + this.dropDownBounds.X, y + this.dropDownBounds.Y + i * this.selectorBounds.Height, this.dropDownBounds.Width, this.selectorBounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
                    }
                    b.DrawString(Game1.smallFont, this.dropDownOptions[i], new Vector2((float)(x + this.dropDownBounds.X + Game1.pixelZoom), (float)(y + this.dropDownBounds.Y + Game1.pixelZoom * 2 + this.selectorBounds.Height * i)), Game1.textColor * scale, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }
                b.Draw(Game1.mouseCursors, new Vector2((float)(x + this.selectorBounds.X + this.selectorBounds.Width - Game1.pixelZoom * 12), (float)(y + this.selectorBounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.Wheat * scale, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.981f);
            }
            else
            {
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, x + this.selectorBounds.X, y + this.selectorBounds.Y, this.selectorBounds.Width - Game1.pixelZoom * 12, this.selectorBounds.Height, Color.White * scale, (float)Game1.pixelZoom, false);
                if (OptionsDropDown.selected == null || OptionsDropDown.selected.Equals(this))
                {
                    b.DrawString(Game1.smallFont, (this.selectedOption >= this.dropDownOptions.Count || this.selectedOption < 0) ? string.Empty : this.dropDownOptions[this.selectedOption], new Vector2((float)(x + this.selectorBounds.X + Game1.pixelZoom), (float)(y + this.selectorBounds.Y + Game1.pixelZoom * 2)), Game1.textColor * scale, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
                }
                b.Draw(Game1.mouseCursors, new Vector2((float)(x + this.selectorBounds.X + this.selectorBounds.Width - Game1.pixelZoom * 12), (float)(y + this.selectorBounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.White * scale, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }

        private void MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            // only allow one component to be interacted with at a time
            if (!this.IsAvailableForSelection())
            {
                return;
            }
            
            if (e.PriorState.LeftButton == ButtonState.Released)
            {
                if(e.NewState.LeftButton == ButtonState.Pressed)
                {
                    // clicked
                    receiveLeftClick(e.NewState.X, e.NewState.Y);
                }
            }
            else if (e.PriorState.LeftButton == ButtonState.Pressed)
            {
                if (e.NewState.LeftButton == ButtonState.Pressed)
                {
                    // clicked
                    leftClickHeld(e.NewState.X, e.NewState.Y);
                } else if (e.NewState.LeftButton == ButtonState.Released)
                {
                    // clicked
                    leftClickReleased(e.NewState.X, e.NewState.Y);
                    clicked = false;
                }
            } else
            {
                clicked = false;
            }
        }

        public void receiveLeftClick(int x, int y)
        {

            if (!this.disabled && this.selectorBounds.Contains(x - (int) location.X, y - (int) location.Y))
            {
                OptionComponent.selected = this;
                this.clicked = true;
                this.startingSelected = this.selectedOption;
                this.leftClickHeld(x, y);
                Game1.playSound("shwip");
            }
        }

        public void leftClickHeld(int x, int y)
        {
            if (!this.disabled && this.clicked && this.dropDownBounds.Contains(x - (int) location.X, y - (int) location.Y))
            {
                this.dropDownBounds.Y = Math.Min(this.dropDownBounds.Y, Game1.viewport.Height - this.dropDownBounds.Height - this.recentSlotY);
                this.selectedOption = (int)Math.Max(Math.Min((float)(y - this.dropDownBounds.Y) / (float)this.selectorBounds.Height, (float)(this.dropDownOptions.Count - 1)), 0f);
            }
        }

        public void leftClickReleased(int x, int y)
        {
            if (!this.disabled && this.dropDownOptions.Count > 0)
            {
                OptionComponent.selected = null;
                this.clicked = false;
                if (this.dropDownBounds.Contains(x, y))
                {
                }
                else
                {
                    this.selectedOption = this.startingSelected;
                }
                OptionsDropDown.selected = null;
            }
        }

        public void receiveKeyPress(Keys key)
        {
            if (Game1.options.snappyMenus && Game1.options.gamepadControls)
            {
                if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
                {
                    this.selectedOption++;
                    if (this.selectedOption >= this.dropDownOptions.Count)
                    {
                        this.selectedOption = 0;
                    }
                }
                else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
                {
                    this.selectedOption--;
                    if (this.selectedOption < 0)
                    {
                        this.selectedOption = this.dropDownOptions.Count - 1;
                    }
                }
            }
        }
    }

    /// <summary>A dropdown UI component which lets the player choose from a list of values.</summary>
    internal class DropDownOption : OptionsDropDown
    {
        private ModOptionSelection Option;

		public virtual new int selectedOption
		{
            get { return Option.Selection; }
            private set {
                this.Option.MakeSelection(value);
            }
		}

        public virtual new List<string> dropDownOptions => Option.List;

        public DropDownOption(ModOptionSelection option, int x = -1, int y = -1) : base(option.LabelText, option.Selection, x, y)
        {
            this.Option = option;
        }

    }


}

