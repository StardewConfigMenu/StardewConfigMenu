using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewConfigFramework;
using System.Collections.Generic;

namespace SCFTester
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

            options.AddModOption(new ModOptionToggle("Test", "toggle"));

            var dropdownoptions = new List<string>();
            dropdownoptions.Add("Toggle");
            dropdownoptions.Add("Always On");
            var dropdown = new ModOptionSelection("Dropdown", "drop", dropdownoptions);

            options.AddModOption(dropdown);

            dropdown.ValueChanged += Dropdown_ValueChanged;

            GraphicsEvents.OnPostRenderEvent += (sender, e) =>
            {

                if (dropdown.Selection == 1 || toggledOn)
                {
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdownoptions[dropdown.Selection], new Vector2(Game1.getMouseX(), Game1.getMouseY()),Color.Black );
                }
            };
        }

        private bool toggledOn = false;

        private void Dropdown_ValueChanged(int selection)
        {
            if (selection == 1)
                toggledOn = !toggledOn;
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
