using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewConfigFramework;

namespace SCFTester2
{
    public class ModEntry: Mod
    {
        internal static IModSettingsFramework Settings;
		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
            Settings = IModSettingsFramework.Instance;
            var options = new ModOptions(this);
            Settings.AddModOptions(options);

            options.AddModOption(new ModOptionToggle("Test", "test"));
            options.AddModOption(new ModOptionSelection("Empty Dropdown", "empty", new System.Collections.Generic.List<string>()));
            var list = new System.Collections.Generic.List<string>();
            list.Add("Option available");
            options.AddModOption(new ModOptionSelection("Disabled Dropdown", "disabled", list, 0, false));

            var label = new ModOptionCategoryLabel("Category Label", "catlabel");
            options.AddModOption(label);
        }

        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the game is opened.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void StardewConfigFrameworkLoaded()
		{

		}
    }
}
