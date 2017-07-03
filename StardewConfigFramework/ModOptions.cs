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
        public IManifest modManifest { get; private set; }

        public List<ModOption> list = new List<ModOption>();

        public ModOptions(Mod mod)
        {
            //this.modName = modName;
            this.modManifest = mod.ModManifest;
            //Load from JSON using Settings

            switch (this.LoadUserSettings())
            {
                case SETTINGS_LOAD_RESPONSE.NONE:
                    mod.Monitor.Log($"User has no saved settings file, creating");
                    break;
                case SETTINGS_LOAD_RESPONSE.SUCCESS:
                    mod.Monitor.Log($"User data successfully loaded");
                    break;
                case SETTINGS_LOAD_RESPONSE.UNKNOWN:
                    mod.Monitor.Log($"Failed to load user data");
                    break;
            }
        }

        public ModOption GetOptionWithIdentifier(String identifier)
        {
            try
            {
                return list.Find(x => x.identifier == identifier);
            } catch
            {
                return null;
            }
        }

        public void AddModOption(ModOption option)
        {
            try
            {
                var old = this.list.Find(x => { return x.identifier == option.identifier; });
                list.Remove(old);
            } finally
            {
                this.list.Add(option);
            } 
        }

        public void InsertModOption(ModOption option, int index)
        {
            try
            {
                var old = this.list.Find(x => { return x.identifier == option.identifier; });
                list.Remove(old);
            }
            finally
            {
                this.list.Insert(index, option);
            }
        }

        internal ModOption[] GetArray()
        {
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
