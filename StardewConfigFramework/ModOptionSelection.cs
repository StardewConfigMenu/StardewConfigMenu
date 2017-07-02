using System;
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

        public ModOptionSelection(string labelText, String identifier, List<String> list, int defaultSelection = 0, bool enabled = true) : base(labelText, identifier, enabled)
        {
            this.List = list;
            this._Selection = defaultSelection;
        }

        public List<String> List { get; private set; }
        private int _Selection;
        public int Selection {
            get {
                return _Selection;
            }
            set {
                if (value == this.Selection)
                    return;
                _Selection = value;
                this.ValueChanged?.Invoke(value);
            }
        }
    }
}
