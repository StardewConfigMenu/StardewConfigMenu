using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewConfigFramework;
using System.Collections.Specialized;

namespace SCFTester {
	public class ModEntry: Mod {
		internal static IModSettingsFramework Settings;
		/*********
		** Public methods
		*********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper) {
			Settings = IModSettingsFramework.Instance;
			//var options = new ModOptions(this);
			var options = ModOptions.LoadUserSettings(this);
			Settings.AddModOptions(options);


			var disableDrop = options.GetOptionWithIdentifier<ModOptionToggle>("toggle") ?? new ModOptionToggle("toggle", "Checkbox");
			options.AddModOption(disableDrop);

			disableDrop.ValueChanged += DisableDrop_ValueChanged;

			dropdown = options.GetOptionWithIdentifier<ModOptionSelection>("drop") ?? new ModOptionSelection("drop", "Dropdown");
			dropdown.Choices.Replace("toggle", "Toggle");
			dropdown.Choices.Replace("on", "Always On");
			dropdown.Choices.Replace("off", "Always Off");

			options.AddModOption(dropdown);

			dropdown.ValueChanged += Dropdown_ValueChanged;

			var checkbox2 = options.GetOptionWithIdentifier<ModOptionToggle>("toggle2") ?? new ModOptionToggle("toggle2", "Checkbox2");
			options.AddModOption(checkbox2);

			options.AddModOption(new ModOptionToggle("toggle3", "Always On", false));

			var slider = options.GetOptionWithIdentifier<ModOptionRange>("range") ?? new ModOptionRange("range", "Slider", 10, 25, 1, 15, true);

			var stepper = options.GetOptionWithIdentifier<ModOptionStepper>("stepper") ?? new ModOptionStepper("stepper", "Plus/Minus Controls", (decimal) 5.0, (decimal) 105.0, (decimal) 1.5, 26, DisplayType.PERCENT);

			options.AddModOption(slider);
			options.AddModOption(stepper);

			options.AddModOption(new ModOptionToggle("stepperCheck", "Show Stepper Value", false));

			options.AddModOption(new ModOptionToggle("toggle5", "Checkbox5"));
			options.AddModOption(new ModOptionToggle("toggle6", "Checkbox6"));
			options.AddModOption(new ModOptionToggle("toggle7", "Checkbox7"));
			options.AddModOption(new ModOptionToggle("toggle8", "Checkbox8"));

			var saveButton = new ModOptionTrigger("okButton", "OK Button", OptionActionType.OK);
			options.AddModOption(saveButton);

			saveButton.ActionTriggered += (id) => {
				options.SaveUserSettings();
			};


			GraphicsEvents.OnPostRenderEvent += (sender, e) => {

				if (dropdown.Selection == "off")
					checkbox2.IsOn = false;
				if (dropdown.Selection == "on" || (options.GetOptionWithIdentifier("toggle3") as ModOptionToggle).IsOn)
					Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdown.Choices.LabelOf("on"), new Vector2(Game1.getMouseX(), Game1.getMouseY()), Color.Black);
				if (toggledOn)
					Game1.spriteBatch.DrawString(Game1.dialogueFont, dropdown.Choices.LabelOf("toggle"), new Vector2(Game1.getMouseX(), Game1.getMouseY() + 12 * Game1.pixelZoom), Color.Black);

				if ((options.GetOptionWithIdentifier("stepperCheck") as ModOptionToggle).IsOn) {
					Game1.spriteBatch.DrawString(Game1.dialogueFont, stepper.Value.ToString(), new Vector2(Game1.getMouseX(), Game1.getMouseY() + 12 * Game1.pixelZoom), Color.Black);
				}
			};
		}

		private ModOptionSelection dropdown;

		private void DisableDrop_ValueChanged(string identifier, bool isOn) {
			dropdown.enabled = isOn;
		}

		private bool toggledOn = false;

		private void Dropdown_ValueChanged(string identifier, string selection) {
			if (selection == "toggle")
				toggledOn = !toggledOn;
		}

		/*********
		** Private methods
		*********/

		private void StardewConfigFrameworkLoaded() {

		}
	}
}
