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
        public string LabelText { get; set; }
        readonly public string identifier;
        public bool enabled;

        internal protected ModOption(string identifier, string label, bool enabled = true)
        {
            this.identifier = identifier;
            this.LabelText = label;
            this.enabled = enabled;
        }
    }
}
