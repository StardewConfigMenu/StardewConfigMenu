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

        public ModOptionSelection(string identifier, string label, int defaultSelection = 0, bool enabled = true) : base(identifier, label, enabled)
        {
            this._Selection = defaultSelection;
        }

        public ModOptionSelection(string identifier, string labelText,  ModSelectionOptionChoices choices, int defaultSelection = 0, bool enabled = true) : base(labelText, identifier, enabled)
        {
            this.Choices = choices;
            
            this.Selection = defaultSelection;
        }

        public ModSelectionOptionChoices Choices { get; private set; }  = new ModSelectionOptionChoices(); 

        private int _Selection = 0;
        public int Selection {
            get {
                return _Selection;
            }
            set {
                _Selection = value;
                this.ValueChanged?.Invoke(value);
            }
        }

        public string SelectionIdentifier
        {
            get { return Choices.IdentifierOf(Selection); }
        }
    }

    /// <summary>
    /// Contains the choices of a ModOptionSelection
    /// </summary>
    public class ModSelectionOptionChoices : OrderedDictionary
    {
		public new string this[int key]
		{
			get
			{
				return base[key] as string;
			}
			set
			{
				base[key] = value;
			}
		}

		public string this[string identifier]
		{
			get
			{
				return base[identifier] as string;
			}
			set
			{
				base[identifier] = value;
			}
		}

        public void Insert(int index, string identifier, string label) {
            base.Insert(index, identifier, label);
        }

        public void Add(string identifier, string label) {
            base.Remove(identifier);
            base.Add(identifier, label);
        }

        public void Remove(string identifier) {
            base.Remove(identifier);
        }

        public void Contains(string identifier) {
            base.Contains(identifier);
        }

		public int IndexOf(string label)
		{
            return this.Labels.IndexOf(label);
        }

        public string IdentifierOf(int index)
        {
            String[] myValues = new String[Keys.Count];
            Values.CopyTo(myValues, 0);
            return myValues[index];
        }

        /// <summary>
        /// Gets the Index of the identifier.
        /// </summary>
        /// <returns>the index, or -1 if not found.</returns>
        /// <param name="identifier">Identifier.</param>
		public int IndexOfIdentifier(string identifier)
		{
			return this.Identifiers.IndexOf(identifier);
		}

        public string IdentifierOfIndex(int index)
        {
            String[] myKeys = new String[Keys.Count];
            Keys.CopyTo(myKeys, 0);
            return myKeys[index];
        }

        public string LabelOf(int index)
		{
			return this[index];
		}

		public string LabelOf(string identifier)
		{
            return this[identifier];
		}

        public List<string> Identifiers {
            get {
                if (this.Keys.Count == 0)
                    return new List<string>();
                String[] identifiers = new String[this.Values.Count];
                this.Values.CopyTo(identifiers, 0);
                return new List<string>(identifiers);
            }
        }

		public List<string> Labels {
			get
			{
                if (this.Values.Count == 0)
                    return new List<string>();
                String[] values = new String[this.Values.Count];
                this.Values.CopyTo(values, 0);
                return new List<string>(values);
			}
		}

		// Blocking other options

		public new string this[object stop]
		{
			get { throw new NotImplementedException("Improper Method use in ModSelectionOptionChoices"); }
			set { throw new NotImplementedException("Improper Method use in ModSelectionOptionChoices"); }
		}

		public new void Add(object dont, object use) {
            throw new NotImplementedException("Improper Method use in ModSelectionOptionChoices");
        }

        public new void Remove(object stop) {
            throw new NotImplementedException("Improper Method use in ModSelectionOptionChoices");
        }

        public new void Insert(int index, object dont, object use) {
			throw new NotImplementedException("Improper Method use in ModSelectionOptionChoices");
		}

    }


}
