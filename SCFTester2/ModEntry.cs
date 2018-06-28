using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewConfigFramework;
using StardewConfigFramework.Options;
using System.Collections.Generic;

namespace SCFTester2 {
	using Action = StardewConfigFramework.Options.Action;

	public class ModEntry: Mod {
		void Testbox_ValueDidChange(string identifier, bool isOn) {
		}

		internal static IConfigMenu Settings;
		internal static SimpleOptionsPackage Options;
		/*********
		** Public methods
		*********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper) {
			Settings = IConfigMenu.Instance;
			Options = new SimpleOptionsPackage(this);
			var config = this.Helper.ReadConfig<TestConfig>();
			Settings.AddOptionsPackage(Options);

			var testbox = new Toggle("checkbox", "Checkbox", config.checkbox);
			Options.AddOption(testbox);

			var emptyDropdown = new Selection("emptyDropdown", "Empty Dropdowns are disabled");
			Options.AddOption(emptyDropdown);

			testbox.StateDidChange += (toggle) => {
				emptyDropdown.Enabled = toggle.IsOn; // should not do anything
			};

			var list = new List<ISelectionChoice>();
			list.Add(new SelectionChoice("first", "First", "This is the first option!"));
			list.Add(new SelectionChoice("second", "Second", "This is the Second option!"));
			list.Add(new SelectionChoice("third", "Third"));
			list.Add(new SelectionChoice("fourth", "Fourth"));

			var filledDropdown = new Selection("filledDropdown", "Filled Dropdown", list, config.filledDropown, true);
			Options.AddOption(filledDropdown);

			var stepper = new Stepper("stepper", "Plus/Minus Controls", (decimal) 5.0, (decimal) 105.0, (decimal) 1.5, config.stepperValue, RangeDisplayType.PERCENT);
			Options.AddOption(stepper);

			var label = new CategoryLabel("catlabel", "Category Label");
			Options.AddOption(label);

			var button = new Action("setButton", "Click Me!", ButtonType.SET);
			button.ActionWasTriggered += (identifier) => {
				filledDropdown.Enabled = !filledDropdown.Enabled;
			};
			Options.AddOption(button);

			var tranformingButton = new Action("clearButton", "Clear Button", ButtonType.CLEAR);
			tranformingButton.ButtonType = ButtonType.CLEAR;
			tranformingButton.ActionWasTriggered += (identifier) => {
				switch (tranformingButton.ButtonType) {
					case ButtonType.CLEAR:
						tranformingButton.Label = "Are you sure?";
						tranformingButton.ButtonType = ButtonType.OK;
						break;
					case ButtonType.OK:
						tranformingButton.Label = "Cleared";
						tranformingButton.ButtonType = ButtonType.DONE;
						break;
					case ButtonType.DONE:
						tranformingButton.Label = "Clear Button";
						tranformingButton.ButtonType = ButtonType.CLEAR;
						break;
					default:
						tranformingButton.Label = "Clear Button";
						tranformingButton.ButtonType = ButtonType.CLEAR;
						break;
				}
			};

			Options.AddOption(tranformingButton);

			Options.AddOption(new Action("doneButton", "Done Button", ButtonType.DONE));
			Options.AddOption(new Action("giftButton", "Gift Button", ButtonType.GIFT));

			var saveButton = new Action("okButton", "OK Button", ButtonType.OK);
			Options.AddOption(saveButton);

			saveButton.ActionWasTriggered += (_) => {
				SaveConfig();
			};

			SaveEvents.AfterLoad += SaveEvents_AfterLoad;
		}

		private void SaveEvents_AfterLoad(object sender, EventArgs e) {

		}

		private void SaveConfig() {
			var config = new TestConfig();

			config.checkbox = Options.GetOption<Toggle>("checkbox").IsOn;
			config.filledDropown = Options.GetOption<Selection>("filledDropdown").SelectedIdentifier;
			config.stepperValue = Options.GetOption<Stepper>("stepper").Value;
			Helper.WriteConfig<TestConfig>(config);
		}


		/*********
		** Private methods
		*********/
		/// <summary>The method invoked when the game is opened.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void StardewConfigFrameworkLoaded() {

		}
	}
}
