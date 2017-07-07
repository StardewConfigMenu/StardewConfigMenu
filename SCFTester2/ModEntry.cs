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
            //var options = ModOptions.LoadUserSettings(this);
            Settings.AddModOptions(options);

            options.AddModOption(new ModOptionToggle("test", "Test"));
            options.AddModOption(new ModOptionSelection("empty", "Empty Dropdown", new ModSelectionOptionChoices()));
            var list = new ModSelectionOptionChoices();
            list.Add("available","Option Available");
            options.AddModOption(new ModOptionSelection("disabled",  "Disabled Dropdown", list, 0, false));

            var label = new ModOptionCategoryLabel("catlabel", "Category Label");
            options.AddModOption(label);

            var button = new ModOptionTrigger("setButton", "Click Me!", OptionActionType.SET);
            button.ActionTriggered += (identifier) => {
                options.GetOptionWithIdentifier("disabled").enabled = !options.GetOptionWithIdentifier("disabled").enabled;
            };
            options.AddModOption(button);

            var clearButton = new ModOptionTrigger("clearButton", "Clear Button", OptionActionType.CLEAR);
            clearButton.ActionTriggered += (identifier) => {
                switch (clearButton.LabelText) {
					case "Clear Button":
						clearButton.LabelText = "Are you sure?";
                        clearButton.type = OptionActionType.OK;
						break;
					case "Are you sure?":
						clearButton.LabelText = "Cleared";
                        clearButton.type = OptionActionType.DONE;
						break;
					case "Cleared":
						clearButton.LabelText = "Clear Button";
                        clearButton.type = OptionActionType.CLEAR;
						break;
                }
            };

            options.AddModOption(clearButton);

			options.AddModOption(new ModOptionTrigger("doneButton", "Done Button", OptionActionType.DONE));
            options.AddModOption(new ModOptionTrigger("giftButton", "Gift Button", OptionActionType.GIFT));

            var saveButton = new ModOptionTrigger("okButton", "OK Button", OptionActionType.OK);
            options.AddModOption(saveButton);

            saveButton.ActionTriggered += (id) =>
            {
                //options.SaveUserSettings();
            };


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
