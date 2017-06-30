﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewConfigFramework
{
	public delegate void ModOptionSelectionHandler(int selection);


	public class ModOptionSelection : ModOption
	{
		public event ModOptionSelectionHandler ValueChanged;

		public ModOptionSelection(string labelText, List<String> list, int defaultSelection = 0, bool enabled = true) : base(labelText, enabled)
		{
			this.List = list;
			this.Selection = defaultSelection;
		}

		internal List<String> List;
		public int Selection { get; private set; }

		public void MakeSelection(int selection)
		{
			this.Selection = selection;
			this.ValueChanged(this.Selection);
		}
	}
}
