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
        public bool enabled { get; protected set; }

        internal ModOption(String text, String identifier, bool enabled = true)
        {
            this.identifier = identifier;
            this.LabelText = text;
            this.enabled = enabled;
        }
    }

    public interface IModOption
    {
        void draw();
    }
}
