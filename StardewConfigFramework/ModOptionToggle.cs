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

        public bool IsOn { get; private set; }

        public void SetOff() {
            this.IsOn = false;
            this.ValueChanged(this.IsOn);
        }

		public void SetOn()
		{
			this.IsOn = true;
            this.ValueChanged(this.IsOn);
		}

		public void Toggle()
		{
			this.IsOn = !this.IsOn;
            this.ValueChanged(this.IsOn);
		}
	}
}
