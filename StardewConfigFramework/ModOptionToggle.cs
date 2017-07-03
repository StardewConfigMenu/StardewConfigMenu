﻿using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewConfigFramework
{
	public delegate void ModOptionToggleHandler(bool isOn);

	public class ModOptionToggle : ModOption
	{

		public event ModOptionToggleHandler ValueChanged;

		public ModOptionToggle(string labelText, String identifier, bool isOn = true, bool enabled = true) : base(labelText, identifier, enabled)
		{
			this.IsOn = isOn;
		}

        public bool IsOn {
            get
            {
                return _isOn;
            }
            set
            {
                if (_isOn == value)
                    return;

                this._isOn = value;
                this.ValueChanged?.Invoke(this.IsOn);
            }
        }

        private bool _isOn;

		public void Toggle()
		{
			this.IsOn = !this.IsOn;
            this.ValueChanged?.Invoke(this.IsOn);
		}
	}
}
