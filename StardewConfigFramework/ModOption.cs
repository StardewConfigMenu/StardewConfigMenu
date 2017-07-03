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
        public string LabelText { get; protected set; }
        public string identifier { get; protected set; }
        public bool enabled;

        internal ModOption(String identifier, String label, bool enabled = true)
        {
            this.identifier = identifier;
            this.LabelText = label;
            this.enabled = enabled;
        }
    }
}
