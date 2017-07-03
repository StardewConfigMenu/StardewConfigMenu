using System;
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

            options.GetOptionWithIdentifier("");

            var disableDrop = new ModOptionToggle("Checkbox", "toggle");
            options.AddModOption(disableDrop);

            disableDrop.ValueChanged += DisableDrop_ValueChanged;

            dropdown = new ModOptionSelection("Dropdown", "drop");

			dropdown.Choices.Add("toggle", "Toggle");
			dropdown.Choices.Add("on", "Always On");
            dropdown.Choices.Add("off", "Always Off");

            options.AddModOption(dropdown);

            dropdown.ValueChanged += Dropdown_ValueChanged;

            var checkbox2 = new ModOptionToggle("Checkbox2", "toggle2");

            options.AddModOption(checkbox2);


            GraphicsEvents.OnPostRenderEvent += (sender, e) =>
            {

                if (dropdown.Selection == 2)
                    checkbox2.IsOn = false;
                if (dropdown.Selection == 1)
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdown.Choices.LabelOf(1), new Vector2(Game1.getMouseX(), Game1.getMouseY()), Color.Black);
                if (toggledOn)
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdown.Choices.LabelOf(0), new Vector2(Game1.getMouseX(), Game1.getMouseY() + 12 * Game1.pixelZoom), Color.Black);
            };


            options.AddModOption(new ModOptionToggle("Checkbox3", "toggle3"));
            options.AddModOption(new ModOptionToggle("Checkbox4", "toggle4"));
            options.AddModOption(new ModOptionToggle("Checkbox5", "toggle5"));
            options.AddModOption(new ModOptionToggle("Checkbox6", "toggle6"));
            options.AddModOption(new ModOptionToggle("Checkbox7", "toggle7"));
            options.AddModOption(new ModOptionToggle("Checkbox8", "toggle8"));
        }

        private ModOptionSelection dropdown;

        private void DisableDrop_ValueChanged(bool isOn)
        {
            dropdown.enabled = isOn;
        }

        private bool toggledOn = false;

        private void Dropdown_ValueChanged(int selection)
        {
            if (selection == 0)
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
