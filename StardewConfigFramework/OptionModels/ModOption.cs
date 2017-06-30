﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewConfigFramework
{
    public abstract class ModOption
    {
        internal string LabelText;
        internal bool enabled;

        internal ModOption(String text, bool enabled = true)
        {
            this.LabelText = text;
            this.enabled = enabled;
        }
    }
}
