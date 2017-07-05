﻿using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewConfigFramework;
using System.Collections.Specialized;

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

            var disableDrop = new ModOptionToggle("toggle", "Checkbox");
            options.AddModOption(disableDrop);

            disableDrop.ValueChanged += DisableDrop_ValueChanged;

            dropdown = new ModOptionSelection("drop", "Dropdown");

			dropdown.Choices.Add("toggle", "Toggle");
			dropdown.Choices.Add("on", "Always On");
            dropdown.Choices.Add("off", "Always Off");

            options.AddModOption(dropdown);

            dropdown.ValueChanged += Dropdown_ValueChanged;

            var checkbox2 = new ModOptionToggle("toggle2", "Checkbox2" );

            options.AddModOption(checkbox2);

            options.AddModOption(new ModOptionToggle("toggle3", "Always On"));

            GraphicsEvents.OnPostRenderEvent += (sender, e) =>
            {

                if (dropdown.Selection == "off")
                    checkbox2.IsOn = false;
                if (dropdown.Selection == "on" || (options.GetOptionWithIdentifier("toggle3") as ModOptionToggle).IsOn)
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdown.Choices.LabelOf("on"), new Vector2(Game1.getMouseX(), Game1.getMouseY()), Color.Black);
                if (toggledOn)
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdown.Choices.LabelOf("toggle"), new Vector2(Game1.getMouseX(), Game1.getMouseY() + 12 * Game1.pixelZoom), Color.Black);
            };


            options.AddModOption(new ModOptionToggle("toggle4", "Checkbox4"));
            options.AddModOption(new ModOptionToggle("toggle5", "Checkbox5"));
            options.AddModOption(new ModOptionToggle("toggle6", "Checkbox6"));
            options.AddModOption(new ModOptionToggle("toggle7", "Checkbox7"));
            options.AddModOption(new ModOptionToggle("toggle8", "Checkbox8"));
        }

        private ModOptionSelection dropdown;

        private void DisableDrop_ValueChanged(string identifier, bool isOn)
        {
            dropdown.enabled = isOn;
        }

        private bool toggledOn = false;

        private void Dropdown_ValueChanged(string identifier, string selection)
        {
            if (selection == "toggle")
                toggledOn = !toggledOn;
        }

        /*********
        ** Private methods
        *********/

        private void StardewConfigFrameworkLoaded()
		{

		}
    }
}
