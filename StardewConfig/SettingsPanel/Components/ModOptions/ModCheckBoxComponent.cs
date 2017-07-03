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

namespace StardewConfigMenu.Panel.Components.ModOptions
{

    internal class ModCheckBoxComponent : CheckBoxComponent
    {
        //
        // Static Fields
        //
        //public const int pixelsHigh = 11;

        //
        // Fields
        //
        private ModOptionToggle option;

        public override bool IsChecked {
            get { return option.IsOn;  }
            protected set
            {
                if (option == null)
                    return; // used to ignore base class assignment
                this.option.IsOn = value;
            }
        }

        internal ModCheckBoxComponent(ModOptionToggle option, int x, int y, bool enabled = true) : base(option.LabelText, option.IsOn, x, y, enabled)
        {
            this.option = option;
        }

        internal ModCheckBoxComponent(ModOptionToggle option, bool enabled = true) : base(option.LabelText, option.IsOn, enabled)
        {
            this.option = option;
        }

    }

}

