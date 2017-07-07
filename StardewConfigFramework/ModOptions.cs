using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace StardewConfigFramework
{
    public class ModOptions
    {
        public IManifest modManifest { get; private set; }

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
        }

        public ModOption GetOptionWithIdentifier(string identifier)
        {
			return OptionList.Find(x => x.identifier == identifier);
        }

        public ModOption RemoveAtIndex(int index)  {
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

        /* TODO
        public static ModOptions LoadUserSettings(Mod mod)
        {
            return IModSettingsFramework.Instance.LoadModOptions(mod);
        }

        public void SaveUserSettings()
        {
            IModSettingsFramework.Instance.SaveModOptions(this);
        }*/
    }
}