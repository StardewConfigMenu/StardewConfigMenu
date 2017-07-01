using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

namespace StardewConfigFramework
{

	public delegate void ModAddedSettings();

	public abstract class IModSettingsFramework
    {
        public static IModSettingsFramework Instance { get; protected set; }
        public abstract void AddModOptions(ModOptions modOptions);
    }

}
