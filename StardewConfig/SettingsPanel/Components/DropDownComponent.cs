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

namespace StardewConfigMenu.Panel.Components
{
    internal delegate void DropDownOptionSelected(int selected);

    internal class DropDownComponent: OptionComponent
    {

        internal event DropDownOptionSelected DropDownOptionSelected;
        //
        // Static Fields
        //
        //public const int pixelsHigh = 11;

        public static Rectangle dropDownButtonSource = new Rectangle(437, 450, 10, 11);

        public static Rectangle dropDownBGSource = new Rectangle(433, 451, 3, 3);

        //
        // Fields
        //
        protected Rectangle dropDownBounds;

        public virtual int selectedOption
        {
            get { return dropDownOptions.IndexOf(this.dropDownDisplayOptions[0]); }
            set {
                if (selectedOption == value)
                    return;

                var index = dropDownDisplayOptions.IndexOf(dropDownOptions[value]);

				dropDownDisplayOptions.Insert(0, dropDownOptions[value]);
                dropDownDisplayOptions.RemoveAt(index + 1);
                this.DropDownOptionSelected?.Invoke(value);
            }
        }

        private int hoveredChoice = 0;

        public override bool enabled 
        {
            get {
                if (!_enabled)
                    return _enabled;
                else
                    return dropDownOptions.Count != 0; }
            protected set
            {
                _enabled = value;
            }
        }

        private bool _enabled;

        // Original List
        private List<string> _dropDownOptions = new List<string>();

        protected virtual List<string> dropDownOptions
        {
            get { return _dropDownOptions;  }
        }

        // List where order can be changed
        private List<string> _dropDownDisplayOptions = new List<string>();

        protected virtual List<string> dropDownDisplayOptions
        {
			get
			{
                if (_dropDownOptions.Count == 0)
				{
					_dropDownDisplayOptions.Clear();
				}
				else
				{
					var options = dropDownOptions;
					var toRemove = _dropDownDisplayOptions.Except(options).ToList();
					var toAdd = options.Except(_dropDownDisplayOptions).ToList();

					_dropDownDisplayOptions.RemoveAll(x => toRemove.Contains(x));
					_dropDownDisplayOptions.AddRange(toAdd);
				}

				dropDownBounds.Height = this.bounds.Height * dropDownOptions.Count;
				return _dropDownDisplayOptions;
			}
        }

        protected virtual void SelectDisplayedOption(int option)
        {
            if (option == 0)
                return;
            var selected = dropDownDisplayOptions[option];
            dropDownDisplayOptions.Insert(0, selected);
            dropDownDisplayOptions.RemoveAt(option + 1);
			this.DropDownOptionSelected?.Invoke(selectedOption);
		}

        //
        // Constructors
        //

        public DropDownComponent(List<string> choices, string label, int width, int x, int y, bool enabled = true) : base(label, enabled)
        {
			this.dropDownOptions.AddRange(choices);
			this.label = label;
            this.bounds = new Rectangle(x, y, width + Game1.pixelZoom * 12, 11 * Game1.pixelZoom);
            this.dropDownBounds = new Rectangle(this.bounds.X, this.bounds.Y, width, this.bounds.Height * this.dropDownOptions.Count);
        }

        // This contructor requires Draw(b,x,y) to move the object from origin
        public DropDownComponent(List<string> choices, string label, int width, bool enabled = true) : base(label, enabled)
        {
            this.dropDownOptions.AddRange(choices);
            this.label = label;
            this.bounds = new Rectangle(0, 0, width + Game1.pixelZoom * 12, 11 * Game1.pixelZoom);
            this.dropDownBounds = new Rectangle(this.bounds.X, this.bounds.Y, width, this.bounds.Height * this.dropDownOptions.Count);
        }

        protected DropDownComponent(string label, int width, bool enabled = true) : base(label, enabled)
		{
			this.label = label;
			this.bounds = new Rectangle(0, 0, width + Game1.pixelZoom * 12, 11 * Game1.pixelZoom);
			this.dropDownBounds = new Rectangle(this.bounds.X, this.bounds.Y, width, this.bounds.Height * this.dropDownOptions.Count);
		}

