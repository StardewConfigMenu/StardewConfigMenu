using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewConfigFramework;
using Microsoft.Xna.Framework.Graphics;


namespace StardewConfigMenu.Panel.Components.ModOptions
{
    internal class ModDropDownComponent : DropDownComponent
    {
        readonly private ModOptionSelection ModData;

		public override bool enabled
		{
			get
			{
				if (!ModData.enabled)
					return ModData.enabled;
				else
					return dropDownOptions.Count != 0;
			}
		}

		public override string label
		{
			get
			{
                return ModData.LabelText;
			}
		}

        public override int selectedOption
        {
            get { return this.ModData.SelectionIndex; }
            set
            {
                if (selectedOption == value)
                    return;
                
				_dropDownDisplayOptions.Insert(0, dropDownOptions[value]);
				_dropDownDisplayOptions.RemoveAt(value + 1);
                this.ModData.SelectionIndex = value;
            }
        }

        protected override List<string> dropDownOptions {
            get {
				if (this.ModData == null) // for base
					return new List<string>();
                return new List<string>(this.ModData.Choices.Labels);
            }
        }

        protected override List<string> dropDownDisplayOptions
        {
            get {
                if (ModData.Choices.Count == 0)
                {
                    _dropDownDisplayOptions.Clear();
                } else
                {
                    var options = dropDownOptions;
                    var toRemove = _dropDownDisplayOptions.Except(options).ToList();
                    var toAdd = options.Except(_dropDownDisplayOptions).ToList();

                    _dropDownDisplayOptions.RemoveAll(x => toRemove.Contains(x));
                    _dropDownDisplayOptions.AddRange(toAdd);
                }
                
                dropDownBounds.Height = this.bounds.Height * this.ModData.Choices.Count;
                return _dropDownDisplayOptions;
            }
        }

        private List<string> _dropDownDisplayOptions = new List<string>();

        public ModDropDownComponent(ModOptionSelection option, int width) : base(option.LabelText, width, option.enabled)
		{
			this.ModData = option;
		}

        public ModDropDownComponent(ModOptionSelection option, int width, int x, int y) : base(option.LabelText, width, x, y)
		{
			this.ModData = option;
		}

        protected override void SelectDisplayedOption(int DisplayedSelection)
        {
            if (this.ModData.Choices.Count == 0)
                return;

            var selected = dropDownDisplayOptions[DisplayedSelection];
            ModData.SelectionIndex = this.ModData.Choices.IndexOf(selected);
            base.SelectDisplayedOption(DisplayedSelection);
        }

    }
}
