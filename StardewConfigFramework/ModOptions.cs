using StardewModdingAPI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using StardewConfigFramework.Serialization;
using System;

namespace StardewConfigFramework {
	public class ModOptions {

		[JsonIgnore]
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

		[JsonConstructor]
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

		private static JsonSerializerSettings serializer = new JsonSerializerSettings {
			Formatting = Formatting.Indented//,
																			//TypeNameHandling = TypeNameHandling.Auto
		};

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", false)]
		public static ModOptions LoadUserSettings(Mod mod) {
			var path = Path.Combine(mod.Helper.DirectoryPath, "StardewConfig.json");


			try {
				string json = File.ReadAllText(path);
				var options = JsonConvert.DeserializeObject<ModOptions>(json, new ModOptionsConverter());
				options.UpdateManifest(mod.ModManifest);
				options.Helper = mod.Helper;
				return options;
			} catch {
				mod.Monitor.Log("No Config Data Found, Starting New Options List");
				return new ModOptions(mod);
			}
		}

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", false)]
		public void SaveUserSettings() {

			var path = Path.Combine(Helper.DirectoryPath, "StardewConfig.json");

			this.Helper.WriteJsonFile("StardewConfig.json", this);

			//File.WriteAllText(path, json);
		}

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", true)]
		public static ModOptions LoadCharacterSettings(Mod mod, string farmerName) {
			var path = Path.Combine(mod.Helper.DirectoryPath, $"StardewConfig-{farmerName}.json");


			try {
				string json = File.ReadAllText(path);
				var options = JsonConvert.DeserializeObject<ModOptions>(json, new ModOptionsConverter());
				options.UpdateManifest(mod.ModManifest);
				options.Helper = mod.Helper;
				return options;
			} catch {
				mod.Monitor.Log($"No Config Data Found For Farmer {farmerName}, Starting New Options List");
				return new ModOptions(mod);
			}
		}

		[Obsolete("Saving/Loading settings within the framework is deprecated. Please use SMAPI methods for saving mod settings and use initial values when populating settings menu", true)]
		public void SaveCharacterSettings(string farmerName) {

			this.Helper.WriteJsonFile($"StardewConfig-{farmerName}.json", this);

		}
	}
}
