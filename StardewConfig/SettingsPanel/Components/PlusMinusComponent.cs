using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewConfigFramework;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework.Input;

namespace StardewConfigMenu.Panel.Components
{
    internal delegate void PlusMinusValueChanged(decimal Value);

    internal class PlusMinusComponent : OptionComponent
    {

        internal event PlusMinusValueChanged PlusMinusValueChanged;

        internal PlusMinusComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, int x, int y, DisplayType type = DisplayType.NONE, bool enabled = true) : base(labelText, enabled)
        {
            this._min = Math.Round(min, 3);
            this._max = Math.Round(max, 3);
            this._stepSize = Math.Round(stepsize, 3);
            this._type = type;

            var valid = CheckValidInput(Math.Round(defaultSelection, 3));
            var newVal = (int)((valid - _min) / _stepSize) * _stepSize + _min;
            this._Value = newVal;

            this.bounds = new Rectangle(x, y, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Height);

            var maxRect = Game1.dialogueFont.MeasureString(this._max.ToString());
            var minRect = Game1.dialogueFont.MeasureString(this._min.ToString());

            ValueMaxLabelSize = (maxRect.X > minRect.X) ? maxRect : minRect;
        }

        internal PlusMinusComponent(string labelText, decimal min, decimal max, decimal stepsize, decimal defaultSelection, DisplayType type = DisplayType.NONE, bool enabled = true) : base(labelText, enabled)
        {
            this._min = Math.Round(min, 3);
            this._max = Math.Round(max, 3);
            this._stepSize = Math.Round(stepsize, 3);
            this._type = type;


            var valid = CheckValidInput(Math.Round(defaultSelection, 3));
            var newVal = (int)((valid - _min) / _stepSize) * _stepSize + _min;
            this._Value = newVal;

            this.bounds = new Rectangle(0, 0, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.minusButtonSource.Height);

            var maxRect = Game1.dialogueFont.MeasureString(this._max.ToString());
            var minRect = Game1.dialogueFont.MeasureString(this._min.ToString());

            ValueMaxLabelSize = (maxRect.X > minRect.X) ? maxRect : minRect;
        }

        protected Rectangle plusButtonbounds
        {
            get
            {
                _plusButtonbounds.X = this.bounds.Right + (int)this.ValueMaxLabelSize.X + typeExtraWidth + 4 * Game1.pixelZoom;
                _plusButtonbounds.Y = this.bounds.Y;
                return _plusButtonbounds;
            }
        }

        protected int typeExtraWidth
        {
            get
            {
                return (int)Game1.dialogueFont.MeasureString(typeExtraString).X;
            }
        }

        protected string typeExtraString
        {
            get
            {
                switch (type)
                {
                    case DisplayType.PERCENT:
                        return "%";
                    default:
                        return "";
                }
            }
        }

        private Rectangle _plusButtonbounds = new Rectangle(0, 0, Game1.pixelZoom * OptionsPlusMinus.plusButtonSource.Width, Game1.pixelZoom * OptionsPlusMinus.plusButtonSource.Height);
        readonly private DisplayType _type;
        public virtual DisplayType type => _type;


        readonly protected Vector2 ValueMaxLabelSize;

        protected Vector2 ValueLabelSize
        {
            get
            {
                return Game1.dialogueFont.MeasureString(Value.ToString());
            }
        }

        readonly private decimal _min;
        public virtual decimal min => _min;

        readonly private decimal _max;
        public virtual decimal max => _max;

        readonly private decimal _stepSize;
        public virtual decimal stepSize => _stepSize;

        private decimal _Value;
        public virtual decimal Value
        {
            get
            {
                return _Value;
            }
            protected set
            {
                var valid = CheckValidInput(Math.Round(value, 3));
                var newVal = (int)((valid - min) / stepSize) * stepSize + min;
                if (newVal != this._Value)
                {
                    this._Value = newVal;
                    this.PlusMinusValueChanged?.Invoke(this._Value);
                }
            }
        }

        protected virtual void StepUp()
        {
            this.Value += this.stepSize;
        }

        protected virtual void StepDown()
        {
            this.Value -= this.stepSize;
        }

        private decimal CheckValidInput(decimal input)
        {
            if (input > _max)
                return _max;

            if (input < _min)
                return _min;

            return input;
        }

        protected override void leftClicked(int x, int y)
        {
            base.leftClicked(x, y);

            if (this.bounds.Contains(x, y) && enabled)
            {
                this.StepDown();
            } else if (this.plusButtonbounds.Contains(x, y) && enabled)
            {
                this.StepUp();
            }
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X), (float)(this.bounds.Y)), OptionsPlusMinus.minusButtonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);

            b.DrawString(Game1.dialogueFont, Value.ToString() + typeExtraString, new Vector2((float)(this.bounds.Right + (this.plusButtonbounds.X - this.bounds.Right - ValueLabelSize.X - typeExtraWidth)/2), (float)(this.bounds.Y + ((this.bounds.Height - ValueMaxLabelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f));
            //Utility.drawBoldText(b, $"{Value.ToString()}", Game1.smallFont, new Vector2((float)(this.bounds.Right + Game1.pixelZoom * 2), (float)(this.bounds.Y + ((this.bounds.Height - valueLabelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f));

            b.Draw(Game1.mouseCursors, new Vector2((float)(this.plusButtonbounds.X), (float)(this.plusButtonbounds.Y)), OptionsPlusMinus.plusButtonSource, Color.White * ((this.enabled) ? 1f : 0.33f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);


            var labelSize = Game1.dialogueFont.MeasureString(this.label);

            Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float)(this.plusButtonbounds.Right + Game1.pixelZoom * 4), (float)(this.bounds.Y + ((this.bounds.Height - labelSize.Y) / 2))), this.enabled ? Game1.textColor : (Game1.textColor * 0.33f), 1f, 0.1f, -1, -1, 1f, 3);

        }
    }
}
