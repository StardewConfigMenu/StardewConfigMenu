using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace StardewConfigFramework {
	public delegate void ModOptionSelectionHandler(string ComponentIdentifier, string selectionIdentifier);

	public class ModOptionSelection: ModOption {
		public event ModOptionSelectionHandler ValueChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StardewConfigFramework.ModOptionSelection"/> class.
		/// </summary>
		/// <param name="labelText">Label text.</param>
		/// <param name="identifier">Identifier.</param>
		/// <param name="choices">Choices. Must contain at least one choice.</param>
		/// <param name="defaultSelection">Default selection.</param>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public ModOptionSelection(string identifier, string labelText, ModSelectionOptionChoices choices = null, int defaultSelection = 0, bool enabled = true) : base(identifier, labelText, enabled) {
			if (choices != null) {
				this.Choices = choices;
				this.SelectionIndex = defaultSelection;
			}

		}

		public ModOptionSelection(string identifier, string labelText, ModSelectionOptionChoices choices, string defaultSelection, bool enabled = true) : base(identifier, labelText, enabled) {
			if (choices != null) {
				this.Choices = choices;
				if (choices.Count > 0)
					this.Selection = defaultSelection;
			}
		}

		public ModSelectionOptionChoices Choices { get; private set; } = new ModSelectionOptionChoices();

		public Dictionary<String, String> hoverTextDictionary = null;

		private int _SelectionIndex = 0;
		public int SelectionIndex {
			get {
				return _SelectionIndex;
			}
			set {
				if (value > ((this.Choices.Count == 0) ? this.Choices.Count : this.Choices.Count - 1) || value < 0)
					throw new IndexOutOfRangeException("Selection is out of range of Choices");

				if (_SelectionIndex != value) {
					_SelectionIndex = value;
					this.ValueChanged?.Invoke(this.identifier, this.Selection);
				}
			}
		}

		//public string Selection => Choices.IdentifierOfIndex(this._SelectionIndex);
		public string Selection {
			get {
				return Choices.IdentifierOf(this._SelectionIndex);
			}
			set {
				if (!Choices.Contains(value))
					throw new IndexOutOfRangeException("Identifier does not exist in Choices");

				if (_SelectionIndex != Choices.IndexOf(value)) {
					_SelectionIndex = Choices.IndexOf(value);
					this.ValueChanged?.Invoke(this.identifier, this.Selection);
				}
			}
		}
	}

	/// <summary>
	/// Contains the choices of a ModOptionSelection
	/// </summary>
	public class ModSelectionOptionChoices {

		private OrderedDictionary dictionary = new OrderedDictionary();
		public int Count => dictionary.Count;

		/// <summary>
		/// Gets or sets the Label with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public string this[int key] {
			get {
				return dictionary[key] as string;
			}
			set {
				dictionary[key] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Labeel with the specified identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public string this[string identifier] {
			get {
				return dictionary[identifier] as string;
			}
			set {
				dictionary[identifier] = value;
			}
		}

		public void Insert(int index, string identifier, string label) {
			dictionary.Remove(identifier);
			dictionary.Insert(index, identifier, label);
		}

		public void Add(string identifier, string label) {
			dictionary.Remove(identifier);
			dictionary.Add(identifier, label);
		}

		public void Replace(string identifier, string label) {
			var index = IndexOf(identifier);
			if (index != -1) {
				dictionary.Remove(identifier);
				dictionary.Insert(index, identifier, label);
			} else
				dictionary.Add(identifier, label);
		}

		/// <summary>
		/// Remove the specified identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public void Remove(string identifier) {
			dictionary.Remove(identifier);
		}

		public bool Contains(string identifier) {
			return dictionary.Contains(identifier);
		}

		public int IndexOfLabel(string label) {
			return this.Labels.IndexOf(label);
		}

		public string IdentifierOfLabel(string label) {
			return this.IdentifierOf(this.Labels.IndexOf(label));
		}

		/// <summary>
		/// Gets the Index of the identifier.
		/// </summary>
		/// <returns>the index, or -1 if not found.</returns>
		/// <param name="identifier">Identifier.</param>
		public int IndexOf(string identifier) {
			return this.Identifiers.IndexOf(identifier);
		}

		public string IdentifierOf(int index) {
			if (dictionary.Keys.Count == 0 || index < 0 || index >= dictionary.Keys.Count)
				return string.Empty;
			String[] myKeys = new String[dictionary.Keys.Count];
			dictionary.Keys.CopyTo(myKeys, 0);
			return myKeys[index];
		}

		public string LabelOf(int index) {
			return this[index];
		}

		public string LabelOf(string identifier) {
			return this[identifier];
		}

		public List<string> Identifiers {
			get {
				if (dictionary.Keys.Count == 0)
					return new List<string>();

				String[] myKeys = new String[dictionary.Keys.Count];
				dictionary.Keys.CopyTo(myKeys, 0);
				return new List<string>(myKeys);
			}
		}

		public List<string> Labels {
			get {
				if (dictionary.Values.Count == 0)
					return new List<string>();

				String[] myValues = new String[dictionary.Values.Count];
				dictionary.Values.CopyTo(myValues, 0);
				return new List<string>(myValues);
			}
		}
	}

}
