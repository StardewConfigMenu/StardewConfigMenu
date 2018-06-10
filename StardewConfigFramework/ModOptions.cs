using StardewModdingAPI;
using System.Collections.Generic;
using System.IO;
using System;

namespace StardewConfigFramework {
	public class ModOptions {

		public IManifest modManifest { get; private set; }

		public SemanticVersion OptionsVersion;

		internal protected IModHelper Helper;

		public IList<ModOption> List {
			get {
				return OptionList.AsReadOnly();
			}
		}

		internal List<ModOption> OptionList { get; set; } = new List<ModOption>();

		public ModOptions(Mod mod) {
			//this.modName = modName;
			this.modManifest = mod.ModManifest;
			this.OptionsVersion = mod.ModManifest.Version as SemanticVersion;
			this.Helper = mod.Helper;
		}

		internal ModOptions(SemanticVersion version, List<ModOption> List) {
			this.OptionList = List;
			this.OptionsVersion = version;
		}

		private void UpdateManifest(IManifest manifest) {
			this.modManifest = manifest;
		}

		public T GetOptionWithIdentifier<T>(string identifier) where T : ModOption {
			return OptionList.Find(x => x.identifier == identifier) as T;
		}

		public ModOption GetOptionWithIdentifier(string identifier) {
			return OptionList.Find(x => x.identifier == identifier);
		}

		public Type GetTypeOfIdentifier(string identifier) {
			return GetOptionWithIdentifier(identifier).GetType();
		}

		public ModOption RemoveAtIndex(int index) {
			var old = OptionList[index];
			this.OptionList.RemoveAt(index);
			return old;
		}

		// Add remove method
		public ModOption RemoveModOptionWithIdentifier(string identifier) {
			var old = this.OptionList.Find(x => { return x.identifier == identifier; });
			OptionList.Remove(old);
			return old;
		}

		public void AddModOption(ModOption option) {
			RemoveModOptionWithIdentifier(option.identifier);
			this.OptionList.Add(option);
		}

		public void InsertModOption(ModOption option, int index) {
			this.RemoveModOptionWithIdentifier(option.identifier);
			this.OptionList.Insert(index, option);
		}

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", true)]
		public static ModOptions LoadUserSettings(Mod mod) {
			mod.Monitor.Log("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu");
			return new ModOptions(mod);
		}

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", true)]
		public void SaveUserSettings() { }

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", true)]
		public static ModOptions LoadCharacterSettings(Mod mod, string farmerName) {
			mod.Monitor.Log("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu");
			return new ModOptions(mod);
		}

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", true)]
		public void SaveCharacterSettings(string farmerName) { }
	}
}
