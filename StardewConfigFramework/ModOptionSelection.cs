using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public ModOptionSelection(string labelText, String identifier, int defaultSelection = 0, bool enabled = true) : base(labelText, identifier, enabled)
        {
            this._Selection = defaultSelection;
        }

        public ModOptionSelection(string labelText, String identifier, List<SelectionChoice> list, int defaultSelection = 0, bool enabled = true) : base(labelText, identifier, enabled)
        {
            this.List = list;
            
            this.Selection = defaultSelection;
        }

        public List<SelectionChoice> List { get; private set; }  = new List<SelectionChoice>(); 

        private int _Selection = 0;
        public int Selection {
            get {
                return _Selection;
            }
            set {
                if (value == this.Selection || value > this.List.Count - 1)
                    return;
                _Selection = value;
                this.ValueChanged?.Invoke(value);
            }
        }

        public string SelectionIdentifier
        {
            get { return List[Selection].identifier; }
        }

        public void AddChoice(string label, string identifier)
        {
            try
            {
                var item = List.Find(x => { return x.identifier == identifier; });
                List.Remove(item);
            }
            finally
            {
                this.List.Add(new SelectionChoice(label, identifier));
            }
        }

        public void RemoveChoice(string identifier)
        {
            try
            {
                var item = List.Find(x => { return x.identifier == identifier; });
                List.Remove(item);
            }
            catch { }
        }

        public int? IndexOf(string identifier)
        {
            try
            {
                return List.FindIndex(x => { return x.identifier == identifier; });
            } catch
            {
                return null;
            }
        }

        public string IdentifierOf(int index)
        {
            return List[index].identifier;
        }

        public string LabelOf(int index)
        {
            return List[index].label;
        }

        public string LabelOf(string identifier)
        {
            var index = IndexOf(identifier);
            if (index == null)
                return null;
            else
                return List[(int) index].label;
        }
    }

    public class SelectionChoice
    {
        public SelectionChoice( string label, string identifier)
        {
            this.label = label;
            this.identifier = identifier;
        }

        public string label { get; private set; }
        public string identifier { get; private set; }
    }
}
