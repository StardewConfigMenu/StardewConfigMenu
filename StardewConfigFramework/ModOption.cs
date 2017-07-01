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
        public string LabelText { get; private set; }
        public bool enabled { get; private set; }

    internal ModOption(String text, bool enabled = true)
        {
            this.LabelText = text;
            this.enabled = enabled;
        }
    }
}
