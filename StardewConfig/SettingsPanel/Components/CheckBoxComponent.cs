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
    internal delegate void CheckBoxToggled(int selected);

    internal class CheckBoxComponent: OptionComponent
    {

        internal event CheckBoxToggled CheckBoxToggled;
        //
        // Static Fields
        //

        public override bool enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        private bool _enabled;

        protected bool IsOn;

        public static Rectangle sourceUnchecked = new Rectangle(227, 425, 9, 9);

        public static Rectangle sourceChecked = new Rectangle(236, 425, 9, 9);

        //
        // Fields
        //

        internal CheckBoxComponent(string label, bool isOn, int x, int y, bool enabled = true) : base(label, enabled)
        {
            this.bounds = new Rectangle();
            this.IsOn = isOn;
        }

        internal CheckBoxComponent(string label, bool isOn, bool enabled = true) : base(label, enabled)
        {
            this.IsOn = isOn;
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

        }

    }

}

