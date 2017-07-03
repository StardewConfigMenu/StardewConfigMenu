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
        private ModOptionSelection ModData;

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

        public override int selectedOption
        {
            get { return this.ModData.Selection; }
            set
            {
                if (selectedOption == value || value + 1 > dropDownOptions.Count || value < 0)
                    return;
                dropDownDisplayOptions.Remove(dropDownOptions[value]);
                dropDownDisplayOptions.Insert(0, dropDownOptions[value]);
                this.ModData.Selection = value;
            }
        }

        protected override List<string> dropDownOptions {
            get {
                if (this.ModData == null) // for base
                    return new List<string>();
                return this.ModData.List.Select(x => x.label).ToList();
            }
        }

        protected override List<string> dropDownDisplayOptions
        {
            get {
                if (dropDownOptions.Count == 0)
                {
                    _dropDownDisplayOptions.Clear();
                } else
                {
                    dropDownOptions.ForEach(x => {
                        if (!_dropDownDisplayOptions.Any( y => {
                            return y.Equals(x);
                        }))
                            _dropDownDisplayOptions.Add(x);
                    });
                }
                
                dropDownBounds.Height = this.bounds.Height * this.dropDownOptions.Count;
                return _dropDownDisplayOptions;
            }
        }

        private List<string> _dropDownDisplayOptions = new List<string>();

        public ModDropDownComponent(ModOptionSelection option, int width) : base(option.LabelText, width)
        {
            this.ModData = option;

            //ReloadData();
            this.SelectDisplayedOption(option.Selection);
            // LongestString = option.List.GroupBy(str => str == null ? 0 : str.Length).OrderByDescending(g => g.Key).First();
        }

        public void ReloadData()
        {
            this.dropDownDisplayOptions.Clear();
            for (int i = 0; i < ModData.List.Count; i++)
            {
                if (i == selectedOption)
                    this.InsertOption(0, ModData.List[i].label);
                else
                    this.AddOption(ModData.List[i].label);
            }



        }

        protected override void SelectDisplayedOption(int DisplayedSelection)
        {
            if (dropDownOptions.Count == 0)
                return;

            var selected = dropDownDisplayOptions[DisplayedSelection];
            ModData.Selection = dropDownOptions.IndexOf(selected);
            base.SelectDisplayedOption(DisplayedSelection);
        }

    }
}
