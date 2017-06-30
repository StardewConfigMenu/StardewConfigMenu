using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;


namespace StardewConfigFramework
{
    internal class ModPage
	{
        private ModOptions Options { get; }

        internal ModPage(ModOptions modOptions) {
            this.Options = modOptions;
        }

	}
}
