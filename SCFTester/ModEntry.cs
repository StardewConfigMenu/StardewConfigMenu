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
		internal static TabbedOptionsPackage Package;
		/*********
		** Public methods
		*********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper) {
			Settings = IConfigMenu.Instance;
			//var options = new ModOptions(this);
			var config = this.Helper.ReadConfig<ModConfig>();
			Package = new TabbedOptionsPackage(this);

			GenerateOptions(Package, config);
			Settings.AddOptionsPackage(Package);
		}

		private void GenerateOptions(TabbedOptionsPackage options, ModConfig config) {
			var firstTab = new OptionsTab("Main");
			options.Tabs.Add(firstTab);

			var enableDrop = new Toggle("enableDrop", "Enable Dropdown", config.enableDropdown);
			firstTab.Options.Add(enableDrop);

			var choices = new List<ISelectionChoice> {
				new SelectionChoice("none", "None"),
				new SelectionChoice("5", "Checkbox 5", "Hover text for Checkbox 5"),
				new SelectionChoice("6", "Checkbox 6", "Hover text for Checkbox 6"),
				new SelectionChoice("7", "Checkbox 7", "Hover text for Checkbox 7")
			};

			var dropdown = new Selection("drop", "Disable Another Option", choices, config.dropdownChoice, config.enableDropdown);
			dropdown.SelectionDidChange += Dropdown_SelectionDidChange; ;
			firstTab.Options.Add(dropdown);

			enableDrop.StateDidChange += (toggle) => {
				dropdown.Enabled = toggle.IsOn;
			};

			var checkbox2 = new Toggle("toggle2", "Checkbox 2", config.checkbox2);
			firstTab.Options.Add(checkbox2);
			checkbox2.StateDidChange += (toggle) => {
				if (toggle.IsOn) {
					var targetIndex = firstTab.Options.IndexOf("toggle8") + 1;
					firstTab.Options.Insert(targetIndex, new Toggle("toggle9", "Added dynamically!"));
				} else {
					firstTab.Options.Remove("toggle9");
				}
			};

			firstTab.Options.Add(new Toggle("toggle3", "Checkbox 3", false));

			var slider = new Range("range", "Slider", 10, 25, 1, config.rangeValue, true);

			var stepper = new Stepper("stepper", "Plus/Minus Controls", (decimal) 5.0, (decimal) 105.0, (decimal) 1.5, config.stepperValue, Stepper.DisplayType.PERCENT);

			firstTab.Options.Add(slider);
			firstTab.Options.Add(stepper);

			firstTab.Options.Add(new Toggle("stepperCheck", "Show Stepper Value", false));

			firstTab.Options.Add(new Toggle("toggle5", "Checkbox 5"));
			firstTab.Options.Add(new Toggle("toggle6", "Checkbox 6"));
			firstTab.Options.Add(new Toggle("toggle7", "Checkbox 7"));
			firstTab.Options.Add(new Toggle("toggle8", "Checkbox 8"));

			var saveButton = new Action("okButton", "OK Button", Action.ActionType.OK);
			firstTab.Options.Add(saveButton);

			saveButton.ActionWasTriggered += SaveButton_ActionWasTriggered;

			GraphicsEvents.OnPostRenderEvent += (sender, e) => {
				return;

				if (firstTab.GetOption<Toggle>("toggle3").IsOn)
					Game1.spriteBatch.DrawString(Game1.dialogueFont, "Cool!", new Vector2(Game1.getMouseX(), Game1.getMouseY()), Color.Black);

				if (firstTab.GetOption<Toggle>("stepperCheck").IsOn) {
					Game1.spriteBatch.DrawString(Game1.dialogueFont, stepper.Value.ToString(), new Vector2(Game1.getMouseX(), Game1.getMouseY() + 12 * Game1.pixelZoom), Color.Black);
				}
			};
		}

		void Dropdown_SelectionDidChange(Selection selection) {
			var selected = selection.SelectedIdentifier;
			Package.Tabs[0].GetOption<Toggle>("toggle5").Enabled = "5" == selected;
			Package.Tabs[0].GetOption<Toggle>("toggle6").Enabled = "6" == selected;
			Package.Tabs[0].GetOption<Toggle>("toggle7").Enabled = "7" == selected;
		}

		void SaveButton_ActionWasTriggered(Action action) {
			var config = new ModConfig() {
				dropdownChoice = Package.Tabs[0].GetOption<Selection>("drop").SelectedIdentifier,
				enableDropdown = Package.Tabs[0].GetOption<Toggle>("enableDrop").IsOn,
				checkbox2 = Package.Tabs[0].GetOption<Toggle>("toggle2").IsOn
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
