using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;


namespace StardewConfigFramework
{
    public class ModOptions
    {
        internal String modName;

        public List<ModOption> list = new List<ModOption>();

        public ModOptions(ref Mod mod) {
            //this.modName = modName;
            this.modName = mod.ModManifest.Name;
			//Load from JSON using Settings

			switch (this.LoadUserSettings())
			{
				case SETTINGS_LOAD_RESPONSE.NONE:
                    Settings.Log($"User has no saved settings file, creating");
					break;
				case SETTINGS_LOAD_RESPONSE.SUCCESS:
					Settings.Log($"User data successfully loaded");
					break;
				case SETTINGS_LOAD_RESPONSE.UNKNOWN:
					Settings.Log($"Failed to load user data");
					break;
			}
        }

        public void AddModOption(ref ModOption option) {
            list.Add(option);
        }

        internal ModOption[] GetArray() {
            return list.ToArray();
        }

		private enum SETTINGS_LOAD_RESPONSE
		{
			UNKNOWN, SUCCESS, NONE
		}

		private SETTINGS_LOAD_RESPONSE LoadUserSettings()
		{
            return SETTINGS_LOAD_RESPONSE.UNKNOWN;
		}
    }
}
