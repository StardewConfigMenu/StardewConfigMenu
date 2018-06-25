using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewConfigFramework;
using System.Collections.Specialized;
using System.Collections.Generic;
using StardewConfigFramework.Options;

namespace SCFTester {
	public class ModEntry: Mod {
		internal static IConfigMenu Settings;
		internal static TabbedOptionsPackage Options;
		/*********
		** Public methods
		*********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper) {
			Settings = IConfigMenu.Instance;
			//var options = new ModOptions(this);
			var config = this.Helper.ReadConfig<ModConfig>();
			Options = new TabbedOptionsPackage(this);
			Settings.AddOptionsPackage(Options);

			GenerateOptions(Options, config);
		}

		private void GenerateOptions(TabbedOptionsPackage options, ModConfig config) {
			var firstTab = new OptionsTab("Main");

			var enableDrop = new Toggle("enableDrop", "Enable Dropdown", config.enableDropdown);
			firstTab.AddOption(enableDrop);

			var choices = new List<SelectionChoice> {
				new SelectionChoice("none", "None"),
				new SelectionChoice("5", "Checkbox 5", "Hover text for Checkbox 5"),
				new SelectionChoice("6", "Checkbox 6", "Hover text for Checkbox 6"),
				new SelectionChoice("7", "Checkbox 7", "Hover text for Checkbox 7")
			};

			var dropdown = new Selection("drop", "Disable Another Option", choices, config.dropdownChoice, config.enableDropdown);
			dropdown.SelectionDidChange += Dropdown_SelectionDidChange; ;
			firstTab.AddOption(dropdown);

			enableDrop.StateDidChange += (toggle) => {
				dropdown.Enabled = toggle.IsOn;
			};

			var checkbox2 = new Toggle("toggle2", "Checkbox 2", config.checkbox2);
			firstTab.AddOption(checkbox2);
			checkbox2.StateDidChange += (toggle) => {
				if (toggle.IsOn) {
					var targetIndex = firstTab.IndexOf("toggle8") + 1;
					firstTab.InsertOption(new Toggle("toggle9", "Added dynamically!"), targetIndex);
				} else {
					firstTab.RemoveOption("toggle9");
				}
			};

			firstTab.AddOption(new Toggle("toggle3", "Checkbox 3", false));

			var slider = new Range("range", "Slider", 10, 25, 1, config.rangeValue, true);

			var stepper = new Stepper("stepper", "Plus/Minus Controls", (decimal) 5.0, (decimal) 105.0, (decimal) 1.5, config.stepperValue, Stepper.DisplayType.PERCENT);

			firstTab.AddOption(slider);
			firstTab.AddOption(stepper);

			firstTab.AddOption(new Toggle("stepperCheck", "Show Stepper Value", false));

			firstTab.AddOption(new Toggle("toggle5", "Checkbox 5"));
			firstTab.AddOption(new Toggle("toggle6", "Checkbox 6"));
			firstTab.AddOption(new Toggle("toggle7", "Checkbox 7"));
			firstTab.AddOption(new Toggle("toggle8", "Checkbox 8"));

			var saveButton = new Action("okButton", "OK Button", Action.ActionType.OK);
			firstTab.AddOption(saveButton);

			saveButton.ActionWasTriggered += SaveButton_ActionWasTriggered;

			GraphicsEvents.OnPostRenderEvent += (sender, e) => {

				if (options.Tabs[0].GetOption<Toggle>("toggle3").IsOn)
					Game1.spriteBatch.DrawString(Game1.dialogueFont, "Cool!", new Vector2(Game1.getMouseX(), Game1.getMouseY()), Color.Black);

				if (options.Tabs[0].GetOption<Toggle>("stepperCheck").IsOn) {
					Game1.spriteBatch.DrawString(Game1.dialogueFont, stepper.Value.ToString(), new Vector2(Game1.getMouseX(), Game1.getMouseY() + 12 * Game1.pixelZoom), Color.Black);
				}
			};
		}

		void Dropdown_SelectionDidChange(Selection selection) {
			var selected = selection.SelectedIdentifier;
			Options.Tabs[0].GetOption<Toggle>("toggle5").Enabled = "5" == selected;
			Options.Tabs[0].GetOption<Toggle>("toggle6").Enabled = "6" == selected;
			Options.Tabs[0].GetOption<Toggle>("toggle7").Enabled = "7" == selected;
		}

		void SaveButton_ActionWasTriggered(Action action) {
			var config = new ModConfig() {
				dropdownChoice = Options.Tabs[0].GetOption<Selection>("drop").SelectedIdentifier,
				enableDropdown = Options.Tabs[0].GetOption<Toggle>("enableDrop").IsOn,
				checkbox2 = Options.Tabs[0].GetOption<Toggle>("toggle2").IsOn
			};
			this.Helper.WriteConfig<ModConfig>(config);
		}


		/*********
		** Private methods
		*********/

		private void StardewConfigFrameworkLoaded() {

		}
	}
}
