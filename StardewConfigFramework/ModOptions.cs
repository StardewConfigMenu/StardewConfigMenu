using StardewModdingAPI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace StardewConfigFramework
{
	public class ModOptions
	{
		public IManifest modManifest { get; private set; }
		private IModHelper Helper;

		public IList<ModOption> List
		{
			get
			{
				return OptionList.AsReadOnly();
			}
		}

		internal List<ModOption> OptionList { get; set; } = new List<ModOption>();

		public ModOptions(Mod mod)
		{
			//this.modName = modName;
			this.modManifest = mod.ModManifest;
			this.Helper = mod.Helper;

		}

		public ModOption GetOptionWithIdentifier(string identifier)
		{
			return OptionList.Find(x => x.identifier == identifier);
		}

		public ModOption RemoveAtIndex(int index)
		{
			var old = OptionList[index];
			this.OptionList.RemoveAt(index);
			return old;
		}

		// Add remove method
		public ModOption RemoveModOptionWithIdentifier(string identifier)
		{
			var old = this.OptionList.Find(x => { return x.identifier == identifier; });
			OptionList.Remove(old);
			return old;
		}

		public void AddModOption(ModOption option)
		{
			RemoveModOptionWithIdentifier(option.identifier);
			this.OptionList.Add(option);
		}

		public void InsertModOption(ModOption option, int index)
		{
			this.RemoveModOptionWithIdentifier(option.identifier);
			this.OptionList.Insert(index, option);
		}

		public static ModOptions LoadUserSettings(Mod mod)
		{
			var path = Path.Combine(mod.Helper.DirectoryPath, "StardewConfig.json");

			return mod.Helper.ReadJsonFile<ModOptions>(path) ?? new ModOptions(mod);
		}

		public void SaveUserSettings()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};

			string strJson = JsonConvert.SerializeObject(this, settings);

			var path = Path.Combine(Helper.DirectoryPath, "StardewConfig.json");

			this.Helper.WriteJsonFile(path, this);

		}
	}
}