        //
        // Methods
        //

        public override void draw(SpriteBatch b, int x, int y)
        {
            this.dropDownBounds.X = x;
            this.dropDownBounds.Y = y;
            base.draw(b, x, y);
        }

        public override void draw(SpriteBatch b)
        {
            float scale = (this.enabled) ? 1f : 0.33f;

            var displayedChoices = this.dropDownDisplayOptions;

            var labelSize = Game1.dialogueFont.MeasureString(this.label);

            // Draw Label
            Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

            // If menu is being clicked, and no other components are in use (to prevent click overlap of the dropdown)
            if (this.IsActiveComponent())
            {
                // Draw Background of dropdown
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, this.dropDownBounds.X, this.dropDownBounds.Y, this.dropDownBounds.Width, this.dropDownBounds.Height, Color.White * scale, (float)Game1.pixelZoom, false);

                for (int i = 0; i < displayedChoices.Count; i++)
                {
                    if (i == this.hoveredChoice && dropDownBounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        b.Draw(Game1.staminaRect, new Rectangle(this.dropDownBounds.X, this.dropDownBounds.Y + i * this.bounds.Height, this.dropDownBounds.Width, this.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
                    }
                    b.DrawString(Game1.smallFont, displayedChoices[i], new Vector2((float)(this.dropDownBounds.X + Game1.pixelZoom), (float)(this.dropDownBounds.Y + Game1.pixelZoom * 2 + this.bounds.Height * i)), Game1.textColor * scale, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
                }
                b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float)(this.bounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.Wheat * scale, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.981f);
            }
            else
            {
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, this.bounds.X, this.bounds.Y, this.bounds.Width - Game1.pixelZoom * 12, this.bounds.Height, Color.White * scale, (float)Game1.pixelZoom, false);

                b.DrawString(Game1.smallFont, (this.selectedOption >= displayedChoices.Count || this.selectedOption < 0) ? string.Empty : displayedChoices[0], new Vector2((float)(this.bounds.X + Game1.pixelZoom), (float)(this.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor * scale, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
                b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float)(this.bounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.White * scale, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }



        protected override void leftClicked(int x, int y)
        {
            if (this.enabled && this.bounds.Contains(x, y))
            {
                this.RegisterAsActiveComponent();
                this.hoveredChoice = 0;
                this.leftClickHeld(x, y);
                Game1.playSound("shwip");
            }
        }

        protected override void leftClickHeld(int x, int y)
        {
            if (this.enabled && this.IsActiveComponent() && this.dropDownBounds.Contains(x, y))
            {
                this.dropDownBounds.Y = Math.Min(this.dropDownBounds.Y, Game1.viewport.Height - this.dropDownBounds.Height);
                this.hoveredChoice = (int)Math.Max(Math.Min((float)(y - this.dropDownBounds.Y) / (float)this.bounds.Height, (float)(this.dropDownOptions.Count - 1)), 0f);
            }
        }

        protected override void leftClickReleased(int x, int y)
        {
            if (this.dropDownBounds.Contains(x, y) && this.IsActiveComponent())
            {
                if (this.enabled && this.dropDownOptions.Count > 0)
                {
                    this.SelectDisplayedOption(this.hoveredChoice);
                }

            }

            this.UnregisterAsActiveComponent();
        }

        protected override void receiveKeyPress(Keys key)
        {
            if (Game1.options.snappyMenus && Game1.options.gamepadControls)
            {
                if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
                {
                    this.hoveredChoice++;
                    if (this.hoveredChoice >= this.dropDownOptions.Count)
                    {
                        this.hoveredChoice = 0;
                    }
                }
                else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
                {
                    this.hoveredChoice--;
                    if (this.hoveredChoice < 0)
                    {
                        this.hoveredChoice = this.dropDownOptions.Count - 1;
                    }
                }
            }
        }
    }

}

