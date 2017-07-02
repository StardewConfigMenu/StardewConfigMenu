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

    internal class ModCheckBoxComponent: CheckBoxComponent
    {
        //
        // Static Fields
        //
        //public const int pixelsHigh = 11;

        //
        // Fields
        //

        internal ModCheckBoxComponent(ModOptionToggle option, int x, int y, bool enabled = true) : base(option.LabelText, enabled)
        {

        }

        internal ModCheckBoxComponent(ModOptionToggle option, bool enabled = true) : base(option.LabelText, enabled)
        {

        }

    }

}

