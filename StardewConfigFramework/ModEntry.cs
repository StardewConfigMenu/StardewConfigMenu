using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewConfigFramework
{
    public class ModEntry: Mod
    {

        internal static IModHelper helper;
        private Settings panel;

		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			ModEntry.helper = helper;
			this.panel = new Settings(this);
            this.StardewConfigFrameworkLoaded();
		}

		/*********
        ** Private methods
        *********/
		/// <summary>The method invoked when the game is opened.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void StardewConfigFrameworkLoaded()
		{
			this.Monitor.Log($"StardewConfigFramework Loaded");
		}
    }
}
